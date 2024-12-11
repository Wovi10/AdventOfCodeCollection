using UtilsCSharp.Enums;
using UtilsCSharp.Utils;

namespace _2024.Models.Day06;

public class Map
{
    public Map(List<string> input)
    {
        Height = input.Count;
        var positions = new List<Position>();

        for (var y = 0; y < input.Count; y++)
        {
            var row = input[y];
            Width = row.Length;
            for (var x = 0; x < row.Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                var character = row[x];
                var position = new Position(coordinate, character);

                if (position.IsGuard)
                {
                    GuardPosition = position.Coordinate;
                    position.SetVisited();
                }

                positions.Add(position);
            }
        }

        Positions = positions.ToArray();
        Obstacles = Positions.Where(p => p.IsObstacle).Select(p => p.Coordinate).ToArray();
    }

    private int Height { get; }
    private int Width { get; }
    private Direction GuardDirection { get; set; } = Direction.Up;
    private Position[] Positions { get; }
    private Coordinate GuardPosition { get; } = new Coordinate(-1, -1);
    private Coordinate[] Obstacles { get; }

    public void StartRunning()
    {
        var current = GuardPosition;
        var next = current.Move(GuardDirection).ToCoordinate();
        SetCoordinateVisited(next);

        while (IsOnMap(next))
        {
            if (Obstacles.Contains(next))
            {
                GuardDirection = GuardDirection switch
                {
                    Direction.Up => Direction.Right,
                    Direction.Right => Direction.Down,
                    Direction.Down => Direction.Left,
                    Direction.Left => Direction.Up,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                current = next;
                SetCoordinateVisited(current);
            }

            next = current.Move(GuardDirection).ToCoordinate();
        }
    }

    public long CountVisited()
        => Positions.Count(p => p.IsVisited);

    private bool IsOnMap(Coordinate next)
        => next.X >= 0 && next.X < Width && (next.Y >= 0 && next.Y < Height);

    private void SetCoordinateVisited(Coordinate coordinate)
        => Positions.First(p => p.Coordinate == coordinate).SetVisited();
}