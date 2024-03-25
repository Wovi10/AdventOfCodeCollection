using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day13;

public class Pattern
{
    public Pattern(List<string> patternString)
    {
        foreach (var lineString in patternString)
            _lines.Add(new Line(lineString));
    }

    private readonly List<Line> _lines = new();
    private readonly List<List<int>> _mirroredPositions = new();
    private int _linesBeforeMirror;
    private bool _mirrorIsVertical;

    public async Task<ReturnObject?> GetPatternNotesAsync()
    {
        var solutionPartOne = await RunPartOne(_lines);
        if (Variables.RunningPartOne)
            return solutionPartOne;

        return await RunPartTwo(solutionPartOne);
    }

    private async Task<ReturnObject?> RunPartOne(List<Line> lines)
    {
        var columns = AddColumns(lines);
        var horizontalNotes = await GetHorizontalNotes(columns);
        if (horizontalNotes != null)
            return horizontalNotes;

        return await GetVerticalNotes(lines);
    }

    private async Task<ReturnObject?> RunPartTwo(ReturnObject? solutionPartOne)
    {
        ReturnObject? result = null;
        var lineCounter = 0;

        foreach (var line in _lines)
        {
            var columnCounter = 0;
            foreach (var lineRock in line.Rocks)
            {
                _mirroredPositions.Clear();
                var copyOfLines = _lines.Select(l => new Line(l.Rocks)).ToList();
                copyOfLines[lineCounter].Rocks[columnCounter] = !lineRock;
                var columns = AddColumns(copyOfLines);
                result = await GetHorizontalNotes(columns, solutionPartOne?.Notes ?? 0);

                if (result != null && (solutionPartOne?.Notes != result.Notes ||
                                       solutionPartOne.IsVertical != result.IsVertical))
                    return result;

                result = await GetVerticalNotes(copyOfLines, solutionPartOne);

                if (result != null && (solutionPartOne?.Notes != result.Notes ||
                                       solutionPartOne.IsVertical != result.IsVertical))
                    return result;

                columnCounter++;
            }

            lineCounter++;
        }

        return result;
    }

    private static List<List<bool>> AddColumns(List<Line> lines)
    {
        var columns = new List<List<bool>>();
        for (var i = 0; i < lines[0].Rocks.Count; i++)
        {
            columns.Add(new List<bool>());
            foreach (var line in lines)
            {
                columns[i].Add(line.Rocks[i]);
            }
        }

        return columns;
    }

    private async Task<ReturnObject?> GetHorizontalNotes(List<List<bool>> columns, int previousValue = 0)
    {
        foreach (var column in columns)
        {
            _mirroredPositions.Add(await column.GetMirroredPositions());
        }

        var options = _mirroredPositions.GetCommonMirrorPositions();

        if (options.Count == 0)
            return null;

        options = Variables.RunningPartOne
            ? options
            : options.Where(position => position != previousValue).ToList();

        _linesBeforeMirror = options.Count > 0 ? options.Max() : 0;
        _mirrorIsVertical = false;

        return _linesBeforeMirror <= 0
            ? null
            : new ReturnObject
            {
                IsVertical = _mirrorIsVertical,
                Notes = _linesBeforeMirror
            };
    }

    private async Task<ReturnObject?> GetVerticalNotes(List<Line> lines, ReturnObject? previousNotes = null)
    {
        foreach (var line in lines)
        {
            var mirroredPositions = await line.GetMirroredPositions();
            if (mirroredPositions.Count <= 0)
                break;
        }

        var commonMirrorPosition = await lines.GetCommonMirrorPosition(previousNotes);

        if (commonMirrorPosition <= 0)
            return null;

        _mirrorIsVertical = true;
        _linesBeforeMirror = commonMirrorPosition;

        return new ReturnObject
        {
            IsVertical = _mirrorIsVertical,
            Notes = _linesBeforeMirror
        };
    }
}