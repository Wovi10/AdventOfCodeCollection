﻿namespace AdventOfCode2023_1.Models.Day19.Comparer.Implementations;

public class EqualsStrategy: IComparisonStrategy
{
    public long Apply(int[] minXmas, int[] maxXmas, Rule rule, Func<string, long> doResult) 
        => doResult(rule.NextState);
}