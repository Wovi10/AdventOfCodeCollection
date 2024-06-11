using UtilsCSharp;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day17;

public class Node(int column, int row) : NodeBase<int>(column, row)
{
    public Node(int heatLoss, int column, int row, int directionColumn, int directionRow, int timesInDirection): this
        (column, row)
    {
        HeatLoss = heatLoss;
        Direction = (directionColumn, directionRow).ToDirection();
        TimesInDirection = timesInDirection;
    }

    public int HeatLoss { get; set; }
    public int TimesInDirection { get; set; }
    public Direction Direction { get; set; }

    public bool IsValid(int height, int width)
        => X.IsBetween(0, height, true, false) &&
           Y.IsBetween(0, width, true, false);

    public bool IsStandingStill()
        => Direction == Direction.None;

    public override bool Equals(object? obj) 
        => obj is Node node && Equals(node);

    private bool Equals(Node other)
    {
        return X == other.X && Y == other.Y &&
               Direction == other.Direction &&
               TimesInDirection == other.TimesInDirection;
    }

    public override int GetHashCode() 
        => HashCode.Combine(X, Y, Direction, TimesInDirection);

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}