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
        throw new NotImplementedException();
    }

    private static int CalculateFurthestDistanceFromStart()
    {
        var maze = new Maze(Input);
        var loopLength = maze.GetLoopLength();
        return (loopLength - 1) / 2;
    }
}