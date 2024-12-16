using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day10;

public class Coordinate(int internalX, int internalY) : NodeBase<int>(internalX, internalY)
{
    public Coordinate((int, int) coordinate) : this(coordinate.Item1, coordinate.Item2) { }

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        return direction switch
        {
            Direction.Up => (internalX, internalY - distance),
            Direction.Right => (internalX + distance, internalY),
            Direction.Down => (internalX, internalY + distance),
            Direction.Left => (internalX - distance, internalY),
            _ => (internalX, internalY)
        };
    }
}