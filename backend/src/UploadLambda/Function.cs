using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.S3;
using Amazon.S3.Model;
using UploadLambda;

var bucketName = "particle-simulation-bucket";
var s3Client = new AmazonS3Client();

var dbClient = new AmazonDynamoDBClient();
var dbContext = new DynamoDBContext(dbClient);

var handler = async (FileUploadRequest input, ILambdaContext context) =>
{
    var fileName = $"{input.Name.ToLower().Replace(" ", "-")}-${Guid.NewGuid()}.json";

    var simulation = new Simulation()
    {
        Id = Guid.NewGuid().ToString(),
        Name = input.Name,
        FileName = fileName,
        Downloads = 0,
    };

    await dbContext.SaveAsync(simulation);

    var request = new GetPreSignedUrlRequest
    {
        BucketName = bucketName,
        Key = fileName,
        Verb = HttpVerb.PUT,
        Expires = DateTime.UtcNow.AddMinutes(15),
        ContentType = input.ContentType,
    };

    var url = s3Client.GetPreSignedURL(request);

    return new FileUploadResponse
    {
        UploadUrl = url,
        FileKey = fileName,
    };
};

await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();
