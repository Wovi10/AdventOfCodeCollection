using AdventOfCode2023_1.Models.Day12;
using AdventOfCode2023_1.Shared;
using NUnit.Framework;

namespace AdventOfCode2023_1;

public class Day12() : DayBase("12", "Hot Springs")
{
    protected override async Task<object> PartOne()
    {
        var result = await GetSumDifferentArrangementCount().ConfigureAwait(false);

        return result;
    }

    protected override async Task<object> PartTwo()
    {
        return Answers.NotYetFound;
        
        var result = await GetSumDifferentArrangementCount().ConfigureAwait(false);

        return result;
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
#if DEBUG
        var results = await Task.WhenAll(tasks.Select(async task =>
        {
            var result = await task.ConfigureAwait(false);
            Interlocked.Increment(ref completedTasks);
            ((IProgress<long>)progress).Report(completedTasks);
            return result;
        })).ConfigureAwait(false);
#else
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
#endif

        return results.Sum();
    }

    private static List<SpringRow> GetSpringRows()
        => Input.Select((line, index) => new SpringRow(line)).ToList();
}