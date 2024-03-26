using AdventOfCode2023_1.Models.Day05;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
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

    protected override Task PartOne()
    {
        var result = GetLowestLocationNumber();
        SharedMethods.PrintAnswer(result);
        EmptyLists();
        return Task.CompletedTask;
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

    protected override async Task PartTwo()
    {
        var result = await GetLowestLocationNumberPart2();
        SharedMethods.PrintAnswer(result);
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

        var isFirstLine = true;
        foreach (var line in Input)
        {
            if (isFirstLine)
            {
                var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
                _seedsToTest.AddRange(seedsLineAsLong);
                isFirstLine = false;
            }

            currentList = GetMappingList(line) ?? currentList;

            if (currentList != null)
                AddSeedMapping(currentList, line);
        }
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

    private async Task<long> GetLowestLocationNumberPart2()
    {
        ProcessFilesPart2();

        var pairCounter = 0;
        var totalTasks = _seedsToTestPart2.Count;
        var completedTasks = 0;

        var progress = new Progress<long>(current =>
        {
            SharedMethods.ClearCurrentConsoleLine();
            Console.Write($"Finished {current} parts of {totalTasks}");
        });

        var tasks = _seedsToTestPart2.Select(pair => pair.TestPair(_seedToSoil,_soilToFert, _fertToWater, _waterToLight, _lightToTemp, _tempToHumid, _humidToLoc));
        var results = Constants.IsDebug
            ? await Task.WhenAll(tasks.Select(async task =>
            {
                var result = await task.ConfigureAwait(false);
                Interlocked.Increment(ref completedTasks);
                ((IProgress<long>) progress).Report(completedTasks);
                return result;
            })).ConfigureAwait(false)
            : await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Min();
    }

    private void ProcessFilesPart2()
    {
        List<SeedMapping>? currentList = null;

        var isFirstLine = true;
        foreach (var line in Input)
        {
            if (!isFirstLine)
            {
                currentList = GetMappingList(line) ?? currentList;

                if (currentList != null)
                    AddSeedMapping(currentList, line);
                continue;
            }

            var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
            var seedsToTestPart2 = StartEndPair.GetPairs(seedsLineAsLong);

            _seedsToTestPart2.AddRange(seedsToTestPart2);
            isFirstLine = false;
        }
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
        foreach (var seedMapping in mappings
                                        .Where(seedMapping => seedMapping.SourceStart <= seed)
                                        .Where(seedMapping => seedMapping.SourceEnd >= seed))
        {
            return seedMapping.MapValue(seed);
        }

        return seed;
    }
}