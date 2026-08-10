using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace AOC.Challenges.Years._2025;

public class Day10() : DayBase("10", "Factory")
{
    protected override Task<object> PartOne()
    {
        var result = FewestPressesToConfigure();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private const char OutcomeStart = '[';
    private const char OutcomeEnd = ']';
    private const char JoltageStart = '{';
    private const char OptionStart = '(';
    private const char OptionEnd = ')';
    private const char Space = ' ';
    private const char ButtonOn = '#';
    private const char Empty = '\0';
    private const char Comma = ',';
    private long FewestPressesToConfigure()
    {
        var input =
            GetInput()
                .Select(l =>
                    (outcome: l.Split(OutcomeEnd).First().Trim()[1..],
                        buttons:l.Split(OutcomeEnd).Last().Split(JoltageStart).First().Trim())
                );

        var result = FewestPresses(input);

        return result.Sum();
    }

    private IEnumerable<long> FewestPresses(IEnumerable<(string outcome, string buttons)> input)
    {
        foreach (var (outcome, buttons) in input)
        {
            var desired = outcome.Select(c => c == ButtonOn).ToArray();
            var possibilities =
                buttons.Split(Space).Select(o => o[1..^1].Split(Comma)).ToArray();

            yield return IterateOverAll(desired, possibilities);
        }
    }

    private long IterateOverAll(bool[] desired, string[][] possibilities)
    {
        var comboLength = 1;
        while (true)
        {
            var combos = FindCombinations(possibilities.Length, comboLength);

            if (ContainsSolution(desired, combos, possibilities))
                return comboLength;

            comboLength++;
        }
    }

    private bool ContainsSolution(bool[] desired, IEnumerable<List<int>> combos, string[][] possibilities)
    {
        var current = desired.Select(_ => false).ToArray();

        foreach (var combo in combos)
        {
            var toUse = possibilities.Where((_, i) => combo.Contains(i)).First().ToArray();
            current = UseButtons(current, toUse);

            if (current == desired)
                return true;
        }

        return false;
    }

    private static IEnumerable<List<int>> FindCombinations(int posLength, int length)
    {
        return Combine(0, new List<int>());

        IEnumerable<List<int>> Combine(int start, List<int> current)
        {
            if (current.Count == length)
            {
                yield return new List<int>(current);
                yield break;
            }

            for (var i = start; i < posLength; i++)
            {
                current.Add(i);
                foreach (var combo in Combine(i+1, current))
                    yield return combo;
                current.RemoveAt(current.Count-1);
            }
        }
    }

    private static bool[] UseButtons(bool[] initial, string[] buttonsToPress)
    {
        foreach (var button in buttonsToPress.Select(int.Parse))
            initial[button] = !initial[button];

        return initial;
    }
}
