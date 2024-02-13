using AdventOfCode2023_1.Models.Day11;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day11 : DayBase
{
    private readonly List<Galaxy> _galaxies = new();
    private readonly List<GalaxyPair> _galaxyPairs = new();
    private int _lineLength = 0;
    private int _inputLength = 0;

    protected override void PartOne()
    {
        var result = GetSumOfShortestPaths();
        SharedMethods.AnswerPart(result);
    }

    protected override void PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetSumOfShortestPaths()
    {
        GetGalaxies();
        GetGalaxyPairs();
        var emptyRows = GetEmptyRows();
        var emptyColumns = GetEmptyColumns();

        return 0;
    }

    private void GetGalaxies()
    {
        var lineCounter = 0;
        _inputLength = Input.Count;
        foreach (var line in Input)
        {
            var characterCounter = 0;
            _lineLength = _lineLength == 0 ? line.Length : _lineLength;
            foreach (var character in line)
            {
                if (character == Constants.HashTag[0]) 
                    _galaxies.Add(new Galaxy(characterCounter, lineCounter));
                characterCounter++;
            }
            lineCounter++;
        }
    }

    private void GetGalaxyPairs()
    {
        for (var i = 0; i < _galaxies.Count; i++)
        {
            for (var j = i + 1; j < _galaxies.Count; j++)
            {
                _galaxyPairs.Add(new GalaxyPair(_galaxies[i], _galaxies[j]));
            }
        }
    }

    private List<int> GetEmptyRows()
    {
        var emptyRows = new List<int>();

        for (var i = 0; i < _inputLength; i++)
        {
            
        }

        return emptyRows;
    }

    private List<int> GetEmptyColumns()
    {
        var emptyColumns = new List<int>();
        for (var i = 0; i < _lineLength-1; i++)
        {
            var isEmpty = true;
            foreach (var line in Input)
            {
                isEmpty = line[i] != Constants.HashTag[0];
                if (!isEmpty)
                    break;
            }
            if (isEmpty)
                emptyColumns.Add(i);
        }

        return emptyColumns;
    }
}