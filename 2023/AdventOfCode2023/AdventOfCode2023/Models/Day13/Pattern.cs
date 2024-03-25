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
    private readonly List<List<bool>> _columns = new();
    private readonly List<List<int>> _mirroredPositions = new();
    public int LinesBeforeMirror = 0;
    public bool MirrorIsVertical = false;

    public async Task<ReturnObject?> GetPatternNotesAsync()
    {
        if (Variables.RunningPartOne)
            return await RunPartOne(_lines);

        return await RunPartTwo();
    }

    private async Task<ReturnObject?> RunPartTwo()
    {
        ReturnObject? result = null;
        var lineCounter = 0;

        foreach (var line in _lines)
        {
            var columnCounter = 0;
            foreach (var lineRock in line.Rocks)
            {
                var copyOfLines = _lines.Select(l => new Line(l.Rocks)).ToList();
                copyOfLines[lineCounter].Rocks[columnCounter] = !lineRock;
                var columns = AddColumns(copyOfLines);
                result = await GetHorizontalNotes(columns);

                if (result != null)
                    return result;

                result = await GetVerticalNotes(copyOfLines);

                if (result != null)
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

    private async Task<ReturnObject?> GetVerticalNotes(List<Line> lines)
    {
        var firstLine = lines.First();
        var firstMirroredPositions = await firstLine.GetMirroredPositions();

        if (firstMirroredPositions.Count <= 0) 
            return null;

        foreach (var line in lines.Skip(1))
        {
            var mirroredPositions = await line.GetMirroredPositions();
            if (mirroredPositions.Count <= 0) 
                break;
        }

        var commonMirrorPosition = await lines.GetCommonMirrorPosition();
        if (commonMirrorPosition <= 0) 
            return null;

        MirrorIsVertical = true;
        LinesBeforeMirror = commonMirrorPosition;

        return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);
    }

    private async Task<ReturnObject?> GetHorizontalNotes(List<List<bool>> columns)
    {
        foreach (var column in columns)
        {
            _mirroredPositions.Add(await column.GetMirroredPositions());
        }

        LinesBeforeMirror = _mirroredPositions.GetCommonMirrorPosition();

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

public class ReturnObject
{
    public ReturnObject(int notes, bool isVertical)
    {
        Notes = notes;
        IsVertical = isVertical;
    }

    public int Notes { get; set; }
    public bool IsVertical { get; set; }
}