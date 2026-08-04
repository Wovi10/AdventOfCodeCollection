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

    private const char Splitter = '^';
    private long CountTimeLines()
    {
        var input = GetInput().Where(l => l.Any(c => c != Splitter)).ToArray(); // 3094 too low
        var countTimelines = 1L;
        var laserIndeces = new Dictionary<int, long> {[input.First().IndexOf('S')] = 1L };

        for (var i = 1; i < input.Length; i++)
        {
            var line = input[i];
            if (!line.Contains(Splitter))
                continue;

            var nextIndeces = new  Dictionary<int, long>();

            foreach (var (laserIndex, count) in laserIndeces)
            {
                if (line[laserIndex] != Splitter)
                {
                    Add(nextIndeces, laserIndex, count);
                    continue;
                }

                countTimelines++;
                Add(nextIndeces, laserIndex - 1, count);
                Add(nextIndeces, laserIndex + 1, count);
            }
            laserIndeces = nextIndeces;
            Log.Debug("Line {0}: {1} active timeline(s), running count {2}", i, laserIndeces.Count, countTimelines);
        }

        Log.Info("CountTimeLines finished with {0} timelines", countTimelines);
        return laserIndeces.Values.Sum();
    }

    private static void Add(Dictionary<int, long> dict, int key, long value)
        => dict[key] = dict.GetValueOrDefault(key) + value;
}
