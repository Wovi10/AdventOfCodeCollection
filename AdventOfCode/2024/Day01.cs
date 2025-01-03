﻿using _2024.Models.Day01;
using AOC.Utils;
using UtilsCSharp;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024;

public class Day01() : DayBase("01", "Historian Hysteria")
{
    protected override Task<object> PartOne()
    {
        var result = GetSumDistances();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetSimilarityScore();

        return Task.FromResult<object>(result);
    }

    private long GetSumDistances()
        => SharedMethods
            .GetInput(Day)
            .GetPairs()
            .GetIds()
            .Sort()
            .GetDistances()
            .Sum();

    private long GetSimilarityScore()
        => SharedMethods
            .GetInput(Day)
            .GetPairs()
            .GetIds()
            .GetAppearanceCounts()
            .GetSimilarities()
            .Sum();
}