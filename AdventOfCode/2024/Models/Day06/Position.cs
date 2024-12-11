using UtilsCSharp.Enums;
using UtilsCSharp.Utils;

namespace _2024.Models.Day06;

public class Position(Coordinate coordinate, char character)
{
    public Coordinate Coordinate { get; } = coordinate;
    public bool IsObstacle { get; private set; } = character == Constants.HashTag[0];
    public bool IsGuard { get; } = character == '^';
    public bool IsVisited { get; private set; }

    private IDictionary<Direction, int> WaysVisited { get; } = new Dictionary<Direction, int>();
    public void SetVisited(bool isVisited) => IsVisited = isVisited;
    public void SetObstacle(bool isObstacle) => IsObstacle = isObstacle;
    public bool IsVisitedTwice => WaysVisited.Any(w => w.Value > 1);

    public void SetWayVisited(Direction direction)
    {
        if (WaysVisited.TryGetValue(direction, out var value))
            WaysVisited[direction] = value + 1;
        else
            WaysVisited.Add(direction, 1);
    }

    public void Reset()
    {
        IsVisited = IsGuard;
        WaysVisited.Clear();
    }
}