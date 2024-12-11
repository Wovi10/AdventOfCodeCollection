using UtilsCSharp.Enums;
using UtilsCSharp.Utils;

namespace _2024.Models.Day06;

public class Position
{
    public Position(Position mapPosition)
    {
        Coordinate = mapPosition.Coordinate;
        IsObstacle = mapPosition.IsObstacle;
        IsGuard = mapPosition.IsGuard;
        IsVisited = mapPosition.IsVisited;
        WaysVisited = mapPosition.WaysVisited;
    }

    public Position(Coordinate coordinate, char character)
    {
        Coordinate = coordinate;
        IsObstacle = character == Constants.HashTag[0];
        IsGuard = character == '^';
    }

    public Coordinate Coordinate { get; }
    public bool IsObstacle { get; private set; }
    public bool IsGuard { get; }
    public bool IsVisited { get; private set; }

    public IDictionary<Direction, int> WaysVisited { get; set; } = new Dictionary<Direction, int>();
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