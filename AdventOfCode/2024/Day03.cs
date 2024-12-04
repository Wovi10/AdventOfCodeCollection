using _2024.Models.Day03;
using AOC.Utils;

namespace _2024;

public class Day03() : DayBase("03", "Mull It Over")
{
    protected override Task<object> PartOne()
    {
        var result = GetMultiplicationsSum();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetMultiplicationsSum()
        => SharedMethods
            .GetInput(Day)
            .ToSingleString()
            .FindAllMultiplicationStrings()
            // .FilterIncorrectMultiplications()
            .ToNumberPairs()
            .MultiplyAll()
            .Sum();
}