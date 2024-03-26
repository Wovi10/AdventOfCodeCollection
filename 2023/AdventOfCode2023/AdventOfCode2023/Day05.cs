using AdventOfCode2023_1.Models.Day05;
using AdventOfCode2023_1.Shared;
using UtilsCSharp;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private readonly List<long> _seedsToTest = new();
    private static readonly List<StartEndPair> SeedsToTestPart2 = new();

    private readonly List<SeedMapping> _seedToSoil = new();
    private readonly List<SeedMapping> _soilToFert = new();
    private readonly List<SeedMapping> _fertToWater = new();
    private readonly List<SeedMapping> _waterToLight = new();
    private readonly List<SeedMapping> _lightToTemp = new();
    private readonly List<SeedMapping> _tempToHumid = new();
    private readonly List<SeedMapping> _humidToLoc = new();

    protected override async Task PartOne()
    {
        await RunPart();
    }

    protected override async Task PartTwo()
    {
        await RunPart();
    }

    private async Task RunPart()
    {
        EmptyLists();
        var result = await GetLowestLocationNumber();
        SharedMethods.PrintAnswer(result);
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

    private async Task<long> GetLowestLocationNumber()
    {
        ProcessFile();

        if (Variables.RunningPartOne)
            return SearchLowestLocation() ?? 0;

        var totalTasks = SeedsToTestPart2.Count;
        var completedTasks = 0;

        var progress = new Progress<long>(current =>
        {
            SharedMethods.ClearCurrentConsoleLine();
            Console.Write($"Finished {current} parts of {totalTasks}");
        });

        var tasks = SeedsToTestPart2.Select(pair => pair.TestPair(_seedToSoil, _soilToFert, _fertToWater,
            _waterToLight, _lightToTemp, _tempToHumid, _humidToLoc));
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

    private long? SearchLowestLocation()
    {
        return _seedsToTest
            .Select(seed => seed.SeedToLocation(_seedToSoil, _soilToFert, _fertToWater, _waterToLight, _lightToTemp,
                _tempToHumid, _humidToLoc))
            .Aggregate(long.MaxValue, (current, result) => MathUtils.GetLowest(result, current));
    }

    private void ProcessFile()
    {
        List<SeedMapping>? currentList = null;

        var isFirstLine = true;
        foreach (var line in Input.Select(l => l.Trim()))
        {
            if (isFirstLine)
            {
                var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
                if (Variables.RunningPartOne)
                    _seedsToTest.AddRange(seedsLineAsLong);
                else
                {
                    var seedsToTestPart2 = StartEndPair.GetPairs(seedsLineAsLong);
                    SeedsToTestPart2.AddRange(seedsToTestPart2);
                }

                isFirstLine = false;
            }

            currentList = GetMappingList(line) ?? currentList;

            if (currentList != null)
                AddSeedMapping(currentList, line);
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
}