namespace _2024.Models.Day13;

public class ArcadeButton
{
    public int X { get; set; }
    public int Y { get; set; }

    public ArcadeButton(string line)
    {
        var useful = line.Split(": ").Last().Split(", ");
        var xPart = useful.First().Split("+").Last().Trim();
        var yPart = useful.Last().Split("+").Last().Trim();

        X = int.Parse(xPart);
        Y = int.Parse(yPart);
    }

    public override string ToString() => $"({X}, {Y})";
}