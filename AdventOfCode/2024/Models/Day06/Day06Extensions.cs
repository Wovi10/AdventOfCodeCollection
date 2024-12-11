namespace _2024.Models.Day06;

public static class Day06Extensions
{
    public static Map ToMap(this List<string> input)
        => new(input);

    public static Map StartRunning(this Map map)
    {
        map.Run(out _);
        return map;
    }

    public static Coordinate ToCoordinate(this (int, int) input)
        => new(input.Item1, input.Item2);

    public static long GetNumWorkingPositions(this Map originalMap)
    {
        var positionsToTry = originalMap.GetOriginalPath();
        var numWorkingPositions = 0;
        var counter = 0;

        foreach (var position in positionsToTry.Where(p => !p.IsGuard))
        {
            position.SetObstacle(true);
            originalMap.AddObstacle(position.Coordinate);
            originalMap.Run(out var looped);
            if (looped)
                numWorkingPositions++;

            originalMap.RemoveObstacle(position.Coordinate);
            position.SetObstacle(false);
            originalMap.Reset();
            counter++;
        }

        return numWorkingPositions;
    }
}