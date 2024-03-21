using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day13;

public static class PatternExtensions
{
    public static long GetPatternNotesSum(this List<Pattern> patterns)
    {
        var counter = 0;
        var sumVerticalNotes = 0;
        var sumHorizontalNotes = 0;

        foreach (var pattern in patterns)
        {
            if (pattern.MirrorIsVertical)
                sumVerticalNotes += pattern.LinesBeforeMirror;
            else
                sumHorizontalNotes += pattern.LinesBeforeMirror;
        }

        return sumVerticalNotes + (100 * sumHorizontalNotes);
    }

    public static bool? IsBeforeMiddle(this int position, int half)
    {
        // NULL means it's in the middle
        // TRUE means it's before the middle
        // FALSE means it's after the middle
        return MathUtils.IsLessThan(position, half);
    }

    public static async Task<int> GetCommonMirrorPosition(this List<Line> lines)
    {
        var mirroredPositions = new List<List<int>>();

        foreach (var line in lines) 
            mirroredPositions.Add(await line.GetMirroredPositions());

        return mirroredPositions.GetCommonMirrorPosition();
    }

    public static int GetCommonMirrorPosition(this List<List<int>> lines)
    {
        if (lines.Count == 0 || lines.Any(line => line.Count == 0))
            return 0;

        var commonPositions = lines[0];
        commonPositions = lines.Skip(1)
            .Aggregate(commonPositions, (current, line) => current.Intersect(line).ToList());

        return commonPositions.Count > 0 ? commonPositions.Max() : 0;
    }

    public static async Task<List<int>> GetMirroredPositions(this List<bool> line)
    {
        List<int> mirroredPositions = new();
        for (var i = 1; i < line.Count-1; i++)
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

        var isBeforeMiddle = position.IsBeforeMiddle(rocks.Count / 2);

        var rangeToCheck = isBeforeMiddle switch
        {
            true => rocks[..(position * 2)],
            false => rocks[(position - placesFromEnd)..],
            null => rocks
        };

        for (var i = 0; i < rangeToCheck.Count / 2; i++)
        {
            if (rangeToCheck[i] != rangeToCheck[^(i + 1)])
                return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}