using UtilsCSharp.Utils;

namespace _2024.Models.Day02;

public static class Day02Extensions
{
    public static IEnumerable<Report> ToReports(this List<string> input)
        => input
            .Select(line => line.Split(Constants.Space))
            .Select(parts => new Report(parts));
}