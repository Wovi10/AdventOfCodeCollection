using UtilsCSharp;

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
    public long LowestLocation = long.MaxValue;

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
                currentPair = new StartEndPair(currentPair.Start, nextPair.End);

            result.Add(currentPair);
            currentPair = nextPair;
        }

        return result;
    }

    public async Task<long> TestPair(List<SeedMapping> seedToSoil, List<SeedMapping> soilToFert,
        List<SeedMapping> fertToWater, List<SeedMapping> waterToLight, List<SeedMapping> lightToTemp,
        List<SeedMapping> tempToHumid, List<SeedMapping> humidToLoc)
    {
        for (var seed = Start; seed < End; seed++)
        {
            var location = await Task.Run(() => SeedToLocation(seed, seedToSoil, soilToFert, fertToWater, waterToLight,
                lightToTemp, tempToHumid, humidToLoc));
            LowestLocation = MathUtils.GetLowest(location, LowestLocation);
        }

        return LowestLocation;
    }

    private static long SeedToLocation(long seed, List<SeedMapping> seedToSoil, List<SeedMapping> soilToFert,
        List<SeedMapping> fertToWater, List<SeedMapping> waterToLight, List<SeedMapping> lightToTemp,
        List<SeedMapping> tempToHumid, List<SeedMapping> humidToLoc)
    {
        var result = TestLocation(seed, seedToSoil);
        result = TestLocation(result, soilToFert);
        result = TestLocation(result, fertToWater);
        result = TestLocation(result, waterToLight);
        result = TestLocation(result, lightToTemp);
        result = TestLocation(result, tempToHumid);
        result = TestLocation(result, humidToLoc);

        return result;
    }

    private static long TestLocation(long seed, List<SeedMapping> mappings)
    {
        foreach (var seedMapping in mappings
                     .Where(seedMapping => seedMapping.SourceStart <= seed && seedMapping.SourceEnd >= seed))
        {
            return seedMapping.MapValue(seed);
        }

        return seed;
    }
}