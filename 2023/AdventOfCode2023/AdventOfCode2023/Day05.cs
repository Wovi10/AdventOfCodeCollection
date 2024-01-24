using AdventOfCode2023_1.Models.Day05;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("05", IsMock);

    private readonly List<long> _seedsToTest = new();
    private static List<StartEndPair> _seedsToTestPart2 = new();

    private readonly List<SeedMapping> _seedToSoil = new();
    private readonly List<SeedMapping> _soilToFert = new();
    private readonly List<SeedMapping> _fertToWater = new();
    private readonly List<SeedMapping> _waterToLight = new();
    private readonly List<SeedMapping> _lightToTemp = new();
    private readonly List<SeedMapping> _tempToHumid = new();
    private readonly List<SeedMapping> _humidToLoc = new();
    
    private long _lowestLocation = long.MaxValue;

    protected override void PartOne()
    {
        
        var result = GetLowestLocationNumber();
        SharedMethods.AnswerPart(1, result);
        EmptyLists();
    }

    private void EmptyLists()
    {
        _seedsToTest.Clear();
        _seedToSoil.Clear();
        _soilToFert.Clear();
        _fertToWater.Clear();
        _waterToLight.Clear();
        _lightToTemp.Clear();
        _tempToHumid.Clear();
        _humidToLoc.Clear();
    }

    protected override void PartTwo()
    {
        var result = GetLowestLocationNumberPart2();
        SharedMethods.AnswerPart(2, result);
    }

    #region Part1

    private long GetLowestLocationNumber()
    {
        ProcessFile();
        return SearchLowestLocation() ?? 0;
    }

    private void ProcessFile()
    {
        List<SeedMapping>? currentList = null;

        foreach (var line in Input.Where(line => !TryAddSeed(line)))
        {
            currentList = GetMappingList(line) ?? currentList;

            if (currentList != null)
                AddSeedMapping(currentList, line);
        }
    }

    private bool TryAddSeed(string line)
    {
        if (!line.StartsWith("seeds:")) return false;

        var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
        var seedsToTest = seedsLineAsLong;

        _seedsToTest.AddRange(seedsToTest);
        return true;
    }

    private long? SearchLowestLocation()
    {
        var lowest = long.MaxValue;

        foreach (var seed in _seedsToTest)
        {
            var result = SeedToLocation(seed);
            lowest = GetLowest(result, lowest);
        }

        return lowest;
    }

    #endregion

    #region Part2

    private long GetLowestLocationNumberPart2()
    {
        List<SeedMapping>? currentList = null;

        foreach (var line in Input.Where(line => !TryAddSeedPart2(line)))
        {
            currentList = GetMappingList(line) ?? currentList;
            
            if (currentList != null)
                AddSeedMapping(currentList, line);
        }

        var pairCounter = 0;
        foreach (var startEndPair in _seedsToTestPart2)
        {
            Console.WriteLine($"{Constants.LineReturn}Starting pair {++pairCounter} of {_seedsToTestPart2.Count}");
            for (var seed = startEndPair.Start; seed < startEndPair.End; seed++)
            {
                var location = SeedToLocation(seed);
                _lowestLocation = GetLowest(location, _lowestLocation);

                if (Constants.IsDebug)
                {
                    var trying = seed - startEndPair.Start;
                    var progress = (double)trying / startEndPair.Range;
                    var percentage = (long)(progress * 100);
                    SharedMethods.WritePercentage(percentage);
                }
            }

            if (Constants.IsDebug)
                Console.WriteLine($"Lowest after pair {pairCounter} is {_lowestLocation}");
        }

        return _lowestLocation;
    }

    private static bool TryAddSeedPart2(string line)
    {
        if (!line.StartsWith("seeds:")) return false;
        
        var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
        var seedsToTestPart2 = StartEndPair.GetPairs(seedsLineAsLong);

        _seedsToTestPart2.AddRange(seedsToTestPart2);
        return true;
    }

    #endregion

    private static long GetLowest(long toCompare, long lowest)
    {
        return long.Min(toCompare, lowest);
    }

    private long SeedToLocation(long seed)
    {
        var result = TestLocation(seed, _seedToSoil);
        result = TestLocation(result, _soilToFert);
        result = TestLocation(result, _fertToWater);
        result = TestLocation(result, _waterToLight);
        result = TestLocation(result, _lightToTemp);
        result = TestLocation(result, _tempToHumid);
        result = TestLocation(result, _humidToLoc);

        return result;
    }

    private void SeedToSoil(long seed)
    {
        foreach (var seedMapping in _seedToSoil)
        {
            var soil = seed;
            if (seedMapping.IsInRange(seed))
                soil = seedMapping.MapValue(seed) ?? seed;

            SoilToFert(soil);
        }
    }

    private void SoilToFert(long soil)
    {
        foreach (var seedMapping in _soilToFert)
        {
            long fertilizer;
            if (seedMapping.IsInRange(soil))
                fertilizer = seedMapping.MapValue(soil) ?? soil;
            else
                fertilizer = soil;

            FertToWater(fertilizer);
        }
    }

    private void FertToWater(long fertilizer)
    {
        foreach (var seedMapping in _fertToWater)
        {
            long water;
            if (seedMapping.IsInRange(fertilizer))
                water = seedMapping.MapValue(fertilizer) ?? fertilizer;
            else
                water = fertilizer;

            WaterToLight(water);
        }
    }

    private void WaterToLight(long water)
    {
        foreach (var seedMapping in _waterToLight)
        {
            long light;
            if (seedMapping.IsInRange(water))
                light = seedMapping.MapValue(water) ?? water;
            else
                light = water;

            LightToTemp(light);
        }
    }

    private void LightToTemp(long light)
    {
        foreach (var seedMapping in _lightToTemp)
        {
            long temperature;
            if (seedMapping.IsInRange(light))
                temperature = seedMapping.MapValue(light) ?? light;
            else
                temperature = light;

            TempToHumid(temperature);
        }
    }

    private void TempToHumid(long temperature)
    {
        foreach (var seedMapping in _tempToHumid)
        {
            long humidity;
            if (seedMapping.IsInRange(temperature))
                humidity = seedMapping.MapValue(temperature) ?? temperature;
            else
                humidity = temperature;

            HumidToLoc(humidity);
        }
    }

    private void HumidToLoc(long humidity)
    {
        foreach (var seedMapping in _humidToLoc)
        {
            long location;
            if (seedMapping.IsInRange(humidity))
                location = seedMapping.MapValue(humidity) ?? humidity;
            else
                location = humidity;

            _lowestLocation = GetLowest(location, _lowestLocation);
        }
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

    private static long TestLocation(long seed, List<SeedMapping> mappings)
    {
        SeedMapping mapping = null;
        foreach (var seedMapping in mappings)
        {
            if (!seedMapping.IsInRange(seed))
                continue;
            return seedMapping.MapValue(seed) ?? seed;
        }
        return seed;
    }
}