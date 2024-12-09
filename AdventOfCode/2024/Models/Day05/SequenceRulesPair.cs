using UtilsCSharp.Utils;

namespace _2024.Models.Day05;

public class SequenceRulesPair
{
    public SequenceRulesPair(string sequence, IEnumerable<PageNumberRule> rules)
    {
        Pages = sequence.Split(Constants.Comma).ToArray();
        RuleResults = rules.Where(ContainsRulePages).Select(rule => (rule, rule.IsCorrect(Pages))).ToArray();
    }

    private string[] Pages { get; }
    public bool IsCorrect => RuleResults.All(result => result.isCorrect);
    private (PageNumberRule rule, bool isCorrect)[] RuleResults { get; set; }

    public string GetMiddlePage()
        => Pages[Pages.Length / 2];

    private bool ContainsRulePages(PageNumberRule rule)
        => Pages.Contains(rule.FirstPage) && Pages.Contains(rule.LastPage);

    public void CorrectSequence()
    {
        while (!IsCorrect)
        {
            var incorrectRules = RuleResults.Where(rule => !rule.isCorrect).ToArray();
            foreach (var (rule, _) in incorrectRules)
            {
                var firstPageIndex = Array.IndexOf(Pages, rule.FirstPage);
                var lastPageIndex = Array.IndexOf(Pages, rule.LastPage);

                ShiftToCorrectPosition(firstPageIndex, lastPageIndex);
            }

            RuleResults = RuleResults.Select(result => (result.rule, result.rule.IsCorrect(Pages))).ToArray();
        }
    }

    private void ShiftToCorrectPosition(int firstPageIndex, int lastPageIndex)
    {
        for (var i = lastPageIndex; i < firstPageIndex; i++)
            (Pages[i], Pages[i + 1]) = (Pages[i + 1], Pages[i]);
    }
}