using AdventOfCode2023_1.Models.Day12;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day12 : DayBase
{
    protected override async Task PartOne()
    {
        var result = await GetSumDifferentArrangementCount().ConfigureAwait(false);
        SharedMethods.AnswerPart(result);
    }

    protected override async Task PartTwo()
    {
        var result = await GetSumDifferentArrangementCount().ConfigureAwait(false);
        SharedMethods.AnswerPart(result);
    }

    private static async Task<long> GetSumDifferentArrangementCount()
    {
        var springRows = GetSpringRows();
        var totalTasks = springRows.Count;
        var completedTasks = 0;
        var progress = new Progress<long>(current =>
        {
            SharedMethods.ClearCurrentConsoleLine();
            Console.Write($"Finished {current} parts of {totalTasks}");
        });

        var tasks = springRows.Select(springRow => springRow.GetPossibleArrangementsAsync());
        var results = Constants.IsDebug
            ? await Task.WhenAll(tasks.Select(async task =>
            {
                var result = await task.ConfigureAwait(false);
                Interlocked.Increment(ref completedTasks);
                ((IProgress<long>) progress).Report(completedTasks);
                return result;
            })).ConfigureAwait(false)
            : await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Sum();
    }

    private static List<SpringRow> GetSpringRows()
        => Input.Select((line, index) => new SpringRow(line, index+1)).ToList();
}