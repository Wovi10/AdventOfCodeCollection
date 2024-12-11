namespace _2024.Models.Day06;

public static class Day06Extensions
{
    public static Coordinate ToCoordinate(this (int, int) input)
        => new(input.Item1, input.Item2);
}