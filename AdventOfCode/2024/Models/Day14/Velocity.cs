using UtilsCSharp.Utils;

namespace _2024.Models.Day14;

public class Velocity
{
    public Velocity(string input)
    {
        var parts = input.Split(Constants.Comma);
        Horizontal = int.Parse(parts.First());
        Vertical = int.Parse(parts.Last());
    }

    public int Horizontal { get; set; }
    public int Vertical { get; set; }

    public override string ToString()
    {
        return $"({Horizontal},{Vertical})";
    }
}