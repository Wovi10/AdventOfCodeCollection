namespace _2024.Models.Day17;

public static class Day17Extensions
{
    public static Computer InitializeComputer(this IEnumerable<string> input)
    {
        return new Computer(input.ToArray());
    }
}