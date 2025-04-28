using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
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
                Timeout = Duration.Seconds(30)
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
                BillingMode = BillingMode.PAY_PER_REQUEST,
            };
            var simulationTable = new Table(this, "simulation-table", simulationTableProps);

            simulationTable.GrantReadWriteData(uploadLambda);

            var fileUploadLambdaProps = new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "fileUploadLambda",
                Code = Code.FromAsset("./src/FileUploadLambda/bin/Release/net8.0/FileUploadLambda.zip"),
                Timeout = Duration.Seconds(600)
            };
            var fileUploadLambda = new Function(this, "file-upload-lambda", fileUploadLambdaProps);

            var simulationBucketProps = new BucketProps
            {
                BucketName="simulation-bucket",
                RemovalPolicy= RemovalPolicy.DESTROY,
            };
            var simulationBucket = new Bucket(this, "simulation-bucket", simulationBucketProps);

            simulationBucket.GrantWrite(fileUploadLambda);
        }
    }
}
