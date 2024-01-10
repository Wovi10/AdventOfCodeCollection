using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("05", true);

    public override void PartOne()
    {
        var result = GetLowestLocationNumber();
        SharedMethods.AnswerPart(1, result);
    }

    private static int GetLowestLocationNumber()
    {
        var seeds = GetSeeds();

        return seeds.Min();
    }

    private static List<int> GetSeeds()
    {
        var seeds = Input
            .First() // First line
            .Split(Constants.Colon)
            .Last() // Everything after colon
            .Trim()
            .Split(Constants.Space)
            .ToList();

        return seeds.Where(seed => int.TryParse(seed, out _)).Select(int.Parse).ToList();
    }

    public override void PartTwo()
    {
        throw new NotImplementedException();
    }
}