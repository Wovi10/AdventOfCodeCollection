namespace _2024.Models.Day10;

public class Trail(List<Coordinate> path)
{
    public List<Coordinate> Path { get; } = path;

    public Trail(Trail trail) : this(new List<Coordinate>(trail.Path))
    {
    }
}