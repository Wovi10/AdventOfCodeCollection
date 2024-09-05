using UtilsCSharp.Utils;

namespace AdventOfCode2023_1.Models.Day12;

public static class SpringField
{
    public static Dictionary<string, long> Cache { get; set; } = new();


    public static long Solve(List<string> input)
    {
        var part2 = 0L;
        foreach (var line in input.Select(l => l.Split(Constants.Space)))
        {
            var springs = line[0];
            var groups = line[1].Split(Constants.Comma).Select(int.Parse).ToList();

            springs = string.Join(Constants.QuestionMark, Enumerable.Repeat(springs, 5));
            groups = Enumerable.Repeat(groups, 5).SelectMany(g => g).ToList();

            part2 += Calculate(springs, groups);
        }

        return part2;
    }

    private static long Calculate(string springs, List<int> groups)
    {
        var key = $"{springs},{string.Join(Constants.Comma, groups)}";

        if (Cache.TryGetValue(key, out var value))
            return value;

        value = GetCount(springs, groups);
        Cache[key] = value;

        return value;
    }

    private static long GetCount(string springs, List<int> groups)
    {
        while (true)
        {
            var groupsIsEmpty = groups.Count == 0;

            if (groupsIsEmpty)
                return springs.Contains(char.Parse(Constants.HashTag)) ? 0 : 1;

            var noSprings = string.IsNullOrWhiteSpace(springs);
            if (noSprings)
                return 0;

            if (springs.StartsWith(Constants.Dot))
            {
                springs = StripOperationalSprings();
                continue;
            }

            if (springs.StartsWith(Constants.QuestionMark))
                return CalculateOptionWithDot() + CalculateOptionWithHashTag();

            if (!springs.StartsWith(Constants.HashTag))
                throw new Exception("Invalid input");

            if (springs.Length < groups[0])
                return 0;

            var hasRoomForDamagedSprings = !springs[..groups[0]].Contains(char.Parse(Constants.Dot));
            if (!hasRoomForDamagedSprings)
                return 0;

            if (groups.Count > 1)
            {
                if (springs.Length < groups[0] + 1 || springs[groups[0]] == char.Parse(Constants.HashTag))
                    return 0;

                springs = springs[(groups[0] + 1)..];
                groups = groups[1..];
                continue;
            }

            springs = springs[groups[0]..];
            groups = groups[1..];
        }

        long CalculateOptionWithHashTag() 
            => Calculate(Constants.HashTag + springs[1..], groups);

        long CalculateOptionWithDot() 
            => Calculate(Constants.Dot + springs[1..], groups);

        string StripOperationalSprings() 
            => springs.Trim(char.Parse(Constants.Dot));
    }
}