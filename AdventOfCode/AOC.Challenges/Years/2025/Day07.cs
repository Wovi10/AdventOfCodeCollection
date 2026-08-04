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

        var laserIndeces = new HashSet<int> { input.First().IndexOf('S') };
        var totalLaserIndeces = 0L;
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

                nextIndeces.Add(laserIndex - 1);
                nextIndeces.Add(laserIndex + 1);
            }
            totalLaserIndeces += nextIndeces.Count;
            laserIndeces = nextIndeces;
        }

        return totalLaserIndeces;
    }
}
