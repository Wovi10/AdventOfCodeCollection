using AdventOfCode2023_1.Models.Day16;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day16 : DayBase
{
    protected override async Task PartOne()
    {
        var result = await CountEnergisedTiles();
        SharedMethods.PrintAnswer(result);
    }

    protected override async Task PartTwo()
    {
        var result = await GetMostEfficientEnergisedTiles();
        SharedMethods.PrintAnswer(result);
    }

    private async Task<int> GetMostEfficientEnergisedTiles()
    {
        var width = Input[0].Length;
        var height = Input.Count;
        
        var tasks = new List<Task<int>>();
        for (var i = 0; i < width; i++) 
            tasks.Add(CountEnergisedTiles(Direction.Downwards, i, 0));
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