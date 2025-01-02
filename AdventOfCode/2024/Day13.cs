using _2024.Models.Day13;
using AOC.Utils;

namespace _2024;

public class Day13(): DayBase("13", "Claw Contraption")
{
    protected override Task<object> PartOne()
    {
        var result = GetFewestTokensToWin();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetFewestTokensToWin();

        return Task.FromResult<object>(result);
    }

    private long GetFewestTokensToWin()
        =>
            GetInput()
                .CreateArcadeMachines()
                .Where(x => x.IsPossible)
                .ToList()
                .Select(x => x.Solution)
                .ToList()
                .Sum();
}