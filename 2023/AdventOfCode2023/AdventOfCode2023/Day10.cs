using AdventOfCode2023_1.Models.Day10;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day10 : DayBase
{
    protected override Task PartOne()
    {
        var result = CalculateFurthestDistanceFromStart();
        SharedMethods.AnswerPart(result);
        return Task.CompletedTask;
    }

    protected override Task PartTwo()
    {
        var result = CalculateEnclosedTiles();
        SharedMethods.AnswerPart(result);
        return Task.CompletedTask;
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