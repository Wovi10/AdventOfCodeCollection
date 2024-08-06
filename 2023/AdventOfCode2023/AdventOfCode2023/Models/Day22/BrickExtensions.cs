namespace AdventOfCode2023_1.Models.Day22;

public static class BrickExtensions
{
    public static bool IntersectsWith(this Range a, Range b)
        => a.Start.Value <= b.End.Value && b.Start.Value <= a.End.Value;
}