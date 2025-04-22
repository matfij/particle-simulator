using Amazon.CDK;

namespace Backend
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new BackendStack(app, "BackendStack", new StackProps { });
            app.Synth();
        }
    }
}
