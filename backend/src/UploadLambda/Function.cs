using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using UploadLambda;

var dbClient = new AmazonDynamoDBClient();
var dbContext = new DynamoDBContext(dbClient);

var handler = async (Simulation input, ILambdaContext context) =>
{
    await dbContext.SaveAsync(input);
    return input.Id;
};

await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();
