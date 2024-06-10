namespace AdventOfCode2023_1.Models.Day05;

public class StartEndPair
{
    private StartEndPair(long start, long range)
    {
        _start = start;
        _end = start + range;
    }

    private readonly long _start;
    private readonly long _end;
    private long _lowestLocation = long.MaxValue;

    public static List<StartEndPair> GetPairs(List<long> seedsToTest)
    {
        var result = new List<StartEndPair>();

        for (var i = 0; i < seedsToTest.Count; i += 2)
        {
            var start = seedsToTest[i];
            var range = seedsToTest[i + 1];
            result.Add(new StartEndPair(start, range));
        }

        return GetRidOfOverlaps(result);
    }

    private static List<StartEndPair> GetRidOfOverlaps(List<StartEndPair> startEndPairs)
    {
        if (startEndPairs.Count == 0)
            return startEndPairs;

        var result = new List<StartEndPair>();
        startEndPairs.Sort((a, b) => a._start.CompareTo(b._start));

        var currentPair = startEndPairs[0];
        for (var i = 1; i < startEndPairs.Count; i++)
        {
            var nextPair = startEndPairs[i];
            if (!(currentPair._end > nextPair._start))
            {
                result.Add(currentPair);
                currentPair = nextPair;

                if (i == startEndPairs.Count - 1)
                    result.Add(nextPair);

                continue;
            }

            if (currentPair._end < nextPair._end)
                currentPair = new StartEndPair(currentPair._start, nextPair._end);

            result.Add(currentPair);
            currentPair = nextPair;
        }

        return result;
    }

    public async Task<long> TestPair(HashSet<SeedMapping> soils, HashSet<SeedMapping> fertilizers,
        HashSet<SeedMapping> waters, HashSet<SeedMapping> lights, HashSet<SeedMapping> temperatures,
        HashSet<SeedMapping> humidities, HashSet<SeedMapping> locations)
    {
        var tasks = new List<Task>();
        for (var seed = _start; seed < _end; seed++)
        {
            var currentSeed = seed;
            var task = Task.Run(() =>
            {
                var location = currentSeed.SeedToLocation(soils, fertilizers, waters, lights, temperatures, humidities,
                    locations);
                if (location < _lowestLocation)
                    Interlocked.Exchange(ref _lowestLocation, location);
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        return _lowestLocation;
    }
}