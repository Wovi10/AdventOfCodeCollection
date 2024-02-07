namespace AdventOfCode2023_1.Models.Day10;

public class Coordinates
{
    public Coordinates(int xCoordinate, int yCoordinate)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordinate;
    }

    public readonly int XCoordinate;
    public readonly int YCoordinate;

    public override string ToString()
    {
        return $"({XCoordinate},{YCoordinate})";
    }
}