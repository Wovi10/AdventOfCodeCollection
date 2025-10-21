using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day18;

public class Coordinate(int internalX, int internalY): NodeBase<int>(internalX, internalY)
{
    public Coordinate(Tuple<int, int> coords): this(coords.Item1, coords.Item2) { }

    public override (int, int) Move(Direction direction, int distance = 1)
        => direction switch
        {
            Direction.Up => (internalX, internalY + distance),
            Direction.Down => (internalX, internalY - distance),
            Direction.Left => (internalX - distance, internalY),
            Direction.Right => (internalX + distance, internalY),
            _ => (internalX, internalY)
        };
}