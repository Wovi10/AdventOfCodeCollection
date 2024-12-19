using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day12;

public class Coordinate(int x, int y):NodeBase<int>(x, y)
{
    public Coordinate((int, int) coordinates) : this(coordinates.Item1, coordinates.Item2)
    {
    }

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        return direction switch
        {
            Direction.Up => (X, Y + distance),
            Direction.Right => (X + distance, Y),
            Direction.Down => (X, Y - distance),
            Direction.Left => (X - distance, Y),
            _ => (X, Y)
        };
    }

    private static readonly Direction[] Directions = {Direction.Up, Direction.Right, Direction.Down, Direction.Left};

    public List<Coordinate> GetNeighbours()
        => Directions.Select(direction => Move(direction)).Select(neighbour => new Coordinate(neighbour)).ToList();
}