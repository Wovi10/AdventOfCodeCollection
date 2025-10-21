namespace _2024.Models.Day19;

public static class Day19Extensions
{
    public static TowelDesignIssue ToTowelDesignIssue(this IEnumerable<string> input)
        => new(input.ToArray());
}