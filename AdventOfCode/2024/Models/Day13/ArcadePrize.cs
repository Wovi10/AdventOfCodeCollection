using AOC.Utils;

namespace _2024.Models.Day13;

public class ArcadePrize
{
    public long X { get; }
    public long Y { get; }

    public ArcadePrize(string line)
    {
        var usefulData = line.Split(": ").Last();
        var parts = usefulData.Split(", ");
        var xPart = parts.First().Split("=").Last();
        var yPart = parts.Last().Split("=").Last();

        var modifier = Variables.RunningPartOne ? 0 : 10000000000000;

        X = long.Parse(xPart) + modifier;
        Y = long.Parse(yPart) + modifier;
    }

    public override string ToString() => $"({X}, {Y})";
}