namespace SimulatorTests;

internal class TestUtils
{
    public static bool CloseTo(float target, float value, float delta = 0.0001f)
    {
        return Math.Abs(target - value) < delta;
    }
}
