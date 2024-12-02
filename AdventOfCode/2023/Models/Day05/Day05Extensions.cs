namespace _2023.Models.Day05;

public static class Day05Extensions
{
    public static long SeedToLocation(this long seed, HashSet<SeedMapping> soils, HashSet<SeedMapping> fertilizers,
        HashSet<SeedMapping> waters, HashSet<SeedMapping> lights, HashSet<SeedMapping> temperatures,
        HashSet<SeedMapping> humidities, HashSet<SeedMapping> locations)
    {
        return 
                seed.MapTo(soils)
                    .MapTo(fertilizers)
                    .MapTo(waters)
                    .MapTo(lights)
                    .MapTo(temperatures)
                    .MapTo(humidities)
                    .MapTo(locations);
    }

    private static long MapTo(this long seed, HashSet<SeedMapping> mappings)
    {
        foreach (var seedMapping in mappings
                     .Where(seedMapping => seedMapping.SourceStart <= seed && seedMapping.SourceEnd >= seed))
        {
            return seedMapping.MapValue(seed);
        }

        return seed;
    }
}