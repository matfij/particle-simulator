using Amazon.CDK;

namespace CfnStack;

static class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        new MainStack(app, "ParticleSimulatorStack", new StackProps { });
        app.Synth();
    }
}
