using AdventOfCode2023_1.Models.Day15;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day15 : DayBase
{
    protected override async Task PartOne()
    {
        var result = await GetSumHashes();
        SharedMethods.PrintAnswer(result);
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private async Task<long> GetSumHashes()
    {
        var steps = GetStepsFromInput();

        var tasks = steps.Select(StepDeHashExtensions.DeHash).ToList();
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Sum();
    }

    private static List<string> GetStepsFromInput()
    {
        var input = Input[0].Split(Constants.Comma);
        return input.ToList();
    }
}