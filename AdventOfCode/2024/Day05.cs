using _2024.Models.Day05;
using AOC.Utils;

namespace _2024;

public class Day05() :DayBase("05", "Print queue")
{
    protected override Task<object> PartOne()
    {
        var result = GetMiddlePagesOfCorrectSequencesSum();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetMiddlePagesOfInCorrectSequencesSum();

        return Task.FromResult<object>(result);
    }

    private int GetMiddlePagesOfCorrectSequencesSum()
        => SharedMethods
            .GetInput(Day)
            .GetSequencePart()
            .Select(sequence
                => new SequenceRulesPair(sequence,
                    SharedMethods
                        .GetInput(Day)[..SharedMethods.GetInput(Day).IndexOf(string.Empty)]
                        .Select(rule => new PageNumberRule(rule))))
            .FilterCorrectSequences(true)
            .Sum(combo => int.Parse(combo.GetMiddlePage()));

    private int GetMiddlePagesOfInCorrectSequencesSum()
        => SharedMethods
            .GetInput(Day)
            .GetSequencePart()
            .Select(sequence
                => new SequenceRulesPair(sequence,
                    SharedMethods
                        .GetInput(Day)[..SharedMethods.GetInput(Day).IndexOf(string.Empty)]
                        .Select(rule => new PageNumberRule(rule))))
            .FilterCorrectSequences(false)
            .ToList()
            .CorrectSequences()
            .Sum(combo => int.Parse(combo.GetMiddlePage()));
}