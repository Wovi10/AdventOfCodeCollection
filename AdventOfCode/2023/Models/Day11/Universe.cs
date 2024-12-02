using System.Numerics;
using UtilsCSharp.Utils;

namespace _2023.Models.Day11;

public class Universe<T> where T : struct, INumber<T>
{
    private T _inputLength;
    private T _lineLength;
    private readonly List<Galaxy<T>> _galaxies;

    public Universe(List<string> input)
    {
        _galaxies = GetGalaxies(input);
    }

    private List<Galaxy<T>> GetGalaxies(List<string> input)
    {
        var galaxies = new List<Galaxy<T>>();
        var lineCounter = T.Zero;
        _inputLength = T.CreateChecked(input.Count);
        foreach (var line in input)
        {
            var characterCounter = T.Zero;
            _lineLength = T.IsZero(_lineLength) ? T.CreateChecked(line.Length) : _lineLength;
            foreach (var character in line)
            {
                if (character == Constants.HashTag.First())
                    galaxies.Add(new Galaxy<T>(characterCounter, lineCounter));
                characterCounter++;
            }

            lineCounter++;
        }

        return galaxies;
    }

    public List<GalaxyPair<T>> GetGalaxyPairs()
    {
        var galaxyPairs = new List<GalaxyPair<T>>();
        for (var i = 0; i < _galaxies.Count; i++)
        {
            for (var j = i + 1; j < _galaxies.Count; j++)
            {
                galaxyPairs.Add(new GalaxyPair<T>(_galaxies[i], _galaxies[j]));
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
            galaxy.Enlarge(enlargementFactor, emptyColumns, emptyRows);
        }
    }

    private List<T> GetEmptyRows()
    {
        var emptyRows = new List<T>();

        for (var i = T.Zero; i < _inputLength; i++)
        {
            if (_galaxies.Any(galaxy => galaxy.Y == i))
                continue;
            emptyRows.Add(i);
        }

        return emptyRows;
    }

    private List<T> GetEmptyColumns()
    {
        var emptyColumns = new List<T>();
        for (var i = T.Zero; i < _lineLength - T.One; i++)
        {
            if (_galaxies.Any(galaxy => galaxy.X == i))
                continue;
            emptyColumns.Add(i);
        }

        return emptyColumns;
    }
}