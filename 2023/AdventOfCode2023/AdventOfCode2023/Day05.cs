﻿using AdventOfCode2023_1.Models.Day05;
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

    protected override void PartOne()
    {
        
        var result = GetLowestLocationNumber();
        SharedMethods.AnswerPart(1, result);
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
        long? lowest = null;

        foreach (var seed in _seedsToTest)
        {
            lowest = GetLowest(seed, lowest);
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

        long? lowest = null;
        var pairCounter = 0;
        foreach (var startEndPair in _seedsToTestPart2)
        {
            Console.WriteLine($"{Constants.LineReturn}Starting {++pairCounter} of {_seedsToTestPart2.Count}");
            var seedCounter = 0L;
            for (var seed = startEndPair.Start; seed < startEndPair.End; seed++)
            {
                lowest = GetLowest(seed, lowest);

                if (Constants.IsDebug)
                {
                    var percentage = seedCounter / startEndPair.Range * 100;
                    SharedMethods.WritePercentage(percentage);
                }

                seedCounter++;
            }

            Console.WriteLine($"{Constants.LineReturn}Lowest for pair {pairCounter} is {lowest}");
        }

        return lowest!.Value;
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

    private long? GetLowest(long seed, long? lowest)
    {
        var result = TestLocation(seed, _seedToSoil);
        result = TestLocation(result, _soilToFert);
        result = TestLocation(result, _fertToWater);
        result = TestLocation(result, _waterToLight);
        result = TestLocation(result, _lightToTemp);
        result = TestLocation(result, _tempToHumid);
        result = TestLocation(result, _humidToLoc);
        if (lowest == null || result < lowest)
            lowest = result;
        return lowest;
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
        var mapping = mappings.FirstOrDefault(map => map.IsInRange(seed));
        return mapping?.MapValue(seed) ?? seed;
    }
}