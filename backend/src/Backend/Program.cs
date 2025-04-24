using Amazon.CDK;

namespace Backend
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new BackendStack(app, "ParticleSimulatorStack", new StackProps { });
            app.Synth();
        }
    }
}
