using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day17;

public class Node
{
    public Node()
    {
    }

    public Node(int heatLoss, int row, int column, int directionRow, int directionColumn, int timesInDirection)
    {
        HeatLoss = heatLoss;
        Row = row;
        Column = column;
        DirectionRow = directionRow;
        DirectionColumn = directionColumn;
        TimesInDirection = timesInDirection;
    }

    public int HeatLoss { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public int DirectionRow { get; set; }
    public int DirectionColumn { get; set; }
    public int TimesInDirection { get; set; }

    public bool IsValid(int height, int width)
        => Row.IsBetween(0, height, true, false) && 
           Column.IsBetween(0, width, true, false);

    public bool IsStandingStill()
        => DirectionRow == 0 && DirectionColumn == 0;

    public bool IsWithinConstraints(Constraints constraints)
    {
        return TimesInDirection >= constraints.MinNumberOfMovements &&
               TimesInDirection < constraints.MaxNumberOfMovements;
    }


    public override bool Equals(object? obj)
    {
        return obj is Node node && Equals(node);
    }

    private bool Equals(Node other)
    {
        return Row == other.Row && Column == other.Column &&
               DirectionRow == other.DirectionRow && DirectionColumn == other.DirectionColumn &&
               TimesInDirection == other.TimesInDirection;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column, DirectionRow, DirectionColumn, TimesInDirection);
    }
}