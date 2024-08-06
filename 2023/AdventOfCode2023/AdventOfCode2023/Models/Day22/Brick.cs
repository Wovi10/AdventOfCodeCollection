using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day22;

public class Brick
{
    public Brick(string input)
    {
        var parts = input.Split('~');
        StartCube = new Cube
        {
            X = int.Parse(parts[0].Split(',')[0]),
            Y = int.Parse(parts[0].Split(',')[1]),
            Z = int.Parse(parts[0].Split(',')[2])
        };
        EndCube = new Cube
        {
            X = int.Parse(parts[1].Split(',')[0]),
            Y = int.Parse(parts[1].Split(',')[1]),
            Z = int.Parse(parts[1].Split(',')[2])
        };

        IsValid = StartCube.Z != 0 && EndCube.Z != 0;
    }

    public Cube StartCube { get; set; }
    public Cube EndCube { get; set; }

    public bool IsValid { get; set; }

    public int LowestZ => Comparisons.GetLowest(StartCube.Z, EndCube.Z);
    public int HighestZ => Comparisons.GetHighest(StartCube.Z, EndCube.Z);

    public List<Brick> BricksBelow { get; set; } = new();
    public List<Brick> BricksAbove { get; set; } = new();

    public bool IsAtBottom => LowestZ == 1;

    public void MoveDown()
    {
        StartCube.Z--;
        EndCube.Z--;
    }
    
    public bool IntersectsXY(Brick brick)
    {
        var xRangeStart = new Range(StartCube.X, EndCube.X);
        var xRangeEnd = new Range(brick.StartCube.X, brick.EndCube.X);
        var xIntersects = xRangeStart.IntersectsWith(xRangeEnd);
        
        var yRangeStart = new Range(StartCube.Y, EndCube.Y);
        var yRangeEnd = new Range(brick.StartCube.Y, brick.EndCube.Y);
        var yIntersects = yRangeStart.IntersectsWith(yRangeEnd);

        return xIntersects && yIntersects;
    }
    
    public bool IsSupporting()
        => BricksAbove.Count > 0;

    public override string ToString() 
        => $"{StartCube}~{EndCube}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var brick = (Brick) obj;
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