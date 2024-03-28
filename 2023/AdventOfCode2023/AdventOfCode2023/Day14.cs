using System.Diagnostics;
using AdventOfCode2023_1.Models.Day14;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day14:DayBase
{
    protected override Task PartOne()
    {
        var result = CalculateTotalLoad();
        SharedMethods.PrintAnswer(result);
        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        var result = CalculateTotalLoad();
        SharedMethods.PrintAnswer(result);
        return Task.CompletedTask;
    }

    private static long CalculateTotalLoad()
    {
        var dish = new Dish(Input);

        if (Variables.RunningPartOne)
            dish.TiltNorth();
        else
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            const int numCycles = 1000000000;
            for (var i = 0; i < numCycles; i++)
            {
                dish.Cycle();
                if (!Constants.IsDebug) 
                    continue;
                SharedMethods.ClearCurrentConsoleLine();
                SharedMethods.PrintProgress(i, numCycles);

                if (i == 1000)
                {
                    stopwatch.Stop();
                    Console.WriteLine();
                    Console.WriteLine($"Finished in {stopwatch.ElapsedMilliseconds / 1000} seconds");
                    Console.WriteLine($"Expected time: {stopwatch.ElapsedMilliseconds / 1000 * numCycles / 1000 / 60 / 60 / 24} days");
                    Console.WriteLine();
                }
            }
        }

        dish.CalculateTotalLoad();

        return dish.TotalLoad;
    }
}