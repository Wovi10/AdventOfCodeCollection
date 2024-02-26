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
        throw new NotImplementedException();
    }

    private static int GetSumDifferentArrangementCount()
    {
        var springRows = GetSpringRows();
        springRows.ForEach(springRow => springRow.SetPossibleArrangements());

        return springRows.Sum(springRow => springRow.PossibleArrangements);
    }

    private static List<SpringRow> GetSpringRows()
    {
        return Input.Select(line => new SpringRow(line)).ToList();
    }
}