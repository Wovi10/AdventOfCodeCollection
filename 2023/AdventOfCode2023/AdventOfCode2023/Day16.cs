using AdventOfCode2023_1.Models.Day16;
using AdventOfCode2023_1.Models.Day16.Enums;
using AdventOfCode2023_1.Shared;
using NUnit.Framework;
using UtilsCSharp.Enums;

namespace AdventOfCode2023_1;

public class Day16() : DayBase("16", "The Floor Will Be Lava")
{
    protected override async Task<object> PartOne()
    {
        var result = await CountEnergisedTiles();

        return result;
    }

    protected override async Task<object> PartTwo()
    {
        var result = await GetMostEfficientEnergisedTiles();

        return result;
    }

    private static async Task<int> GetMostEfficientEnergisedTiles()
    {
        var width = Input[0].Length;
        var height = Input.Count;

        var tasks = new List<Task<int>>();
        for (var i = 0; i < width; i++)
            tasks.Add(CountEnergisedTiles(Direction.Down, i, 0));
        for (var i = 0; i < height; i++)
            tasks.Add(CountEnergisedTiles(Direction.Right, 0, i));

        var result = await Task.WhenAll(tasks);
        return result.Max();
    }

    private static async Task<int> CountEnergisedTiles(Direction direction = Direction.Right, int x = 0, int y = 0)
    {
        var grid = new Grid(Input);
        await grid.ChangeDirection(direction, x, y);
        grid.SetEnergisedTilesCount();
        grid.ResetGrid();

        return grid.EnergisedTilesCount;
    }
}