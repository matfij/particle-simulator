namespace SimulatorEngine;

internal class Utils
{
    private static readonly Random RandomFactory = new Random();

    public static int RandRange(int min, int max) => RandomFactory.Next(min, max);
}
