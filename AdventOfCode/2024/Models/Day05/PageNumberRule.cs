using UtilsCSharp.Utils;

namespace _2024.Models.Day05;

public class PageNumberRule
{
    public string FirstPage { get; }
    public string LastPage { get; }

    public PageNumberRule(string rule)
    {
        var parts = rule.Split(Constants.Pipe);
        FirstPage = parts[0].Trim();
        LastPage = parts[1].Trim();
    }

    public bool IsCorrect(string[] sequence)
        => Array.IndexOf(sequence, FirstPage) is not -1 &&
           Array.IndexOf(sequence, LastPage) is not -1 &&
           Array.IndexOf(sequence, FirstPage) < Array.IndexOf(sequence, LastPage);
}