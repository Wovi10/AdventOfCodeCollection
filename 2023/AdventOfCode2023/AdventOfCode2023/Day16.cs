using AdventOfCode2023_1.Models.Day16;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day16 : DayBase
{
    protected override Task PartOne()
    {
        var result = CountEnergisedTiles();
        SharedMethods.PrintAnswer(result);
        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        throw new NotImplementedException();
    }

    private static int CountEnergisedTiles()
    {
        var grid = new Grid(Input);
        grid.ChangeDirection();

        return grid.Rows.SelectMany(row => row).Count(tile => tile.IsEnergised);
    }
}