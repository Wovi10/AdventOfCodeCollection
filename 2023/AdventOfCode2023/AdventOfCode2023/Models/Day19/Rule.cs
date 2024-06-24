
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day19;

public class Rule
{
    public Rule(string rulesLine)
    {
        if (Variables.RunningPartOne)
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
                CompareValue = -1;
                CompareValue = '=';
                NextState = rulesLine.Split('}').First();
            }

            return;
        }

        if (rulesLine.Contains(':'))
        {
            Type = rulesLine[0].ToType();
            Comparer = rulesLine[1];
            CompareValue = int.Parse(rulesLine[2..rulesLine.IndexOf(':')]);
            NextState = rulesLine[(rulesLine.IndexOf(':') + 1)..];
        }
        else
        {
            Type = Type.Unknown;
            CompareValue = -1;
            Comparer = '=';
            NextState = rulesLine;
        }
    }

    public Type Type { get; }
    public char Comparer { get; }
    public int CompareValue { get; }
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