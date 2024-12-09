namespace _2024.Models.Day05;

public static class Day05Extensions
{

    public static IEnumerable<string> GetSequencePart(this List<string> source)
        => source[(source.IndexOf(string.Empty) + 1)..];

    public static IEnumerable<SequenceRulesPair> FilterCorrectSequences(this IEnumerable<SequenceRulesPair> source,
        bool? isCorrect = null)
        => source.Where(combo => isCorrect is null || combo.IsCorrect == isCorrect);

    public static IEnumerable<SequenceRulesPair> CorrectSequences(this List<SequenceRulesPair> source)
    {
        source.ForEach(pair => pair.CorrectSequence());

        return source;
    }
}