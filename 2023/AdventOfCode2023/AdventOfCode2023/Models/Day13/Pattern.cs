using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day13;

public class Pattern
{
    public Pattern(List<string> patternString, int index)
    {
        Index = index;
        foreach (var lineString in patternString)
        {
            _lines.Add(new Line(lineString));
        }
    }

    private readonly int Index;
    private readonly List<Line> _lines = new();
    private readonly List<List<int>> _mirroredPositions = new();
    public int LinesBeforeMirror = 0;
    public bool MirrorIsVertical = false;

    public async Task<ReturnObject?> GetPatternNotesAsync()
    {
        var solutionPartOne = await RunPartOne(_lines);
        if (Variables.RunningPartOne)
            return solutionPartOne;
        

        return await RunPartTwo(solutionPartOne);
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

                if (result != null && (solutionPartOne?.Notes != result.Notes || solutionPartOne?.IsVertical != result.IsVertical))
                    return result;

                result = await GetVerticalNotes(copyOfLines, solutionPartOne);

                if (result != null && (solutionPartOne?.Notes != result.Notes || solutionPartOne?.IsVertical != result.IsVertical))
                    return result;

                columnCounter++;
            }
            lineCounter++;
        }

        return result;
    }

    private async Task<ReturnObject?> RunPartOne(List<Line> lines)
    {
        var columns = AddColumns(lines);
        var horizontalNotes = await GetHorizontalNotes(columns);
        if (horizontalNotes != null)
            return horizontalNotes;

        var verticalNotes = await GetVerticalNotes(lines);
        return verticalNotes;
    }

    private async Task<ReturnObject?> GetVerticalNotes(List<Line> lines, ReturnObject? previousNotes = null)
    {
        foreach (var line in lines)
        {
            var mirroredPositions = await line.GetMirroredPositions();
            if (mirroredPositions.Count <= 0) 
                break;
        }

        var commonMirrorPositions = await lines.GetCommonMirrorPositions();

        if (commonMirrorPositions.Count == 0)
            return null;

        if (!Variables.RunningPartOne && previousNotes is {IsVertical: true})
        {
            commonMirrorPositions = commonMirrorPositions.Where(position => position != previousNotes.Notes).ToList();
        }

        var commonMirrorPosition = commonMirrorPositions.Count > 0 ? commonMirrorPositions.Max() : 0;

        if (commonMirrorPosition <= 0) 
            return null;

        MirrorIsVertical = true;
        LinesBeforeMirror = commonMirrorPosition;

        return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);
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

        LinesBeforeMirror = options.Count > 0 ? options.Max() : 0;
        MirrorIsVertical = false;

        if (LinesBeforeMirror > 0)
            return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);

        return null;

    }

    private List<List<bool>> AddColumns(List<Line> lines)
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
}

public class ReturnObject(int notes, bool isVertical)
{
    public int Notes { get; set; } = notes;
    public bool IsVertical { get; set; } = isVertical;
}