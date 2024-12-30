using AOC.Utils;

namespace _2024.Models.Day13;

public class ArcadeButton
{
    public long X { get; set; }
    public long Y { get; set; }

    public ArcadeButton(string line)
    {
        var useful = line.Split(": ").Last().Split(", ");
        var xPart = useful.First().Split("+").Last().Trim();
        var yPart = useful.Last().Split("+").Last().Trim();

        X = long.Parse(xPart);
        Y = long.Parse(yPart);
    }

    public override string ToString() => $"({X}, {Y})";
}