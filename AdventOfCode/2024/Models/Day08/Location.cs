namespace _2024.Models.Day08;

public class Location
{
    public int? Id { get; init; }
    public Coordinate Coordinate { get; init; }
    public bool IsAntenna { get; init; }
    public char? Frequency { get; init; }

    public (int, int) DistanceTo(Location other)
        => Coordinate.DistanceTo(other.Coordinate);

    public Coordinate Move((int, int) distance)
        => Coordinate.Move(distance);
}