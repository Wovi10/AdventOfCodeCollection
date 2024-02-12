using AdventOfCode2023_1.Models.Day10;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day10 : DayBase
{
    protected override void PartOne()
    {
        var result = CalculateFurthestDistanceFromStart();
        SharedMethods.AnswerPart(result);
    }

    protected override void PartTwo()
    {
        var result = CalculateEnclosedTiles();
        SharedMethods.AnswerPart(result);
    }

    private static int CalculateFurthestDistanceFromStart()
    {
        var maze = new Maze(Input);
        var loopLength = maze.GetLoopLength();
        return loopLength / 2;
    }

    private int CalculateEnclosedTiles()
    {
        var maze = new Maze(Input);
        return maze.CalculateEnclosedTiles();
    }
}