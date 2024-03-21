namespace AdventOfCode2023_1.Models.Day13;

public class Line
{
    public Line(string lineString)
    {
        foreach (var character in lineString.Trim())
        {
            Rocks.Add(character == '#');
        }
    }

    public readonly List<bool> Rocks = new();
    public int LinesBeforeMirror { get; set; }
    private List<int> MirroredPositions = new();
    private bool _wasTested = false;

    public async Task<List<int>> GetMirroredPositions()
    {
        if (_wasTested)
            return MirroredPositions;

        _wasTested = true;
        MirroredPositions = await Rocks.GetMirroredPositions();
        return MirroredPositions;
    }
}