namespace _2024.Models.Day14;

public static class Day14Extensions
{
    public static IEnumerable<Robot> CreateRobots(this IEnumerable<string> input)
    {
        foreach (var line in input)
        {
            yield return new Robot(line);
        }
    }

    public static IEnumerable<Robot> RunAll(this IEnumerable<Robot> robots, int secondsToWait, int width, int height)
    {
        var runAll = robots as Robot[] ?? robots.ToArray();
        foreach (var robot in runAll)
        {
            robot.Run(secondsToWait, width, height);
        }

        return runAll;
    }
}