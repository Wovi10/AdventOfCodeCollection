using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day15;

public class Coordinate(int internalX, int internalY) : NodeBase<int>(internalX, internalY)
{
    public override (int, int) Move(Direction direction, int distance = 1)
    {
        return direction switch
        {
            Direction.Up => (X, Y - distance),
            Direction.Down => (X, Y + distance),
            Direction.Left => (X - distance, Y),
            Direction.Right => (X + distance, Y),
            _ => (X, Y)
        };
    }
}