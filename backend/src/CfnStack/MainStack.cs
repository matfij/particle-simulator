using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Constructs;

namespace CfnStack;

public class MainStack : Stack
{
    internal MainStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        #region Upload Lambda
        var uploadLambda = new Function(this, "upload-lambda", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Handler = "UploadLambda",
            Code = Code.FromAsset("./src/UploadLambda/bin/Release/net8.0/UploadLambda.zip"),
            Timeout = Duration.Seconds(30),
        });
        #endregion

        #region Preview Lambda
        var previewLambda = new Function(this, "preview-lambda", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Handler = "PreviewLambda",
            Code = Code.FromAsset("./src/PreviewLambda/bin/Release/net8.0/PreviewLambda.zip"),
            Timeout = Duration.Seconds(30),
        });
        #endregion

        #region Download Lambda
        var downloadLambda = new Function(this, "download-lambda", new FunctionProps
        {
            Runtime = Runtime.DOTNET_8,
            Handler = "DownloadLambda",
            Code = Code.FromAsset("./src/DownloadLambda/bin/Release/net8.0/DownloadLambda.zip"),
            Timeout = Duration.Seconds(30),
        });
        #endregion

        #region Simulation DynamoDB Table
        var simulationTable = new Table(this, "simulation-table", new TableProps
        {
            TableName = "Simulation",
            PartitionKey = new Attribute
            {
                Name = "Id",
                Type = AttributeType.STRING,
            },
            RemovalPolicy = RemovalPolicy.DESTROY, // RETAIN in production
            BillingMode = BillingMode.PAY_PER_REQUEST,
        });

        simulationTable.GrantWriteData(uploadLambda);
        simulationTable.GrantReadData(previewLambda);
        simulationTable.GrantReadWriteData(downloadLambda);
        #endregion

        #region Simulation S3 Bucket
        var simulationBucketProps = new BucketProps
        {
            BucketName = "particle-simulation-bucket",
            RemovalPolicy = RemovalPolicy.DESTROY,
            BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
            EnforceSSL = true,
        };
        var simulationBucket = new Bucket(this, "particle-simulation-bucket", simulationBucketProps);

        simulationBucket.GrantWrite(uploadLambda);
        simulationBucket.GrantRead(downloadLambda);
        #endregion

        #region API Gateway
        var api = new RestApi(this, "simulation-api", new RestApiProps
        {
            RestApiName = "Particle Simulator Gateway",
            EndpointConfiguration = new EndpointConfiguration
            {
                Types = [EndpointType.REGIONAL]
            }
        });

        var uploadLambdaIntegration = new LambdaIntegration(uploadLambda);
        api.Root.AddResource("upload").AddMethod("POST", uploadLambdaIntegration);

        var previewLambdaIntegration = new LambdaIntegration(previewLambda);
        api.Root.AddResource("preview").AddMethod("GET", previewLambdaIntegration);

        var downloadLambdaIntegration = new LambdaIntegration(downloadLambda);
        api.Root.AddResource("download").AddMethod("POST", downloadLambdaIntegration);
        #endregion
    }
}
