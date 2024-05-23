namespace AdventOfCode2023_1.Shared.Types;

public class Point2D(int internalX, int internalY)
{
    public int X { get; } = internalX;
    public int Y { get; } = internalY;

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point2D point)
            return Equals(point);

        return false;
    }

    public bool Equals(Point2D other)
        => X == other.X && Y == other.Y;

    public override int GetHashCode()
        => HashCode.Combine(X, Y);
}