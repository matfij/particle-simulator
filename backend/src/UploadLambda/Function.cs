using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.APIGatewayEvents;
using UploadLambda;
using System.Net;

var bucketName = "particle-simulation-bucket";
var s3Client = new AmazonS3Client();

var dbClient = new AmazonDynamoDBClient();
var dbContext = new DynamoDBContext(dbClient);

var httpHeaders = new Dictionary<string, string>
{
    { "Content-Type", "application/json" }
};

var handler = async (APIGatewayProxyRequest request, ILambdaContext context) =>
{
    try
    {
        FileUploadRequest? input;
        try
        {
            input = JsonSerializer.Deserialize<FileUploadRequest>(request.Body);
        }
        catch (Exception e) when (e is JsonException || e is ArgumentNullException)
        {
            input = null;
        }
        if (input == null)
        {
            var error = new ApiError { Message = "Invalid request body" };
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = JsonSerializer.Serialize(error),
                Headers = httpHeaders,
            };
        }

        var fileName = $"{input.Name.ToLower().Replace(" ", "-")}-{Guid.NewGuid()}.json";

        var simulation = new Simulation()
        {
            Id = Guid.NewGuid().ToString(),
            Name = input.Name,
            FileName = fileName,
            Downloads = 0,
        };

        await dbContext.SaveAsync(simulation);

        var s3Request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = fileName,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddMinutes(15),
            ContentType = input.ContentType,
        };

        var url = s3Client.GetPreSignedURL(s3Request);

        var response = new FileUploadResponse
        {
            UploadUrl = url,
            FileKey = fileName,
        };

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonSerializer.Serialize(response),
            Headers = httpHeaders,
        };
    }
    catch
    {
        var error = new ApiError { Message = $"Unexpected error occurred" };
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            Body = JsonSerializer.Serialize(error),
            Headers = httpHeaders,
        };
    }
    
};

await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();
