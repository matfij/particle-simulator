namespace SimulatorTests;

internal class TestUtils
{
    public static bool CloseTo(float target, float value, float delta)
    {
        return Math.Abs(target - value) < delta;
    }
}
