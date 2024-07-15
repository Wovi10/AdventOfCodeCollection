namespace AdventOfCode2023_1.Models.Day19.Comparer.Implementations;

public class GreaterThanStrategy: IComparisonStrategy
{
    public long Apply(int[] minXmas, int[] maxXmas, Rule rule, Func<string, long> doResult)
    {
        var returnValue = 0L;
        var typeIndex = (int)rule.Type;
        var tempMin = minXmas[typeIndex];

        if (maxXmas[typeIndex] > rule.CompareValue)
        {
            minXmas[typeIndex] = int.Max(minXmas[typeIndex], rule.CompareValue + 1);
            returnValue += doResult(rule.NextState);
        }

        minXmas[typeIndex] = tempMin;
        maxXmas[typeIndex] = int.Min(maxXmas[typeIndex], rule.CompareValue);

        return returnValue;
    }
}