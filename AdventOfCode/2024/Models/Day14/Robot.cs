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
        for (var i = 0; i < secondsToWait; i++)
        {

        }
    }
}