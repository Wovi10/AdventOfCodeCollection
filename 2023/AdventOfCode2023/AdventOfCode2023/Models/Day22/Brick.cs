using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day22;

public class Brick
{
    public Brick(string input)
    {
        var parts = input.Split('~');

        StartCube = new Cube(parts.First());
        EndCube = new Cube(parts.Last());

        IsValid = StartCube.Height != 0 && EndCube.Height != 0;
    }

    public Cube StartCube { get; set; }
    public Cube EndCube { get; set; }

    private Range WidthRange => new(StartCube.Width, EndCube.Width);
    private Range DepthRange => new(StartCube.Depth, EndCube.Depth);

    public bool IsValid { get; set; }

    public int Lowest => Comparisons.GetLowest(StartCube.Height, EndCube.Height);
    public int Highest => Comparisons.GetHighest(StartCube.Height, EndCube.Height);

    public List<Brick> BricksBelow { get; set; } = new();
    public List<Brick> BricksAbove { get; set; } = new();

    public bool IsAtBottom => Lowest == 1;

    public void MoveDown()
    {
        StartCube.Height--;
        EndCube.Height--;
    }

    public bool IntersectsOnX_Y(Brick brick) =>
        WidthRange.IntersectsWith(brick.WidthRange) &&
        DepthRange.IntersectsWith(brick.DepthRange);

    public override string ToString()
        => $"{StartCube}~{EndCube}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var brick = (Brick)obj;
        return StartCube.Equals(brick.StartCube) && EndCube.Equals(brick.EndCube);
    }

    protected bool Equals(Brick other)
    {
        return StartCube.Equals(other.StartCube) && EndCube.Equals(other.EndCube);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartCube, EndCube);
    }
}