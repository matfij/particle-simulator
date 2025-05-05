using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
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
            #region Upload Lambda
            var uploadLambda = new Function(this, "upload-lambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                Handler = "UploadLambda",
                Code = Code.FromAsset("./src/UploadLambda/bin/Release/net8.0/UploadLambda.zip"),
                Timeout = Duration.Seconds(30)
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

            simulationTable.GrantReadWriteData(uploadLambda);
            #endregion

            #region Simulation S3 Bucket
            var simulationBucketProps = new BucketProps
            {
                BucketName="particle-simulation-bucket",
                RemovalPolicy= RemovalPolicy.DESTROY,
                Encryption = BucketEncryption.S3_MANAGED,
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL,
                EnforceSSL = true,
            };
            var simulationBucket = new Bucket(this, "particle-simulation-bucket", simulationBucketProps);

            simulationBucket.GrantWrite(uploadLambda);
            #endregion

            #region API Gateway
            var api = new RestApi(this, "simulation-api", new RestApiProps
            {
                RestApiName = "Particle Simulator Gateway",
                ApiKeySourceType = ApiKeySourceType.HEADER,
            });

            var plan = api.AddUsagePlan("api-usage-plan", new UsagePlanProps
            {
                Name = "UsagePlan",
                Throttle = new ThrottleSettings
                {
                    RateLimit = 5,
                    BurstLimit = 10,
                }
            });

            plan.AddApiStage(new UsagePlanPerApiStage
            {
                Stage = api.DeploymentStage,
            });

            var uploadLambdaIntegration = new LambdaIntegration(uploadLambda);

            api.Root.AddResource("upload").AddMethod("POST", uploadLambdaIntegration, new MethodOptions
            {
                ApiKeyRequired = true,
            });
            #endregion
        }
    }
}
