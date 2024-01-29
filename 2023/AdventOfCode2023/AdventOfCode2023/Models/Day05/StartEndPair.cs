namespace AdventOfCode2023_1.Models.Day05;

public class StartEndPair
{
    private StartEndPair(long start, long range)
    {
        Start = start;
        Range = range;
        End = start + range;
    }

    public readonly long Start;
    public readonly long Range;
    public readonly long End;

    public static List<StartEndPair> GetPairs(List<long> seedsToTest)
    {
        var result = new List<StartEndPair>();
        for (var i = 0; i < seedsToTest.Count; i += 2)
        {
            var start = seedsToTest[i];
            var range = seedsToTest[i + 1];
            result.Add(new StartEndPair(start, range));
        }

        result = GetRidOfOverlaps(result);

        return result;
    }

    private static List<StartEndPair> GetRidOfOverlaps(List<StartEndPair> startEndPairs)
    {
        if (startEndPairs.Count == 0) 
            return startEndPairs;

        var result = new List<StartEndPair>();
        var sortedStartEndPairs = startEndPairs.OrderBy(pair => pair.Start).ToList();
        var currentPair = sortedStartEndPairs[0];
        for (var i = 1; i < sortedStartEndPairs.Count; i++)
        {
            var nextPair = sortedStartEndPairs[i];
            if (!(currentPair.End > nextPair.Start))
            {
                result.Add(currentPair);
                currentPair = nextPair;
                if (i == sortedStartEndPairs.Count - 1)
                    result.Add(nextPair);
                continue;
            }

            if (currentPair.End < nextPair.End)
            {
                var newPair = new StartEndPair(currentPair.Start, nextPair.End);
                currentPair = newPair;
            }
            result.Add(currentPair);
            currentPair = nextPair;
        }

        return result;
    }
}