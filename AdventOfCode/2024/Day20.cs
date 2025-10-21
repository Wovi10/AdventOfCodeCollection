using _2024.Models.Day20;
using AOC.Utils;

namespace _2024;

public class Day20(): DayBase("20", "Race Condition")
{
    protected override Task<object> PartOne()
    {
        var result = GetNumberOfGoodCheats();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetNumberOfGoodCheats()
    {
        return
            GetInput()
                .ToRace()
                .FillShortestPathNoCheating()
                .FindWithCheats();
    }
}