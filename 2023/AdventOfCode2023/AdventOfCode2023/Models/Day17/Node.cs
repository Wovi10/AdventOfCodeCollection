using System.Numerics;
using UtilsCSharp;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day17;

public class Node<T>(T row, T column) : NodeBase<T>(row, column) where T: ISignedNumber<T>
{
    public Node(int heatLoss, T row, T column, int directionRow, int directionColumn, int timesInDirection): this(row, column)
    {
        HeatLoss = heatLoss;
        DirectionRow = directionRow;
        DirectionColumn = directionColumn;
        TimesInDirection = timesInDirection;
    }

    public int HeatLoss { get; set; }
    private int Row { get; set; }
    private int Column { get; set; }
    public int DirectionRow { get; set; }
    public int DirectionColumn { get; set; }
    public int TimesInDirection { get; set; }
    public (int, int) Coordinates => (Row, Column);

    public bool IsValid(int height, int width)
        => Row.IsBetween(0, height, true, false) &&
           Column.IsBetween(0, width, true, false);

    public bool IsStandingStill()
        => DirectionRow == 0 && DirectionColumn == 0;

    public override bool Equals(object? obj)
    {
        return obj is Node<T> node && Equals(node);
    }

    private bool Equals(Node<T> other)
    {
        return Row == other.Row && Column == other.Column &&
               DirectionRow == other.DirectionRow && DirectionColumn == other.DirectionColumn &&
               TimesInDirection == other.TimesInDirection;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column, DirectionRow, DirectionColumn, TimesInDirection);
    }

    public override (T, T) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}