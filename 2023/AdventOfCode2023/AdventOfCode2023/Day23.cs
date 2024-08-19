﻿using AdventOfCode2023_1.Models.Day23;

namespace AdventOfCode2023_1;

public class Day23() : DayBase("23", "A long walk")
{
    protected override Task<object> PartOne()
    {
        var result = GetLongestHike();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private int GetLongestHike()
    {
        return 
            Input
                .CreateSnowIsland()
                .FindLongestHike();
    }
}