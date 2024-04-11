using System.Collections;
using System.Reflection;
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
    private readonly HashSet<Coordinates> _visited = new();
    private readonly Dictionary<Coordinates, List<Direction>> _lastDirectionsToCoord = new();

    public int GetMinimalHeatLoss()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
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

        var path = new List<Coordinates>();
        var current = EndCoordinates;
        while (!current.Equals(_startCoordinates))
        {
            path.Add(current);
            current = GetPreviousCell(current);
        }

        path.Add(_startCoordinates);
        path.Reverse();

        return path.Select(coord => Rows[coord.GetYCoordinate()][coord.GetXCoordinate()]).Sum();
    }

    private void Dijkstra()
    {
        _startCoordinates.MinimalHeatLoss = Rows[0][0];

        for (var i = 0; i < Rows.Count; i++)
        {
            var yCoord = i;
            for (var j = 0; j < Rows[i].Count; j++)
            {
                var xCoord = j;
                var currentNode = new Coordinates(xCoord, yCoord);
                var currentDistance = currentNode.MinimalHeatLoss;

                if (!_lastDirectionsToCoord.TryGetValue(currentNode, out List<Direction>? currentDirections))
                {
                    currentDirections = new List<Direction>();
                    _lastDirectionsToCoord[currentNode] = currentDirections;
                }

                var neighbours = currentNode.GetNeighbours(Height, Width, _lastDirectionsToCoord[currentNode]);

                foreach (var neighbour in neighbours)
                {
                    var heatLossForCoord = Rows[neighbour.GetYCoordinate()][neighbour.GetXCoordinate()];

                    var newDirection = currentNode.GetDirectionToNeighbour(neighbour);

                    var newDistance =
                        MathUtils.GetLowest(neighbour.MinimalHeatLoss, currentDistance + heatLossForCoord);

                    if (newDistance >= neighbour.MinimalHeatLoss)
                        continue;

                    neighbour.MinimalHeatLoss = newDistance;

                    _lastDirectionsToCoord[neighbour] = new List<Direction>();

                    foreach (var direction in currentDirections)
                    {
                        _lastDirectionsToCoord[neighbour].Add(direction);
                    }

                    _lastDirectionsToCoord[neighbour].Add(newDirection);
                    
                }

                if (currentNode.MinimalHeatLoss != int.MaxValue) 
                    currentNode.Visited = true;
            }
        }
    }

    private Coordinates GetPreviousCell(Coordinates current)
    {
        var minDistance = int.MaxValue;
        var minDistanceNeighbor = new Coordinates(-1, -1);

        var directions = new List<Direction>
        {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left
        };

        foreach (var direction in directions)
        {
            var newCoordinates = current.Move(direction);

            if (!IsValidMove(newCoordinates) ||
                newCoordinates.MinimalHeatLoss >= minDistance)
                continue;

            minDistance = newCoordinates.MinimalHeatLoss;
            minDistanceNeighbor = newCoordinates;
        }

        return minDistanceNeighbor;
    }

    private bool IsValidMove(Coordinates current)
    {
        var x = current.GetXCoordinate();
        var y = current.GetYCoordinate();

        return y >= 0 && y < Height && x >= 0 && x < Width;
    }
}