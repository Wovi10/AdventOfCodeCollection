﻿using AdventOfCode2023_1.Models.Day09;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day09 : DayBase
{
    protected override void PartOne()
    {
        var result = PredictValue();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        var result = PredictValue();
        SharedMethods.AnswerPart(2, result);
    }
    
    private static long PredictValue()
    {
        var historySelection = GetHistorySelection();
        var addedValues = new List<long>();
        foreach (var history in historySelection)
        {
            history.CalculateNextSequence();
            history.Extrapolate();
            addedValues.Add(history.AddedValue);
        }

        return addedValues.Sum();
    }

    private static List<History> GetHistorySelection()
    {
        return Input
            .Select(line => line
                                    .Split(Constants.Space)
                                    .Where(x => long.TryParse(x, out _))
                                    .Select(long.Parse)
                                    .ToList()
            )
            .Select(historyAsList => new History(historyAsList))
            .ToList();
    }
}