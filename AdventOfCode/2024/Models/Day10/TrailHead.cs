namespace _2024.Models.Day10;

public class TrailHead(Coordinate currentCoordinate)
{
    public Coordinate Coordinate { get; init; } = currentCoordinate;
    public List<Coordinate> TrailEnds { get; set; } = new();
}