using System.Drawing;

namespace AdventOfCode2023_1.Models.Day18;

public class DigInstruction
{
    public DigInstruction(string inputLine)
    {
        Direction = inputLine[0].CharToDirection();
        Distance = int.Parse(inputLine.Substring(2, inputLine.IndexOf(' ', 2) - 2));
        var colorHex = inputLine.Substring(inputLine.IndexOf('#') + 1, inputLine.IndexOf(')') - inputLine.IndexOf('#') - 1);
        Color = ColorTranslator.FromHtml($"#{colorHex}");
    }

    public (int, int) Direction { get; set; }
    public int Distance { get; set; }
    public Color Color { get; set; }
}