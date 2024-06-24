
namespace AdventOfCode2023_1.Models.Day19;

public class Rule
{
    public Rule(string rulesLine)
    {
        if (rulesLine.Contains(':'))
        {
            var firstHalf = rulesLine.Split(':').First();
            Type = firstHalf.First().ToType();
            Comparer = firstHalf[1];
            CompareValue = int.Parse(firstHalf[2..]);
            NextState = rulesLine.Split(':').Last().Trim();
        }
        else
        {
            Type = Type.Unknown;
            NextState = rulesLine.Split('}').First();
        }
    }

    public Type Type { get; }
    private char Comparer { get; }
    private int CompareValue { get; }
    public string NextState { get; }
    
    public bool Works(int value)
    {
        return Comparer switch
        {
            '<' => value < CompareValue,
            '>' => value > CompareValue,
            '=' => value == CompareValue,
            _ => false
        };
    }
}