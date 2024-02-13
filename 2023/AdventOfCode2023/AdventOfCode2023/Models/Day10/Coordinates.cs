namespace AdventOfCode2023_1.Models.Day10;

public class Coordinates
{
    public Coordinates(int xCoordinate, int yCoordinate)
    {
        _xCoordinate = xCoordinate;
        _yCoordinate = yCoordinate;
    }

    private readonly int _xCoordinate;
    private readonly int _yCoordinate;

    public int GetYCoordinate()
        => _yCoordinate;

    public int GetXCoordinate()
        => _xCoordinate;

    public override bool Equals(object? obj)
    {
        if (obj is Coordinates coordinates)
            return Equals(coordinates);

        return false;
    }

    public bool Equals(Coordinates other)
        => _xCoordinate == other._xCoordinate && _yCoordinate == other._yCoordinate;

    public override int GetHashCode()
        => HashCode.Combine(_xCoordinate, _yCoordinate);
}