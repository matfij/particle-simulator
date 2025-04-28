using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.S3;
using Amazon.S3.Model;
using FileUploadLambda;

var bucketName = "simulation-bucket";
var s3Client = new AmazonS3Client();

var handler = async (FileUploadRequest input, ILambdaContext context) =>
{
    var request = new GetPreSignedUrlRequest
    {
        BucketName = bucketName,
        Key = input.FileName,
        Verb = HttpVerb.PUT,
        Expires = DateTime.UtcNow.AddMinutes(15),
        ContentType = input.ContentType,
    };

    var url = s3Client.GetPreSignedURL(request);

    return new FileUploadResponse
    {
        UploadUrl = url,
        FileKey = input.FileName,
    };
};

await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();
