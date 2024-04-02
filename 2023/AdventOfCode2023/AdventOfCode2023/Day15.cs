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
        
        // Create a list of tasks to run in parallel and wait for them all to finish
        var totalTasks = steps.Count;
        var completedTasks = 0;
        var progress = new Progress<long>(current =>
        {
            SharedMethods.ClearCurrentConsoleLine();
            Console.Write($"Finished {current} parts of {totalTasks}");
        });

        var tasks = steps.Select(StepDeHashExtensions.DeHash).ToList();
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

    private static List<string> GetStepsFromInput()
    {
        var input = Input[0].Split(Constants.Comma);
        return input.ToList();
    }
}