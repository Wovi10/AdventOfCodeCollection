using AdventOfCode2023_1.Models.Day05;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("05", IsMock);

    private readonly List<long> _seedsToTest = new();

    private readonly List<SeedMapping> _seedToSoil = new();
    private readonly List<SeedMapping> _soilToFert = new();
    private readonly List<SeedMapping> _fertToWater = new();
    private readonly List<SeedMapping> _waterToLight = new();
    private readonly List<SeedMapping> _lightToTemp = new();
    private readonly List<SeedMapping> _tempToHumid = new();
    private readonly List<SeedMapping> _humidToLoc = new();

    protected override void PartOne()
    {
        
        var result = GetLowestLocationNumber();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        var result = GetLowestLocationNumber(true);
        SharedMethods.AnswerPart(2, result);
    }

    #region Part1

    private long GetLowestLocationNumber(bool part2 = false)
    {
        ProcessFile(part2);
        return SearchLowestLocation() ?? 0;
    }

    private void ProcessFile(bool part2)
    {
        List<SeedMapping>? currentList = null;

        foreach (var line in Input.Where(line => !TryAddSeed(line, part2)))
        {
            currentList = GetMappingList(line) ?? currentList;

            if (currentList != null)
                AddSeedMapping(currentList, line);
        }
    }

    private bool TryAddSeed(string line, bool part2)
    {
        if (!line.StartsWith("seeds:")) return false;
        
        var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
        List<long> seedsToTest;
        if (part2)
        {
            seedsToTest = new List<long>();
            var startEndPairs = StartEndPair.GetPairs(seedsLineAsLong);
        }
        else
            seedsToTest = seedsLineAsLong;

        _seedsToTest.AddRange(seedsToTest);
        return true;

    }

    private List<SeedMapping>? GetMappingList(string line)
    {
        return line switch
        {
            "seed-to-soil map:" => _seedToSoil,
            "soil-to-fertilizer map:" => _soilToFert,
            "fertilizer-to-water map:" => _fertToWater,
            "water-to-light map:" => _waterToLight,
            "light-to-temperature map:" => _lightToTemp,
            "temperature-to-humidity map:" => _tempToHumid,
            "humidity-to-location map:" => _humidToLoc,
            _ => null
        };
    }

    private static void AddSeedMapping(List<SeedMapping> currentList, string line)
    {
        var parts = line.Split(Constants.Space).Where(x => long.TryParse(x, out _)).Select(long.Parse).ToArray();
        if (parts.Length == 3) 
            currentList.Add(new SeedMapping(parts[1], parts[0], parts[2]));
    }

    private long? SearchLowestLocation()
    {
        long? lowest = null;

        foreach (var seed in _seedsToTest)
        {
            //AnsiConsole.WriteLine("Testing seed " + seed + " for lowest location");
            var result = TestLocation(seed, _seedToSoil);
            result = TestLocation(result, _soilToFert);
            result = TestLocation(result, _fertToWater);
            result = TestLocation(result, _waterToLight);
            result = TestLocation(result, _lightToTemp);
            result = TestLocation(result, _tempToHumid);
            result = TestLocation(result, _humidToLoc);
            if (lowest == null || result < lowest)
                lowest = result;
        }

        return lowest;
    }

    private static long TestLocation(long seed, List<SeedMapping> mappings)
    {
        var mapping = mappings.FirstOrDefault(x => x.IsInRange(seed));
        return mapping?.MapValue(seed) ?? seed;
    }

    #endregion
}