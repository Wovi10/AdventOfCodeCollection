using System.Collections.Concurrent;

namespace _2023.Models.Day05;

public class StartEndPair
{
    private StartEndPair(long start, long range)
    {
        _start = start;
        _end = start + range;
    }

    private readonly long _start;
    private readonly long _end;
    // private long _lowestLocation = long.MaxValue;

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

    public long TestPair(HashSet<SeedMapping> soils, HashSet<SeedMapping> fertilizers,
        HashSet<SeedMapping> waters, HashSet<SeedMapping> lights, HashSet<SeedMapping> temperatures,
        HashSet<SeedMapping> humidities, HashSet<SeedMapping> locations)
    {
        var range = LongRange(_start, _end - _start);
        var partitioner = Partitioner.Create(range);

        var lowestLocation = long.MaxValue;

        Parallel.ForEach(partitioner, (seed, _) =>
        {
            var location = seed.SeedToLocation(soils, fertilizers, waters, lights, temperatures, humidities,
                locations);
            var currentLowest = Interlocked.Read(ref lowestLocation);
            while (location < currentLowest)
            {
                if (Interlocked.CompareExchange(ref lowestLocation, location, currentLowest) == currentLowest)
                    break;

                currentLowest = Interlocked.CompareExchange(ref lowestLocation, location, currentLowest);
            }
        });

        return lowestLocation;
    }

    private static IEnumerable<long> LongRange(long start, long count)
    {
        for (var i = start; i < start + count; i++)
        {
            yield return i;
        }
    }
}