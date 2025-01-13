namespace _2024.Models.Day14;

public static class Day14Extensions
{
    public static IEnumerable<Robot> CreateRobots(this IEnumerable<string> input)
        => input.Select(line => new Robot(line));

    public static IEnumerable<Robot> RunAll(this IEnumerable<Robot> robots, int secondsToWait, int width, int height)
    {
        var runAll = robots as Robot[] ?? robots.ToArray();
        foreach (var robot in runAll)
            robot.Run(secondsToWait, width, height);

        return runAll;
    }

    public static IEnumerable<Robot> FilterCenterRobots(this IEnumerable<Robot> robots, int width, int height)
    {
        var horizontalCenter = width / 2;
        var verticalCenter = height / 2;
        return robots.Where(r => r.Coordinate.X != horizontalCenter && r.Coordinate.Y != verticalCenter);
    }

    public static IEnumerable<IGrouping<int, Robot>> GroupByQuadrant(this IEnumerable<Robot> robots, int width, int height)
    {
        var horizontalCenter = width / 2;
        var verticalCenter = height / 2;
        return robots.GroupBy(r => r.Coordinate.X < horizontalCenter ? r.Coordinate.Y < verticalCenter ? 1 : 3 : r.Coordinate.Y < verticalCenter ? 2 : 4);
    }
}