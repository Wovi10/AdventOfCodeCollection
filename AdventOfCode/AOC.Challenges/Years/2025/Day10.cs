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
        var result = FewestPressesForJoltage();

        return Task.FromResult<object>(result);
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
        foreach (var combo in combos)
        {
            var allToUse = possibilities.Where((_, i) => combo.Contains(i)).ToArray();

            var current = desired.Select(_ => false).ToArray();
            foreach (var toUse in allToUse)
                current = UseButtons(current, toUse);

            if (current.SequenceEqual(desired))
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

    private long FewestPressesForJoltage()
    {
        var input =
            GetInput()
                .Select(l =>
                    {
                        var useful = l.Split(OutcomeEnd).Last().Trim().Split(JoltageStart);
                        return (
                            buttons:
                            useful.First().Trim()
                                .Split(Space).Select(o => o[1..^1].Split(Comma).Select(int.Parse).ToArray()).ToArray(),
                            joltages:
                            useful.Last()[..^1]
                                .Split(Comma).Select(int.Parse).ToArray()
                        );
                    }
                );

        return FewestPressesJoltage(input).Sum();
    }

    private static IEnumerable<long> FewestPressesJoltage(IEnumerable<(int[][] buttons, int[] joltages)> input)
    {
        foreach (var (buttons, joltages) in input)
            yield return IterateOverAllJoltages(joltages, buttons);
    }

    private static long IterateOverAllJoltages(int[] desired, int[][] possibilities)
    {
        var comboLength = desired.Max();
        var maxComboLength = desired.Sum();
        while (comboLength <= maxComboLength)
        {
            foreach (var combo in FindCombinationsJoltages(possibilities.Length, comboLength))
            {
                var dict = new Dictionary<int, int>();
                var distinctIndexesCombo = combo.Distinct().ToArray();

                foreach (var i in distinctIndexesCombo)
                {
                    var numInCombo = combo.Count(c => c == i);
                    foreach (var j in possibilities[i])
                        if (!dict.TryAdd(j, numInCombo))
                            dict[j] += numInCombo;
                }

                if (dict.Count == desired.Length && ItWorks(dict, desired))
                    return comboLength;
            }

            comboLength++;
        }

        return 0;
    }

    private static bool ItWorks(Dictionary<int, int> allToUse, int[] desired)
        => allToUse.OrderBy(k => k.Key).Select(k => k.Value).ToArray().SequenceEqual(desired);

    private static IEnumerable<List<long>> FindCombinationsJoltages(int posLength, int length)
    {
        return Combine(0, new List<long>());

        IEnumerable<List<long>> Combine(int start, List<long> current)
        {
            if (current.Count == length)
            {
                yield return new List<long>(current);
                yield break;
            }

            for (var i = start; i < posLength; i++)
            {
                current.Add(i);
                foreach (var combo in Combine(i, current))
                    yield return combo;
                current.RemoveAt(current.Count-1);
            }
        }
    }

}
