using System.Collections;
using System.Reflection;
using AdventOfCode2023_1.Models.Day17.Enums;

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

        var currentDirection = Direction.None;
        var consecutiveDirections = 0;

        while (true)
        {
            var minDistanceNode = GetMinDistanceNode();

            if (minDistanceNode.Equals(new Coordinates(-1, -1)))
                break;

            _visited.Add(minDistanceNode);

            foreach (var neighbour in GetNeighbours(minDistanceNode, currentDirection))
            {
                var newDistance = _distances[minDistanceNode] +
                                  Rows[neighbour.GetYCoordinate()][neighbour.GetXCoordinate()];

                if (newDistance < _distances[neighbour])
                    _distances[neighbour] = newDistance;
            }

            var newDirection = GetDirectionFromPreviousNode(minDistanceNode, GetPreviousCell(minDistanceNode));

            if (newDirection == currentDirection)
            {
                consecutiveDirections++;
                if (consecutiveDirections >= 3)
                {
                    currentDirection = Direction.None;
                    consecutiveDirections = 0;
                }
            }
            else
            {
                currentDirection = newDirection;
                consecutiveDirections = 0;
            }
        }
    }

    private Direction GetDirectionFromPreviousNode(Coordinates current, Coordinates previous)
    {
        // Calculate the direction from the previous node to the current node
        var deltaY = current.GetYCoordinate() - previous.GetYCoordinate();
        var deltaX = current.GetXCoordinate() - previous.GetXCoordinate();

        return deltaY switch
        {
            -1 when deltaX == 0 => Direction.Up,
            1 when deltaX == 0 => Direction.Down,
            0 when deltaX == -1 => Direction.Left,
            0 when deltaX == 1 => Direction.Right,
            _ => Direction.None
        };
    }

    private IEnumerable<Coordinates> GetNeighbours(Coordinates cell, Direction currentDirection)
    {
        var directions = new List<Direction>
        {
            Direction.Up,
            Direction.Right,
            Direction.Down,
            Direction.Left
        };

        var oppositeDirection = GetOppositeDirection(currentDirection);
        if (oppositeDirection != Direction.None)
            directions.Remove(oppositeDirection);

        foreach (var direction in directions)
        {
            var newCoordinates = GetNewCoordinates(direction, cell);

            if (IsValidMove(newCoordinates.GetYCoordinate(), newCoordinates.GetXCoordinate()))
                yield return newCoordinates;
        }
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => Direction.None
        };
    }

    private bool IsValidMove(int y, int x)
    {
        return y >= 0 && y < Height && x >= 0 && x < Width;
    }

    private Coordinates GetMinDistanceNode()
    {
        var minDistanceNode = new Coordinates(-1, -1);
        var minDistance = int.MaxValue;

        foreach (var node in _distances)
        {
            if (_visited.Contains(node.Key) || node.Value >= minDistance)
                continue;

            minDistance = node.Value;
            minDistanceNode = node.Key;
        }

        return minDistanceNode;
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