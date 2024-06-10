using AdventOfCode2023_1.Models.Day05;
using AdventOfCode2023_1.Shared;
using UtilsCSharp;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private readonly HashSet<long> _seedsToTest = new();
    private static readonly HashSet<StartEndPair> SeedsToTestPart2 = new();

    private readonly HashSet<SeedMapping> _soils = new();
    private readonly HashSet<SeedMapping> _fertilizers = new();
    private readonly HashSet<SeedMapping> _waters = new();
    private readonly HashSet<SeedMapping> _lights = new();
    private readonly HashSet<SeedMapping> _temperatures = new();
    private readonly HashSet<SeedMapping> _humidities = new();
    private readonly HashSet<SeedMapping> _locations = new();

    protected override Task<object> PartOne()
    {
        EmptyLists();
        var result = SearchLowestLocation() ?? 0;

        return Task.FromResult<object>(result);
    }

    protected override async Task<object> PartTwo()
    {
        EmptyLists();
        var result = await GetLowestLocationNumber();

        return Task.FromResult<object>(result);
    }

    private void EmptyLists()
    {
        _seedsToTest.Clear();
        _soils.Clear();
        _fertilizers.Clear();
        _waters.Clear();
        _lights.Clear();
        _temperatures.Clear();
        _humidities.Clear();
        _locations.Clear();
    }

    private async Task<long> GetLowestLocationNumber()
    {
        ProcessFile();
        IEnumerable<Task<long>> tasks;
        long[] results;

        if (!Constants.IsDebug)
        {
            tasks = SeedsToTestPart2.Select(pair =>
                pair.TestPair(_soils, _fertilizers, _waters, _lights, _temperatures, _humidities, _locations));
            results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.Min();
        }

        var totalTasks = SeedsToTestPart2.Count;
        var completedTasks = 0;

        var progress = new Progress<long>(current =>
        {
            SharedMethods.ClearCurrentConsoleLine();
            Console.Write($"Finished {current} parts of {totalTasks}");
        });

        tasks = SeedsToTestPart2.Select(pair => pair.TestPair(_soils, _fertilizers, _waters,
            _lights, _temperatures, _humidities, _locations));
        results = await Task.WhenAll(tasks.Select(async task =>
        {
            var result = await task.ConfigureAwait(false);
            Interlocked.Increment(ref completedTasks);
            ((IProgress<long>) progress).Report(completedTasks);
            return result;
        })).ConfigureAwait(false);

        return results.Min();
    }

    private long? SearchLowestLocation()
    {
        ProcessFile();

        return _seedsToTest
            .Select(seed => seed.SeedToLocation(_soils, _fertilizers, _waters, _lights, _temperatures,
                _humidities, _locations))
            .Aggregate(long.MaxValue, (current, result) => MathUtils.GetLowest(result, current));
    }

    private void ProcessFile()
    {
        HashSet<SeedMapping>? currentList = null;

        var isFirstLine = true;
        foreach (var line in Input.Select(l => l.Trim()))
        {
            if (isFirstLine)
            {
                var seedsLineAsLong = line[7..].Split(Constants.Space).Select(long.Parse).ToList();
                if (Variables.RunningPartOne)
                    _seedsToTest.UnionWith(seedsLineAsLong);
                else
                {
                    var startEndPairs = StartEndPair.GetPairs(seedsLineAsLong);
                    SeedsToTestPart2.UnionWith(startEndPairs);
                }

                isFirstLine = false;
            }

            currentList = GetMappingList(line) ?? currentList;

            if (currentList != null)
                AddSeedMapping(currentList, line);
        }
    }

    private HashSet<SeedMapping>? GetMappingList(string line)
    {
        return line switch
        {
            "seed-to-soil map:" => _soils,
            "soil-to-fertilizer map:" => _fertilizers,
            "fertilizer-to-water map:" => _waters,
            "water-to-light map:" => _lights,
            "light-to-temperature map:" => _temperatures,
            "temperature-to-humidity map:" => _humidities,
            "humidity-to-location map:" => _locations,
            _ => null
        };
    }

    private static void AddSeedMapping(HashSet<SeedMapping> currentList, string line)
    {
        var parts = line.Split(Constants.Space).Where(x => long.TryParse(x, out _)).Select(long.Parse).ToArray();
        if (parts.Length == 3)
            currentList.Add(new SeedMapping(parts[1], parts[0], parts[2]));
    }
}