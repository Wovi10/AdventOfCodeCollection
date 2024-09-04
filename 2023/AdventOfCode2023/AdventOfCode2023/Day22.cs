﻿using AdventOfCode2023_1.Models.Day22;

namespace AdventOfCode2023_1;

public class Day22() : DayBase("22", "Sand slabs")
{
    protected override Task<object> PartOne()
    {
        var result = GetNumberOfDisintegrableBricks();
        
        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = FindOptimalChainReactionCount();
        
        return Task.FromResult<object>(result);
    }

    private int FindOptimalChainReactionCount()
        => Input
            .CreateBrickPile()
            .OrderBricks()
            .MoveBricksDown()
            .FindDisintegrableBricks()
            .CountChainReaction();

    private int GetNumberOfDisintegrableBricks() 
        => Input
            .CreateBrickPile()
            .OrderBricks()
            .MoveBricksDown()
            .FindDisintegrableBricks()
            .CountDisintegrableBricks();
}