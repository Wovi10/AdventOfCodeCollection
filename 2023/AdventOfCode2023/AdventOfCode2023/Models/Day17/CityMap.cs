using AdventOfCode2023_1.Models.Day17.Enums;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day17;

public class CityMap
{
    public CityMap(List<string> rows)
    {
        Rows = new List<List<int>>();
        foreach (var rowBlocks in rows.Select(row => row.Select(num => int.Parse(num.ToString())).ToList()))
        {
            Rows.Add(rowBlocks);
        }

        Height = Rows.Count;
        Width = Rows[0].Count;
        EndCoordinates = new Coordinates(Rows[0].Count - 1, Rows.Count - 1);
    }

    private List<List<int>> Rows { get; }
    private int Height { get; }
    private int Width { get; }
    private readonly Coordinates _startCoordinates = new(0, 0);
    private Coordinates EndCoordinates { get; }
    private readonly HashSet<Coordinates> _coordinates = new();
    private readonly Dictionary<Coordinates, List<Coordinates>> _bestPathToCoord = new();

    public int GetMinimalHeatLoss()
    {
        for (var y = Height-1; y >= 0; y--)
        {
            for (var x = Width-1; x >= 0; x--)
            {
                var coordinates = new Coordinates(x, y)
                {
                    MinimalHeatLoss = int.MaxValue
                };
                _coordinates.Add(coordinates);
            }
        }

        while (_coordinates.Any(coordinate => !coordinate.Visited))
            Dijkstra();

        _bestPathToCoord.TryGetValue(_startCoordinates, out var bestPath);

        if (bestPath == null)
            return 0;

        return _coordinates.TryGetValue(_startCoordinates, out var startCoordinates)
            ? startCoordinates.MinimalHeatLoss - Rows[0][0]
            : 0;
    }

    private void Dijkstra()
    {
        if (!_bestPathToCoord.TryGetValue(EndCoordinates, out _))
            _bestPathToCoord.Add(EndCoordinates, new List<Coordinates> {EndCoordinates});

        _coordinates.TryGetValue(EndCoordinates, out var endCoordinates);

        if (endCoordinates is {Visited: false})
        {
            endCoordinates.MinimalHeatLoss = Rows[endCoordinates.GetYCoordinate()][endCoordinates.GetXCoordinate()];
            endCoordinates.Visited = true;
        }

        foreach (var currentCoordinate in _coordinates)
        {
            if (currentCoordinate.Equals(EndCoordinates))
                currentCoordinate.SetCoordinate(0);

            if (currentCoordinate.MinimalHeatLoss == int.MaxValue)
                continue;

            var currentDistance = currentCoordinate.MinimalHeatLoss;
            if (!_bestPathToCoord.TryGetValue(currentCoordinate, out var bestPathToCurrent))
            {
                _bestPathToCoord.Add(currentCoordinate, new List<Coordinates>());
                bestPathToCurrent = _bestPathToCoord[currentCoordinate];
            }

            var neighbours = currentCoordinate.GetNeighbours(bestPathToCurrent, Height, Width);
            foreach (var neighbour in neighbours)
            {
                _coordinates.TryGetValue(neighbour, out var neighbourCoord);

                if (neighbourCoord == null)
                    continue;

                var heatLossForCoord = Rows[neighbourCoord.GetYCoordinate()][neighbourCoord.GetXCoordinate()];

                var possibleNewDistance = currentDistance + heatLossForCoord < 0
                    ? int.MaxValue
                    : currentDistance + heatLossForCoord;

                var newDistance =
                    MathUtils.GetLowest(neighbourCoord.MinimalHeatLoss, possibleNewDistance);

                if (newDistance >= neighbourCoord.MinimalHeatLoss)
                    continue;

                neighbourCoord.MinimalHeatLoss = newDistance;
                if (_bestPathToCoord.TryGetValue(neighbourCoord, out var bestPathToNeighbour))
                    bestPathToNeighbour.Clear();
                else
                {
                    _bestPathToCoord.Add(neighbourCoord, new List<Coordinates>());
                    bestPathToNeighbour = _bestPathToCoord[neighbourCoord];
                }

                bestPathToNeighbour.AddRange(bestPathToCurrent);
                bestPathToNeighbour.Add(neighbourCoord);

                neighbourCoord.Visited = true;
            }
        }

        if (_coordinates.TryGetValue(_startCoordinates, out var startCoordinates))
            startCoordinates.Visited = startCoordinates.MinimalHeatLoss != int.MaxValue;
    }
}