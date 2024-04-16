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
    private readonly Dictionary<Coordinates, List<Direction>> _bestPathToCoord = new();

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

        var counter = 0;
        while (_coordinates.Any(coordinate => !coordinate.Visited) && counter++ < 10) 
            Dijkstra();

        var unvisitedCoordinates = _coordinates.Where(coordinate => !coordinate.Visited).ToList();
        foreach (var coordinates in unvisitedCoordinates)
        {
            _coordinates.Remove(coordinates);
        }
        
        var path = new List<Coordinates>();
        var current = EndCoordinates;
        while (!current.Equals(_startCoordinates))
        {
            path.Add(current);
            current = GetPreviousCell(current);
        }

        path.Add(_startCoordinates);
        path.Reverse();

        return _coordinates.TryGetValue(EndCoordinates, out var endCoordinates) ? endCoordinates.MinimalHeatLoss : 0;
    }

    private void Dijkstra()
    {
        _coordinates.TryGetValue(_startCoordinates, out var startCoordinates);

        if (startCoordinates is {Visited: false}) 
            startCoordinates.MinimalHeatLoss = 0;

        var timesInDirection = 0;
        var currentDirection = Direction.None;
        foreach (var currentCoordinate in _coordinates)
        {
            if (currentCoordinate.Equals(_startCoordinates))
            {
                currentCoordinate.SetCoordinate(0);
                continue;
            }

            var currentDistance = currentCoordinate.MinimalHeatLoss;
            var bestPathToCurrent = _bestPathToCoord.TryGetValue(currentCoordinate, out var bestPath)
                ? bestPath
                : new List<Direction>();

            var neighbours = currentCoordinate.GetNeighbours(currentDirection, timesInDirection, Height, Width);
            foreach (var neighbour in neighbours)
            {
                _coordinates.TryGetValue(neighbour, out var neighbourCoord);

                if (neighbourCoord == null)
                    continue;

                var heatLossForCoord = Rows[neighbourCoord.GetYCoordinate()][neighbourCoord.GetXCoordinate()];
                
                var possibleNewDistance = (currentDistance + heatLossForCoord) < 0 ? int.MaxValue : currentDistance + heatLossForCoord;
                    
                var newDistance =
                    MathUtils.GetLowest(neighbourCoord.MinimalHeatLoss, possibleNewDistance);

                if (newDistance >= neighbourCoord.MinimalHeatLoss)
                    continue;
                
                neighbourCoord.MinimalHeatLoss = newDistance;
                var bestPathToNeighbour = new List<Direction>(bestPath);
                bestPathToNeighbour.Add(currentDirection);
                
                
            }
        }

        for (var i = 0; i < Rows.Count; i++)
        {
            var yCoord = i;
            for (var j = 0; j < Rows[i].Count; j++)
            {
                var xCoord = j;
                _coordinates.TryGetValue(new Coordinates(xCoord, yCoord), out var currentNode);

                if (currentNode == null)
                    continue;

                var currentDistance = currentNode.MinimalHeatLoss;

                if (!_lastDirectionsToCoord.TryGetValue(currentNode, out List<Direction>? currentDirections))
                {
                    currentDirections = new List<Direction>();
                    _lastDirectionsToCoord[currentNode] = currentDirections;
                }

                var neighbours = currentNode.GetNeighbours(Height, Width, _lastDirectionsToCoord[currentNode]);

                foreach (var neighbour in neighbours)
                {
                    _coordinates.TryGetValue(neighbour, out var neighbourCoord);

                    if (neighbourCoord == null)
                        continue;

                    var heatLossForCoord = Rows[neighbourCoord.GetYCoordinate()][neighbourCoord.GetXCoordinate()];

                    var newDirection = currentNode.GetDirectionToNeighbour(neighbourCoord);

                    var possibleNewDistance = (currentDistance + heatLossForCoord) < 0 ? int.MaxValue : currentDistance + heatLossForCoord;
                    
                    var newDistance =
                        MathUtils.GetLowest(neighbourCoord.MinimalHeatLoss, possibleNewDistance);

                    if (newDistance >= neighbourCoord.MinimalHeatLoss)
                        continue;

                    neighbourCoord.MinimalHeatLoss = newDistance;

                    _lastDirectionsToCoord[neighbourCoord] = new List<Direction>();

                    foreach (var direction in currentDirections)
                    {
                        _lastDirectionsToCoord[neighbourCoord].Add(direction);
                    }

                    _lastDirectionsToCoord[neighbourCoord].Add(newDirection);

                    _coordinates.TryGetValue(currentNode, out var valueToAdjust);

                    if (valueToAdjust != null) 
                        valueToAdjust.Visited = true;
                }
            }
        }

        if (_coordinates.TryGetValue(EndCoordinates, out var endCoordinates)) 
            endCoordinates.Visited = endCoordinates.MinimalHeatLoss != int.MaxValue;
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
            var possibleNewCoords = current.Move(direction);

            _coordinates.TryGetValue(possibleNewCoords, out var newCoordinates);

            if (newCoordinates == null || !IsValidMove(newCoordinates) ||
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