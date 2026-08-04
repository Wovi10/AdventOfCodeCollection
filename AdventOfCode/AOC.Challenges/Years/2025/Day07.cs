using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day07() : DayBase("07", "Laboratories")
{
    protected override Task<object> PartOne()
    {
        var result = CountOfLaserSplits();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = CountTimeLines();

        return Task.FromResult<object>(result);
    }

    private long CountOfLaserSplits()
    {
        var input = GetInput().ToArray();
        var numberOfSplits = 0L;

        var laserIndeces = new HashSet<int> { input.First().IndexOf('S') };
        for (var i = 1; i < input.Length; i++)
        {
            var line = input[i];
            if (!line.Contains('^'))
                continue;

            var nextIndeces = new  HashSet<int>();
            foreach (var laserIndex in laserIndeces)
            {
                if (line[laserIndex] != '^')
                {
                    nextIndeces.Add(laserIndex);
                    continue;
                }

                numberOfSplits++;
                nextIndeces.Add(laserIndex - 1);
                nextIndeces.Add(laserIndex + 1);
            }
            laserIndeces = nextIndeces;
        }
        return numberOfSplits;
    }

    private long CountTimeLines()
    {
        var input = GetInput().ToArray(); // 3094 too low
        var countTimelines = 1L;
        var laserIndeces = new List<int> { input.First().IndexOf('S') };

        for (var i = 1; i < input.Length; i++)
        {
            var line = input[i];
            if (!line.Contains('^'))
                continue;

            var nextIndeces = new  List<int>();
            foreach (var laserIndex in laserIndeces)
            {
                if (line[laserIndex] != '^')
                {
                    nextIndeces.Add(laserIndex);
                    continue;
                }

                countTimelines++;
                nextIndeces.Add(laserIndex - 1);
                nextIndeces.Add(laserIndex + 1);
            }
            laserIndeces = nextIndeces;
            Log.Debug("Line {0}: {1} active timeline(s), running count {2}", i, laserIndeces.Count, countTimelines);
        }

        Log.Info("CountTimeLines finished with {0} timelines", countTimelines);
        return countTimelines;
    }
}
