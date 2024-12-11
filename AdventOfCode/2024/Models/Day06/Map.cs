using UtilsCSharp.Enums;
using UtilsCSharp.Utils;

namespace _2024.Models.Day06;

public class Map
{
    private readonly HashSet<Coordinate> _obstacles = new();
    private readonly Dictionary<Coordinate, Position> _positionLookup = new();

    public Map(List<string> input)
    {
        Height = input.Count;

        for (var y = 0; y < input.Count; y++)
        {
            var row = input[y];
            Width = row.Length;
            for (var x = 0; x < row.Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                var character = row[x];
                var position = new Position(coordinate, character);

                _positionLookup.Add(coordinate, position);

                if (position.IsGuard)
                {
                    GuardCoordinate = position.Coordinate;
                    position.SetVisited(true);
                }

                if (position.IsObstacle)
                    _obstacles.Add(coordinate);

                Positions.Add(position);
            }
        }
    }

    private int Height { get; }
    private int Width { get; }
    private Direction GuardDirection { get; set; } = Direction.Up;
    private List<Position> Positions { get; } = new();
    private Coordinate GuardCoordinate { get; } = new(-1, -1);

    public List<Position> GetOriginalPath()
    {
        Run(out _);
        return Positions.Where(p => p.IsVisited).ToList();
    }

    public void Run(out bool looped)
    {
        looped = false;
        var current = GuardCoordinate;

        while (true)
        {
            var currentAsPosition = _positionLookup[current];
            if (currentAsPosition.IsVisitedTwice)
            {
                looped = true;
                return;
            }

            var next = current.Move(GuardDirection).ToCoordinate();
            if (!IsOnMap(next))
                break;

            var nextAsPosition = _positionLookup[next];
            if (_obstacles.Contains(next))
                RotateGuardDirection();
            else
                current = MarkAsVisited(next, nextAsPosition, out currentAsPosition);
        }
    }

    private Coordinate MarkAsVisited(Coordinate next, Position nextAsPosition, out Position currentAsPosition)
    {
        currentAsPosition = nextAsPosition;

        SetCoordinateVisited(next);
        return next;
    }

    private void RotateGuardDirection()
        => GuardDirection = (Direction)(((int)GuardDirection + 1) % 4);

    public long CountVisited()
        => Positions.Count(p => p.IsVisited);

    public void Reset()
    {
        GuardDirection = Direction.Up;

        foreach (var position in Positions)
            position.Reset();
    }

    private bool IsOnMap(Coordinate coordinate)
        => coordinate.X >= 0 && coordinate.X < Width && coordinate.Y >= 0 && coordinate.Y < Height;

    private void SetCoordinateVisited(Coordinate coordinate)
    {
        var position = _positionLookup[coordinate];
        position.SetVisited(true);
        position.SetWayVisited(GuardDirection);
    }

    public void AddObstacle(Coordinate positionCoordinate)
        => _obstacles.Add(positionCoordinate);

    public void RemoveObstacle(Coordinate positionCoordinate)
        => _obstacles.Remove(positionCoordinate);
}