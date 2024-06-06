

using UtilsCSharp.Utils;

namespace AdventOfCode2023_1.Models.Day11;

public class Universe
{
    private int _inputLength;
    private int _lineLength;
    private readonly List<Galaxy<int>> _galaxies;

    public Universe(List<string> input)
    {
        _galaxies = GetGalaxies(input);
    }

    private List<Galaxy<int>> GetGalaxies(List<string> input)
    {
        var galaxies = new List<Galaxy<int>>();
        var lineCounter = 0;
        _inputLength = input.Count;
        foreach (var line in input)
        {
            var characterCounter = 0;
            _lineLength = _lineLength == 0 ? line.Length : _lineLength;
            foreach (var character in line)
            {
                if (character == Constants.HashTag.First())
                    galaxies.Add(new Galaxy<int>(characterCounter, lineCounter));
                characterCounter++;
            }

            lineCounter++;
        }

        return galaxies;
    }

    public List<GalaxyPair<int>> GetGalaxyPairs()
    {
        var galaxyPairs = new List<GalaxyPair<int>>();
        for (var i = 0; i < _galaxies.Count; i++)
        {
            for (var j = i + 1; j < _galaxies.Count; j++)
            {
                galaxyPairs.Add(new GalaxyPair<int>(_galaxies[i], _galaxies[j]));
            }
        }

        return galaxyPairs;
    }

    public void Enlarge(int enlargementFactor = 1)
    {
        if (enlargementFactor != 1)
            enlargementFactor--;

        var emptyColumns = GetEmptyColumns();
        var emptyRows = GetEmptyRows();

        foreach (var galaxy in _galaxies)
        {
            galaxy.XAfterEnlargement +=
                (emptyColumns.Count(emptyColumn => galaxy.X > emptyColumn) * enlargementFactor);
            galaxy.YAfterEnlargement += emptyRows.Count(emptyRow => galaxy.Y > emptyRow) * enlargementFactor;

            galaxy.X = galaxy.XAfterEnlargement;
            galaxy.Y = galaxy.YAfterEnlargement;
        }
    }

    private List<int> GetEmptyRows()
    {
        var emptyRows = new List<int>();

        for (var i = 0; i < _inputLength; i++)
        {
            if (_galaxies.Any(galaxy => galaxy.Y == i))
                continue;
            emptyRows.Add(i);
        }

        return emptyRows;
    }

    private List<int> GetEmptyColumns()
    {
        var emptyColumns = new List<int>();
        for (var i = 0; i < _lineLength - 1; i++)
        {
            if (_galaxies.Any(galaxy => galaxy.X == i))
                continue;
            emptyColumns.Add(i);
        }

        return emptyColumns;
    }
}