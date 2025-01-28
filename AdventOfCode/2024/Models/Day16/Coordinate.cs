using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day16;

public class Coordinate: NodeBase<int>
{
    public Coordinate(int internalX, int internalY) : base(internalX, internalY)
    {
    }

    public Coordinate((int, int) valueTuple) : base(valueTuple.Item1, valueTuple.Item2)
    {
    }

    public override (int, int) Move(Direction direction, int distance = 1)
        => direction switch
        {
            Direction.Up => (X, Y - distance),
            Direction.Down => (X, Y + distance),
            Direction.Left => (X - distance, Y),
            Direction.Right => (X + distance, Y),
            _ => (X, Y)
        };

    public Coordinate GetCoordinateUp()
        => new(X, Y - 1);

    public Coordinate GetCoordinateRight()
        => new(X + 1, Y);

    public Coordinate GetCoordinateDown()
        => new(X, Y + 1);

    public Coordinate GetCoordinateLeft()
        => new(X - 1, Y);

    public ReindeerPositioning[] GetNeighbouringCoordinatesWithDirection()
        =>
        [
            new(new(X, Y - 1), Direction.Up),
            new(new(X+1,Y), Direction.Right),
            new(new(X,Y+1), Direction.Down),
            new(new(X-1,Y), Direction.Left)
        ];
}