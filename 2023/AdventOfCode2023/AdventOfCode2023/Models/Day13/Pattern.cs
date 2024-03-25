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

    public async Task<ReturnObject> GetPatternNotesAsync()
    {
        if (Variables.RunningPartOne)
            return await RunPartOne();
        
        return await RunPartTwo();
    }

    private async Task<ReturnObject> RunPartTwo()
    {
        foreach (var line in _lines)
        {
            var counter = 0;
            foreach (var lineRock in line.Rocks)
            {
                line.Rocks[counter] = !lineRock;
                var verticalNotes = await GetVerticalNotes();
                if (verticalNotes != null)
                    return verticalNotes;
                line.Rocks[counter] = lineRock;
                counter++;
            }
        }

        AddColumns();
        _lines.Clear();
        
        foreach (var column in _columns)
        {
            var counter = 0;
            foreach (var columnRock in column)
            {
                column[counter] = !columnRock;
                var horizontalNotes = await GetHorizontalNotes();
                if (horizontalNotes != null)
                    return horizontalNotes;
                column[counter] = columnRock;
                counter++;
            }
        }

        return new ReturnObject(0, false);
    }

    private async Task<ReturnObject> RunPartOne()
    {
        var verticalNotes = await GetVerticalNotes();
        if (verticalNotes != null)
            return verticalNotes;

        AddColumns();
        _lines.Clear();
        return await GetHorizontalNotes();
    }

    private async Task<ReturnObject?> GetVerticalNotes()
    {
        var firstLine = _lines.First();
        var firstMirroredPositions = await firstLine.GetMirroredPositions();

        if (firstMirroredPositions.Count <= 0) 
            return null;

        foreach (var line in _lines.Skip(1))
        {
            var mirroredPositions = await line.GetMirroredPositions();
            if (mirroredPositions.Count <= 0) 
                break;
        }

        var commonMirrorPosition = await _lines.GetCommonMirrorPosition();
        if (commonMirrorPosition <= 0) 
            return null;

        MirrorIsVertical = true;
        LinesBeforeMirror = commonMirrorPosition;

        return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);
    }

    private async Task<ReturnObject?> GetHorizontalNotes()
    {
        foreach (var column in _columns)
        {
            _mirroredPositions.Add(await column.GetMirroredPositions());
        }

        LinesBeforeMirror = _mirroredPositions.GetCommonMirrorPosition();

        if (LinesBeforeMirror > 0)
            return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);

        return null;

    }

    private void AddColumns()
    {
        for (var i = 0; i < _lines[0].Rocks.Count; i++)
        {
            _columns.Add(new List<bool>());
            foreach (var line in _lines)
            {
                _columns[i].Add(line.Rocks[i]);
            }
        }
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