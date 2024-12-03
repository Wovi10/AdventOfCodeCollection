using UtilsCSharp;

namespace _2024.Models.Day02;

public class Report
{
    private int[] Parts { get; set; }

    public Report(string[] parts)
    {
        Parts = parts.Select(part => part.Trim()).Select(int.Parse).ToArray();
    }

    public bool IsSafe()
    {
        if (!IsAllAscending() && !IsAllDescending())
            return false;

        for (var i = 0; i < Parts.Length; i++)
        {
            if (i + 1 >= Parts.Length)
                return true;

            var absDifference = (Parts[i] - Parts[i + 1]).Abs();

            if (absDifference is < 1 or > 3)
                return false;
        }

        return true;
    }

    private bool IsAllDescending()
        => Parts.OrderDescending().ToArray().SequenceEqual(Parts);

    private bool IsAllAscending()
        => Parts.Order().ToArray().SequenceEqual(Parts);
}