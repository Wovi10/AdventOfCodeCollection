namespace _2024.Models.Day13;

public class ArcadePrize
{
    public int X { get; set; }
    public int Y { get; set; }

    public ArcadePrize(string line)
    {
        var usefulData = line.Split(": ").Last();
        var parts = usefulData.Split(", ");
        var xPart = parts.First().Split("=").Last();
        var yPart = parts.Last().Split("=").Last();

        X = int.Parse(xPart);
        Y = int.Parse(yPart);
    }

    public override string ToString() => $"({X}, {Y})";
}