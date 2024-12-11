using UtilsCSharp.Utils;

namespace _2024.Models.Day06;

public class Position(Coordinate coordinate, char character)
{
    public Coordinate Coordinate { get; } = coordinate;
    public bool IsObstacle { get; } = character == Constants.HashTag[0];
    public bool IsGuard { get; } = character == '^';
    public bool IsVisited { get; private set; }

    public void SetVisited() => IsVisited = true;
}