using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.APIGatewayEvents;
using SharedLibrary;

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
        var scanConfig = new ScanOperationConfig
        {
            Limit = 10
        };

        if (request.Headers is not null && request.Headers.TryGetValue("pagination-token", out string? paginationToken))
        {
            scanConfig.PaginationToken = paginationToken;
        }

        var search = dbContext.FromScanAsync<Simulation>(scanConfig);
        var simulations = await search.GetRemainingAsync();

        var simulationPreview = new SimulationPreview()
        {
            Simulations = simulations,
            PaginationToken = search.PaginationToken,
        };

        return new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = JsonSerializer.Serialize(simulationPreview),
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
