using AdventOfCode2023_1.Models.Day12;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day12 : DayBase
{
    protected override void PartOne()
    {
        var result = GetSumDifferentArrangementCount();
        SharedMethods.AnswerPart(result);
    }

    protected override void PartTwo()
    {
        var result = GetSumDifferentArrangementCount();
        SharedMethods.AnswerPart(result);
    }

    private static long GetSumDifferentArrangementCount()
    {
        var springRows = GetSpringRows();

        return springRows.Select(springRow =>
        {
            Console.Write($"{Constants.LineReturn}Running row {springRows.IndexOf(springRow)+1} of {springRows.Count}");
            return springRow.GetPossibleArrangements();
        }).Sum();
    }

    private static List<SpringRow> GetSpringRows()
        => Input.Select(line => new SpringRow(line)).ToList();
}