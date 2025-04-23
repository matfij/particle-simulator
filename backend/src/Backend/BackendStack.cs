using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace Backend
{
    public class BackendStack : Stack
    {
        internal BackendStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var uploadLambdaProps = new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "UploadLambda",
                Code = Code.FromAsset("./src/UploadLambda/bin/Release/net8.0/UploadLambda.zip"),
            };
            var uploadLambda = new Function(this, "upload-lambda", uploadLambdaProps);

            var simulationTableProps = new TableProps
            {
                TableName = "Simulation",
                PartitionKey = new Attribute
                {
                    Name = "Id",
                    Type = AttributeType.STRING,
                },
                RemovalPolicy = RemovalPolicy.DESTROY, // RETAIN in production
            };
            var simulationTable = new Table(this, "simulation-table", simulationTableProps);

            simulationTable.GrantReadWriteData(uploadLambda);
        }
    }
}
