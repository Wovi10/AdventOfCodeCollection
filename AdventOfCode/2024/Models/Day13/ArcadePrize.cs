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

        var modifier = Constants.IsRealExercise && !Variables.RunningPartOne ? 10000000000000 : 0;

        X = long.Parse(xPart) + modifier;
        Y = long.Parse(yPart) + modifier;
    }

    public override string ToString() => $"({X}, {Y})";
}