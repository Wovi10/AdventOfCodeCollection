using _2023.Models.Day10;
using AOC.Utils;

namespace _2023;

public class Day10() : DayBase("10", "Pipe Maze")
{
    protected override Task<object> PartOne()
    {
        var result = CalculateFurthestDistanceFromStart();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        Input = SharedMethods.GetInput(Day);
        var result = CalculateEnclosedTiles();

        return Task.FromResult<object>(result);
    }

    private static int CalculateFurthestDistanceFromStart()
    {
        var maze = new Maze(Input);
        var loopLength = maze.GetLoopLength();
        return loopLength / 2;
    }

    private static int CalculateEnclosedTiles()
    {
        var maze = new Maze(Input);
        return maze.CalculateEnclosedTiles();
    }
}