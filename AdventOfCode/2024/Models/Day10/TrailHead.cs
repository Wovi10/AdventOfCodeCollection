namespace _2024.Models.Day10;

public class TrailHead(Coordinate currentCoordinate)
{
    public Coordinate Coordinate { get; } = currentCoordinate;
    public List<Coordinate> TrailEnds { get; } = new();
}