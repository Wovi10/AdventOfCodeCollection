using AdventOfCode2023_1.Shared;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day13;

public static class PatternExtensions
{
    public static long GetPatternNotesSum(this ReturnObject?[] patterns)
    {
        var sumVerticalNotes = patterns.Sum(pattern => pattern?.IsVertical ?? false ? pattern.Notes : 0);
        var sumHorizontalNotes = patterns.Sum(pattern => pattern?.IsVertical ?? true ? 0 : pattern.Notes);

        return sumVerticalNotes + (100 * sumHorizontalNotes);
    }

    private static bool IsBeforeMiddle(this int position, int placesFromEnd)
        => position.IsLessThan(placesFromEnd);

    public static async Task<int> GetCommonMirrorPosition(this List<Line> lines, ReturnObject? previousNotes = null)
    {
        var commonMirrorPositions = await lines.GetCommonMirrorPositions();

        if (commonMirrorPositions.Count == 0)
            return 0;

        if (!Variables.RunningPartOne && previousNotes is {IsVertical: true})
            commonMirrorPositions = commonMirrorPositions.Where(position => position != previousNotes.Notes).ToList();

        var commonMirrorPosition = 
                commonMirrorPositions.Count > 0 
                    ? commonMirrorPositions.Max() 
                    : 0;

        return 
            commonMirrorPosition <= 0 
                ? 0 
                : commonMirrorPosition;
    }

    private static async Task<List<int>> GetCommonMirrorPositions(this List<Line> lines)
    {
        var mirroredPositions = new List<List<int>>();

        foreach (var line in lines)
            mirroredPositions.Add(await line.GetMirroredPositions());

        return mirroredPositions.GetCommonMirrorPositions();
    }

    public static List<int> GetCommonMirrorPositions(this List<List<int>> lines)
    {
        if (lines.Count == 0 || lines.Any(line => line.Count == 0))
            return new List<int>();

        var commonPositions = lines[0];
        commonPositions = 
            lines
                .Skip(1)
                .Aggregate(commonPositions, (current, line) => current.Intersect(line).ToList());

        return commonPositions;
    }

    public static async Task<List<int>> GetMirroredPositions(this List<bool> line)
    {
        List<int> mirroredPositions = new();
        for (var i = 1; i < line.Count; i++)
        {
            if (await CanBeMirrored(i, line))
                mirroredPositions.Add(i);
        }

        mirroredPositions.Sort();
        return mirroredPositions;
    }

    private static Task<bool> CanBeMirrored(int position, List<bool> rocks)
    {
        var placesFromEnd = rocks.Count - position;

        var isBeforeMiddle = position.IsBeforeMiddle(placesFromEnd);

        var rangeToCheck = isBeforeMiddle switch
        {
            true when placesFromEnd == position => rocks,
            true => rocks[..(position * 2)],
            false => rocks[(position - placesFromEnd)..]
        };

        for (var i = 0; i < rangeToCheck.Count / 2; i++)
        {
            if (rangeToCheck[i] != rangeToCheck[^(i + 1)])
                return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}