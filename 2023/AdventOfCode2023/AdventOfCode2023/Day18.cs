﻿using AdventOfCode2023_1.Models.Day18;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day18:DayBase
{
    protected override async Task PartOne()
    {
        var result = await CalculateHoleSize();
        SharedMethods.PrintAnswer(result);
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private static async Task<long> CalculateHoleSize()
    {
        var excavationSite = new ExcavationSite(Input);
        await excavationSite.ExecuteDigPlan();
        
        return excavationSite.GetHoleSize();
    }
}