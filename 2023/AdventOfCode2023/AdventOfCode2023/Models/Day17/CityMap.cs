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
    private readonly Dictionary<Coordinates, int> _distances = new();
    private readonly HashSet<Coordinates> _visited = new();
    private readonly Dictionary<Coordinates, List<Direction>> _lastThreeDirectionsToCoord = new();

    public int GetMinimalHeatLoss()
    {
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            _distances[new Coordinates(x, y)] = int.MaxValue;

        Dijkstra(_startCoordinates);

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
            var newCoordinates = GetNewCoordinates(direction, current);

            if (!IsValidMove(newCoordinates.GetYCoordinate(), newCoordinates.GetXCoordinate()) ||
                _distances[newCoordinates] >= minDistance) continue;

            minDistance = _distances[newCoordinates];
            minDistanceNeighbor = newCoordinates;
        }

        return minDistanceNeighbor;
    }

    private static Coordinates GetNewCoordinates(Direction direction, Coordinates current)
    {
        var newY = GetNewY(direction, current.GetYCoordinate());
        var newX = GetNewX(direction, current.GetXCoordinate());

        return new Coordinates(newX, newY);
    }

    private void Dijkstra(Coordinates source)
    {
        _distances[source] = Rows[source.GetYCoordinate()][source.GetXCoordinate()];

        for (var i = 0; i < Rows.Count; i++)
        {
            var currentRow = Rows[i];
            var yCoord = i;
            for (var j = 0; j < currentRow.Count; j++)
            {
                var xCoord = j;
                var currentNode = new Coordinates(xCoord, yCoord);
                var currentDistance = _distances[currentNode];

                if (!_lastThreeDirectionsToCoord.TryGetValue(currentNode, out List<Direction>? currentDirections))
                {
                    currentDirections = new List<Direction>();
                    _lastThreeDirectionsToCoord[currentNode] = currentDirections;
                }

                var neighbours = currentNode.GetNeighbours(Height, Width, _lastThreeDirectionsToCoord[currentNode]);

                foreach (var neighbour in neighbours)
                {
                    var heatLossForCoord = Rows[neighbour.GetYCoordinate()][neighbour.GetXCoordinate()];

                    var newDirection = currentNode.GetDirectionToNeighbour(neighbour);

                    var newDistance =
                        MathUtils.GetLowest(_distances[neighbour], currentDistance + heatLossForCoord);

                    if (newDistance >= _distances[neighbour])
                        continue;

                    _distances[neighbour] = newDistance;

                    if (!_lastThreeDirectionsToCoord.TryGetValue(neighbour, out List<Direction>? neighbourDirections))
                    {
                        neighbourDirections = new List<Direction>();
                        _lastThreeDirectionsToCoord[neighbour] = neighbourDirections;
                    }

                    foreach (var direction in currentDirections)
                    {
                        if (_lastThreeDirectionsToCoord[neighbour].Count == 3)
                            continue;
                        _lastThreeDirectionsToCoord[neighbour].Add(direction);
                    }

                    if (_lastThreeDirectionsToCoord[neighbour].Count > 3)
                        _lastThreeDirectionsToCoord[neighbour].RemoveAt(0);

                    _lastThreeDirectionsToCoord[neighbour].Add(newDirection);
                }
            }
        }
    }

    private bool IsValidMove(int y, int x)
    {
        return y >= 0 && y < Height && x >= 0 && x < Width;
    }

    private static int GetNewX(Direction direction, int xCoord)
    {
        return direction switch
        {
            Direction.Up => xCoord - 1,
            Direction.Down => xCoord + 1,
            _ => xCoord
        };
    }

    private static int GetNewY(Direction direction, int yCoord)
    {
        return direction switch
        {
            Direction.Left => yCoord - 1,
            Direction.Right => yCoord + 1,
            _ => yCoord
        };
    }
}