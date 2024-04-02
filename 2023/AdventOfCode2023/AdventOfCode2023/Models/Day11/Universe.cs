using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day11;

public class Universe
{
    private int _inputLength;
    private int _lineLength;
    private readonly List<Galaxy> _galaxies;

    public Universe(List<string> input)
    {
        _galaxies = GetGalaxies(input);
    }

    private List<Galaxy> GetGalaxies(List<string> input)
    {
        var galaxies = new List<Galaxy>();
        var lineCounter = 0;
        _inputLength = input.Count;
        foreach (var line in input)
        {
            var characterCounter = 0;
            _lineLength = _lineLength == 0 ? line.Length : _lineLength;
            foreach (var character in line)
            {
                if (character == Constants.HashTag.First())
                    galaxies.Add(new Galaxy(characterCounter, lineCounter));
                characterCounter++;
            }

            lineCounter++;
        }

        return galaxies;
    }

    public List<GalaxyPair> GetGalaxyPairs()
    {
        var galaxyPairs = new List<GalaxyPair>();
        for (var i = 0; i < _galaxies.Count; i++)
        {
            for (var j = i + 1; j < _galaxies.Count; j++)
            {
                galaxyPairs.Add(new GalaxyPair(_galaxies[i], _galaxies[j]));
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
                (emptyColumns.Count(emptyColumn => galaxy.XCoordinate > emptyColumn) * enlargementFactor);
            galaxy.YAfterEnlargement += emptyRows.Count(emptyRow => galaxy.YCoordinate > emptyRow) * enlargementFactor;

            galaxy.XCoordinate = galaxy.XAfterEnlargement;
            galaxy.YCoordinate = galaxy.YAfterEnlargement;
        }
    }

    private List<int> GetEmptyRows()
    {
        var emptyRows = new List<int>();

        for (var i = 0; i < _inputLength; i++)
        {
            if (_galaxies.Any(galaxy => galaxy.YCoordinate == i))
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
            if (_galaxies.Any(galaxy => galaxy.XCoordinate == i))
                continue;
            emptyColumns.Add(i);
        }

        return emptyColumns;
    }
}