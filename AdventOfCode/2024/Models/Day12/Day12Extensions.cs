namespace _2024.Models.Day12;

public static class Day12Extensions
{
    public static Garden ToGarden(this IEnumerable<string> input)
        => new(input.ToList());
}