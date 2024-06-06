using UtilsCSharp;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day17;

public class Node(int row, int column) : NodeBase<int>(row, column)
{
    public Node(int heatLoss, int row, int column, int directionRow, int directionColumn, int timesInDirection): this
        (row, column)
    {
        HeatLoss = heatLoss;
        DirectionRow = directionRow;
        DirectionColumn = directionColumn;
        TimesInDirection = timesInDirection;
    }

    public int HeatLoss { get; set; }
    public int DirectionRow { get; set; }
    public int DirectionColumn { get; set; }
    public int TimesInDirection { get; set; }
    public (int, int) Coordinates => (X, Y);

    public bool IsValid(int height, int width)
        => X.IsBetween(0, height, true, false) &&
           Y.IsBetween(0, width, true, false);

    public bool IsStandingStill()
        => DirectionRow == 0 && DirectionColumn == 0;

    public override bool Equals(object? obj)
    {
        return obj is Node node && Equals(node);
    }

    private bool Equals(Node other)
    {
        return X == other.X && Y == other.Y &&
               DirectionRow == other.DirectionRow && DirectionColumn == other.DirectionColumn &&
               TimesInDirection == other.TimesInDirection;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, DirectionRow, DirectionColumn, TimesInDirection);
    }

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}