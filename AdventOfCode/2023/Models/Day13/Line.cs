using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2023.Models.Day13;

public class Line
{
    public Line(string lineString)
    {
        foreach (var character in lineString.Trim())
        {
            Rocks.Add(character.ToString() == Constants.HashTag);
        }
    }

    public Line(List<bool> rocks)
    {
        foreach (var rock in rocks)
            Rocks.Add(rock);
    }

    public readonly List<bool> Rocks = new();
    private List<int> _mirroredPositions = new();
    private bool _wasTested;

    public async Task<List<int>> GetMirroredPositions()
    {
        if (Variables.RunningPartOne && _wasTested)
            return _mirroredPositions;

        _wasTested = true;
        _mirroredPositions = await Rocks.GetMirroredPositions();
        return _mirroredPositions;
    }
}