namespace AdventOfCode2023_1.Models.Day19.Comparer.Implementations;

public class LessThanStrategy: IComparisonStrategy
{
    public long Apply(int[] minXmas, int[] maxXmas, Rule rule, Func<string, long> doResult)
    {
        var returnValue = 0L;
        var typeIndex = (int)rule.Type;
        var tempMax = maxXmas[typeIndex];

        if (minXmas[typeIndex] < rule.CompareValue)
        {
            maxXmas[typeIndex] = int.Min(maxXmas[typeIndex], rule.CompareValue - 1);
            returnValue += doResult(rule.NextState);
        }

        maxXmas[typeIndex] = tempMax;
        minXmas[typeIndex] = int.Max(minXmas[typeIndex], rule.CompareValue);

        return returnValue;
    }
}