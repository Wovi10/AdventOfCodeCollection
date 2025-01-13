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

    public static int FindEasterEgg(this IEnumerable<Robot> robots, int secondsToWait, int width, int height)
    {
        var allRobots = robots as Robot[] ?? robots.ToArray();

        var counter = 1;
        while (true)
        {
            foreach (var robot in allRobots)
                robot.Run(1, width, height);

            if (allRobots.GroupBy(r => r.Coordinate).All(g => g.Count() == 1))
            {
                Console.WriteLine($"Time: {counter}");
                PrintRobotsFormation(allRobots, width, height);
                Console.WriteLine();

                return counter;
            }

            counter++;
        }
    }

    private static void PrintRobotsFormation(Robot[] allRobots, int width, int height)
    {
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var coordinate = new Coordinate(x, y);
                var robot = allRobots.FirstOrDefault(r => r.Coordinate == coordinate);
                Console.Write(robot != null ? "#" : ".");
            }

            Console.WriteLine();
        }
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