namespace AdventOfCode2023_1.Models.Day05;

public class StartEndPair(long start, long range)
{
    public long Start { get; } = start;
    public long Range { get; } = range;
    public long End => Start + Range;

    public static List<StartEndPair> GetPairs(List<long> seedsToTest)
    {
        var result = new List<StartEndPair>();
        for (var i = 0; i < seedsToTest.Count; i += 2)
        {
            var start = seedsToTest[i];
            var length = seedsToTest[i + 1];
            result.Add(new StartEndPair(start, length));
        }
        
        result = GetRidOfOverlaps(result);

        return result;
    }

    private static List<StartEndPair> GetRidOfOverlaps(List<StartEndPair> startEndPairs)
    {
        // Get rid of all overlaps in the list, making it as small as possible
        var result = new List<StartEndPair>();
        var sortedStartEndPairs = startEndPairs.OrderBy(pair => pair.Start).ToList();
        var currentPair = sortedStartEndPairs[0];
        for (var i = 1; i < sortedStartEndPairs.Count; i++)
        {
            var nextPair = sortedStartEndPairs[i];
            if (!(currentPair.End > nextPair.Start))
            {
                result.Add(currentPair);
                continue;
            }

            if (currentPair.End < nextPair.End)
            {
                var newPair = new StartEndPair(currentPair.Start, nextPair.End);
                currentPair = newPair;
            }
            result.Add(currentPair);
        }

        return result;
    }
}