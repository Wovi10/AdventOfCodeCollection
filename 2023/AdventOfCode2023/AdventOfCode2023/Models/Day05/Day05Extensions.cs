namespace AdventOfCode2023_1.Models.Day05;

public static class Day05Extensions
{
    public static long SeedToLocation(this long seed, List<SeedMapping> seedToSoil, List<SeedMapping> soilToFert,
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