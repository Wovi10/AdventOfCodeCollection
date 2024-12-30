using AOC.Utils;

namespace _2024.Models.Day13;

public class ArcadePrize
{
    public long X { get; set; }
    public long Y { get; set; }

    public ArcadePrize(string line)
    {
        var usefulData = line.Split(": ").Last();
        var parts = usefulData.Split(", ");
        var xPart = parts.First().Split("=").Last();
        var yPart = parts.Last().Split("=").Last();

        X = long.Parse(xPart);
        Y = long.Parse(yPart);
    }

    public override string ToString() => $"({X}, {Y})";
}