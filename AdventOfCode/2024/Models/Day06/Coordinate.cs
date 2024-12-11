using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day06;

public class Coordinate(int x, int y): NodeBase<int>(x, y)
{
    public override (int, int) Move(Direction direction, int distance = 1)
        => direction switch
            {
                Direction.Up => (X, Y - distance),
                Direction.Right => (X + distance, Y),
                Direction.Down => (X, Y + distance),
                Direction.Left => (X - distance, Y),
                _ => throw new ArgumentOutOfRangeException()
            };

}