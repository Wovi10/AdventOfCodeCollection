using UtilsCSharp;
using UtilsCSharp.Enums;
using UtilsCSharp.Utils;

namespace _2024.Models.Day14;

public class Robot
{
    public Coordinate Coordinate { get; set; }
    public Velocity Velocity { get; set; }

    public Robot(string input)
    {
        var parts = input.Split(Constants.Space);
        Coordinate = new Coordinate(parts.First().Split("=").Last());
        Velocity = new Velocity(parts.Last().Split("=").Last());
    }

    public void Run(int secondsToWait, int width, int height)
    {
        var newX = RunHorizontal(secondsToWait, width);
        var newY = RunVertical(secondsToWait, height);

        Coordinate = new Coordinate(newX, newY);
    }

    private int RunVertical(int secondsToWait, int height)
        => RunDirection(secondsToWait, height, Velocity.Vertical, Coordinate.Y);

    private int RunHorizontal(int secondsToWait, int width)
        => RunDirection(secondsToWait, width, Velocity.Horizontal, Coordinate.X);

    private static int RunDirection(int secondsToWait, int limit, int velocity, int initialCoordinate)
    {
        var timesToRun = secondsToWait % limit;
        var result = initialCoordinate;

        for (var i = 0; i < timesToRun; i++)
        {
            result = result.Add(velocity);
            if (result < 0)
                result += limit;
            else if (result >= limit)
                result -= limit;
        }

        return result;
    }
}