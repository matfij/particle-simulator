using System.Net;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.S3;
using SharedLibrary;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.S3.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

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
        SimulationDownloadRequest input;
        try
        {
            input = JsonSerializer.Deserialize<SimulationDownloadRequest>(request.Body);
        }
        catch (Exception ex) when (ex is JsonException || ex is ArgumentNullException)
        {
            context.Logger.LogError($"Invalid request body: {ex}");
            var error = new ApiError { Message = $"Invalid request body" };
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = JsonSerializer.Serialize(error),
                Headers = httpHeaders,
            };
        }

        var simulation = await dbContext.LoadAsync<Simulation>(input.SimulationId);

        if (simulation == null)
        {
            context.Logger.LogError($"Simulation not found: {input.SimulationId}");
            var error = new ApiError { Message = $"Simulation not found: {input.SimulationId}" };
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = JsonSerializer.Serialize(error),
                Headers = httpHeaders,
            };
        }

        simulation.Downloads += 1;
        await dbContext.SaveAsync(simulation);

        var s3Request = new GetPreSignedUrlRequest
        {
            BucketName = bucketName,
            Key = simulation.FileName,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(5),
        };

        var url = s3Client.GetPreSignedURL(s3Request);

        var response = new SimulationDownloadResponse
        {
            DownloadUrl = url,
        };

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonSerializer.Serialize(response),
            Headers = httpHeaders,
        };
    }
    catch (Exception ex)
    {
        context.Logger.LogError($"Error processing request: {ex}");
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
