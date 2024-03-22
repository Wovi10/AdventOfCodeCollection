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
        var firstLine = _lines.First();
        var firstMirroredPositions = await firstLine.GetMirroredPositions();

        if (firstMirroredPositions.Count > 0)
        {
            foreach (var line in _lines.Skip(1))
            {
                var mirroredPositions = await line.GetMirroredPositions();
                if (mirroredPositions.Count > 0) 
                    continue;
                break;
            }
            var commonMirrorPosition = await _lines.GetCommonMirrorPosition();
            if (commonMirrorPosition > 0)
            {
                MirrorIsVertical = true;
                LinesBeforeMirror = commonMirrorPosition;
                return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);
            }
        }
        AddColumns();
        _lines.Clear();
        foreach (var column in _columns)
        {
            _mirroredPositions.Add(await column.GetMirroredPositions());
        }

        LinesBeforeMirror = _mirroredPositions.GetCommonMirrorPosition();
        
        return new ReturnObject(LinesBeforeMirror, MirrorIsVertical);
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