using AdventOfCode2023_1.Models.Day19.Comparer.Implementations;

namespace AdventOfCode2023_1.Models.Day19.Comparer;

public class ComparisonStrategyFactory
{
    public static IComparisonStrategy GetStrategy(char comparer)
    {
        return comparer switch
        {
            '=' => new EqualsStrategy(),
            '<' => new LessThanStrategy(),
            '>' => new GreaterThanStrategy(),
            _ => throw new NotImplementedException()
        };
    } 
}