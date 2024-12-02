namespace _2023.Models.Day19.Comparer;

public interface IComparisonStrategy
{
    long Apply(int[] minXmas, int[] maxXmas, Rule rule, Func<string, long> doResult);
}