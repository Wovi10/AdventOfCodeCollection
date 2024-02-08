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

    public override bool Equals(object? obj)
    {
        if (obj is Coordinates coordinates)
            return Equals(coordinates);

        return false;
    }

    protected bool Equals(Coordinates other)
    {
        return XCoordinate == other.XCoordinate && YCoordinate == other.YCoordinate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(XCoordinate, YCoordinate);
    }

    public override string ToString()
    {
        return $"({XCoordinate},{YCoordinate})";
    }
}