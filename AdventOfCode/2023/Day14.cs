using System.Diagnostics;
using _2023.Models.Day14;
using AOC.Utils;
using UtilsCSharp;

namespace _2023;

public class Day14() : DayBase("14", "Parabolic Reflector Dish")
{
    protected override Task<object> PartOne()
    {
        var result = CalculateTotalLoad();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = CalculateTotalLoad();

        return Task.FromResult<object>(result);
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
            var foundLoop = false;
            var listOfLoads = new List<long>();
            for (var i = 0L; i < numCycles; i++)
            {
                var totalLoad = dish.Cycle();
                listOfLoads.Add(totalLoad);
#if DEBUG
                SharedMethods.ClearCurrentConsoleLine();
                SharedMethods.PrintProgress(i, numCycles);

                if (i == 1000)
                {
                    stopwatch.Stop();
                    var elapsed = stopwatch.ElapsedMilliseconds;
                    Console.WriteLine();
                    Console.WriteLine($"Finished in {MathUtils.MillSecToSec(elapsed)} seconds");
                    Console.WriteLine($"Expected time: {MathUtils.MillSecToWeek(elapsed * numCycles / i)} weeks");
                    Console.WriteLine();
                }
#endif

                if (foundLoop)
                    continue;

                var loopLength = Algorithms.GetLoopLength(listOfLoads);

                if (loopLength == 0)
                    continue;

                foundLoop = true;

                var numLoopInCycles = numCycles / loopLength;
                var leftOverOfList = listOfLoads.Count - (loopLength * 3);
                var loopLenghtInLeftover = leftOverOfList / loopLength;
                var leftOverAfterLoopLengths = leftOverOfList - (loopLength * loopLenghtInLeftover);

                i = loopLength * numLoopInCycles + leftOverAfterLoopLengths - 1;
            }
        }

        dish.CalculateTotalLoad();

        return dish.TotalLoad;
    }
}