using AdventOfCode2023_1.Shared;

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

    public Line(List<bool> rocks)
    {
        foreach (var rock in rocks) 
            Rocks.Add(rock);
    }

    public readonly List<bool> Rocks = new();
    public int LinesBeforeMirror { get; set; }
    private List<int> MirroredPositions = new();
    private bool _wasTested = false;

    public async Task<List<int>> GetMirroredPositions()
    {
        if (Variables.RunningPartOne && _wasTested)
            return MirroredPositions;

        _wasTested = true;
        MirroredPositions = await Rocks.GetMirroredPositions();
        return MirroredPositions;
    }
}