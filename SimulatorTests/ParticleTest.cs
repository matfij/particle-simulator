using SimulatorEngine;

namespace SimulatorTests
{
    public class ParticleTest
    {
        [Fact]
        public void Should_Create_SandParticle()
        {
            var particle = new SandParticle(10, 40);

            Assert.NotNull(particle);
            Assert.Equal(10, particle.X);
            Assert.Equal(40, particle.Y);
            Assert.Equal(20, particle.Temperature);
            Assert.Equal(1442, particle.GetDensity());
            Assert.Equal(0xEFEBF01, (float)particle.GetColor());
        }

        [Fact]
        public void Should_Create_WaterParticle()
        {
            var particle = new WaterParticle(30, 50);

            Assert.NotNull(particle);
            Assert.Equal(30, particle.X);
            Assert.Equal(50, particle.Y);
            Assert.Equal(20, particle.Temperature);
            Assert.Equal(1000, particle.GetDensity());
            Assert.Equal(0x1CA3EC, (float)particle.GetColor());
        }

        [Fact]
        public void Should_Create_IronParticle()
        {
            var particle = new IronParticle(500, 300);

            Assert.NotNull(particle);
            Assert.Equal(500, particle.X);
            Assert.Equal(300, particle.Y);
            Assert.Equal(20, particle.Temperature);
            Assert.Equal(7800, particle.GetDensity());
            Assert.Equal(0xA19D94, (float)particle.GetColor());
        }

        [Fact]
        public void Should_Create_OxygenParticle()
        {
            var particle = new OxygenParticle(2, 7);

            Assert.NotNull(particle);
            Assert.Equal(2, particle.X);
            Assert.Equal(7, particle.Y);
            Assert.Equal(20, particle.Temperature);
            Assert.Equal(1.4, particle.GetDensity());
            Assert.Equal(0xA19D94, (float)particle.GetColor());
        }
    }
}
