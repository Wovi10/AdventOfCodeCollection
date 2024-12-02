using _2023.Models.Day12;
using AOC.Utils;

namespace _2023;

public class Day12() : DayBase("12", "Hot Springs")
{
    protected override async Task<object> PartOne()
    {
        var result = await GetSumDifferentArrangementCount();

        return result;
    }

    protected override Task<object> PartTwo()
    {
        var result = SpringField.Solve(Input);

        return Task.FromResult<object>(result);
    }

    private static async Task<long> GetSumDifferentArrangementCount()
    {
        var springRows = GetSpringRows();
        var results = await RunAsync(springRows);

        return results.Sum();
    }

    private static async Task<long[]> RunAsync(List<SpringRow> springRows)
    {
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
        var results = @await Task.WhenAll(tasks).ConfigureAwait(false);
#endif
        return results;
    }

    private static List<SpringRow> GetSpringRows()
        => Input.Select((line, _) => new SpringRow(line)).ToList();
}