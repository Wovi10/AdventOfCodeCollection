using UtilsCSharp.Utils;

namespace _2024.Models.Day05;

public class SequenceRulesPair
{
    public SequenceRulesPair(string sequence, List<PageNumberRule> rules)
    {
        Pages = sequence.Split(Constants.Comma).ToArray();
        IsCorrect = rules.Where(ContainsRulePages).All(rule => rule.IsCorrect(Pages));
    }

    private string[] Pages { get; }
    public bool IsCorrect { get; }

    public string GetMiddlePage()
        => Pages[Pages.Length / 2];

    private bool ContainsRulePages(PageNumberRule rule)
        => Pages.Contains(rule.FirstPage) && Pages.Contains(rule.LastPage);
}