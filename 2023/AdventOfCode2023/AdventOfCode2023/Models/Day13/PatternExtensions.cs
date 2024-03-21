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
}