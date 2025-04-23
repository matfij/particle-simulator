using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using UploadLambda;

var handler = async (Simulation input, ILambdaContext context) =>
{
    var dbClient = new AmazonDynamoDBClient();
    var dbContext = new DynamoDBContext(dbClient);

    await dbContext.SaveAsync(input);

    return "SUCCESS";
};

await LambdaBootstrapBuilder.Create(handler, new DefaultLambdaJsonSerializer())
        .Build()
        .RunAsync();
