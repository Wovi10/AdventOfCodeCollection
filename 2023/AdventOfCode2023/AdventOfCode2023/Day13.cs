﻿using AdventOfCode2023_1.Models.Day13;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day13 : DayBase
{
    protected override async Task PartOne()
    {
        var result = await GetPatternNotesSum().ConfigureAwait(false);
        SharedMethods.PrintAnswer(result);
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private async Task<long> GetPatternNotesSum()
    {
        var patterns = GetPatterns();
        var totalTasks = patterns.Count;
        var completedTasks = 0;
        var progress = new Progress<long>(current =>
        {
            SharedMethods.ClearCurrentConsoleLine();
            Console.Write($"Finished {current} parts of {totalTasks}");
        });
        
        var tasks = patterns.Select(pattern => pattern.GetPatternNotesAsync());
        if (Constants.IsDebug)
        {
            await Task.WhenAll(tasks.Select(async task =>
            {
                await task.ConfigureAwait(false);
                Interlocked.Increment(ref completedTasks);
                ((IProgress<long>) progress).Report(completedTasks);
            })).ConfigureAwait(false);
        }
        else
            await Task.WhenAll(tasks).ConfigureAwait(false);

        return patterns.GetPatternNotesSum();
    }

    private static List<Pattern> GetPatterns()
    {
        List<string> lines = new();
        List<Pattern> patterns = new();

        foreach (var line in Input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                patterns.Add(new Pattern(lines));
                lines.Clear();
                continue;
            }

            lines.Add(line);
        }

        patterns.Add(new Pattern(lines));

        return patterns;
    }
}