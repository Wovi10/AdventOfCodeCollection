namespace _2024.Models.Day04;

public static class Day04Extensions
{
    private const string XmasString = "XMAS";
    private const string BackWardsXmas = "SAMX";

    public static long SearchXmas(this List<string> input)
    {
        var height = input.Count;
        var width = input.First().Length;

        var xmasCount = 0;

        for (var y = 0; y < width; y++)
        {
            var currentLine = input[y];
            var nextFourLines = input.Skip(y).Take(XmasString.Length).ToList();
            var previousFourLines = input.Skip(Math.Max(y - (XmasString.Length - 1),0)).Take(Math.Min(y+1, XmasString.Length)).ToList();

            for (var x = 0; x < height; x++)
            {
                if (x.CanGoDown() && HasBackwardsXmas(currentLine, x))
                    xmasCount++;

                if (x.CanGoUp(width) && HasForwardXmas(currentLine, x))
                    xmasCount++;

                if (y.CanGoUp(height) && HasDownwardsXmas(nextFourLines, x))
                    xmasCount++;

                if (y.CanGoDown() && HasUpwardsXmas(previousFourLines, x))
                    xmasCount++;

                if (x.CanGoDown() && y.CanGoDown() &&
                    HasLeftUpDiagonalXmas(previousFourLines, x))
                    xmasCount++;

                if (x.CanGoDown() && y.CanGoUp(height) &&
                    HasLeftDownDiagonalXmas(nextFourLines, x))
                    xmasCount++;

                if (x.CanGoUp(width) && y.CanGoDown() &&
                    HasRightUpDiagonalXmas(previousFourLines, x))
                    xmasCount++;

                if (x.CanGoUp(width) && y.CanGoUp(height) &&
                    HasRightDownDiagonalXmas(nextFourLines, x))
                    xmasCount++;
            }
        }

        return xmasCount;
    }

    private static bool HasBackwardsXmas(string currentLine, int x)
        => $"{currentLine[x]}{currentLine[x - 1]}{currentLine[x - 2]}{currentLine[x - 3]}"
            .SequenceEqual(XmasString);

    private static bool HasForwardXmas(string currentLine, int x)
        => $"{currentLine[x]}{currentLine[x + 1]}{currentLine[x + 2]}{currentLine[x + 3]}"
            .SequenceEqual(XmasString);

    private static bool HasDownwardsXmas(List<string> nextFourLines, int x)
        => $"{nextFourLines[0][x]}{nextFourLines[1][x]}{nextFourLines[2][x]}{nextFourLines[3][x]}"
            .SequenceEqual(XmasString);

    private static bool HasUpwardsXmas(List<string> previousFourLines, int x)
        => $"{previousFourLines[3][x]}{previousFourLines[2][x]}{previousFourLines[1][x]}{previousFourLines[0][x]}"
            .SequenceEqual(XmasString);

    private static bool HasLeftUpDiagonalXmas(List<string> previousFourLines, int x)
        => $"{previousFourLines[3][x]}{previousFourLines[2][x-1]}{previousFourLines[1][x-2]}{previousFourLines[0][x-3]}"
            .SequenceEqual(XmasString);

    private static bool HasLeftDownDiagonalXmas(List<string> nextFourLines, int x)
        => $"{nextFourLines[0][x]}{nextFourLines[1][x-1]}{nextFourLines[2][x-2]}{nextFourLines[3][x-3]}"
            .SequenceEqual(XmasString);

    private static bool HasRightUpDiagonalXmas(List<string> previousFourLines, int x)
        => $"{previousFourLines[3][x]}{previousFourLines[2][x+1]}{previousFourLines[1][x+2]}{previousFourLines[0][x+3]}"
            .SequenceEqual(XmasString);

    private static bool HasRightDownDiagonalXmas(List<string> nextFourLines, int x)
        => $"{nextFourLines[0][x]}{nextFourLines[1][x+1]}{nextFourLines[2][x+2]}{nextFourLines[3][x+3]}"
            .SequenceEqual(XmasString);

    private static bool CanGoUp(this int amount, int max)
        => amount + XmasString.Length-1 <= max - 1;

    private static bool CanGoDown(this int amount)
        => amount >= XmasString.Length - 1;
}