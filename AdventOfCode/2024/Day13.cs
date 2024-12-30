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
        throw new NotImplementedException();
    }

    private long GetFewestTokensToWin()
    {
        var possibleMachines =
            GetInput()
                .CreateArcadeMachines()
                .Where(x => x.CanWin)
                .ToList();

        var thingy = possibleMachines.First().Solution;

        return 0;
    }
}