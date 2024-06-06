using System.Numerics;
using UtilsCSharp.Utils;

namespace AdventOfCode2023_1.Models.Day18;

public static class Day18Extensions
{
    public static (int, int) ToOffset(this char direction)
    {
        return direction switch
        {
            'U' => Offset.Up,
            'R' => Offset.Right,
            'D' => Offset.Down,
            'L' => Offset.Left,
            _ => Offset.Still
        };
    }

    public static (int, int) ToOffset(this int direction)
    {
        const int intRight = 0;
        const int intDown = 1;
        const int intLeft = 2;
        const int intUp = 3;

        return direction switch
        {
            intRight => Offset.Right,
            intDown => Offset.Down,
            intLeft => Offset.Left,
            intUp => Offset.Up,
            _ => Offset.Still
        };
    }

    public static Node<T> Move<T>(this Node<T> position, DigInstruction<T> instruction) where T : ISignedNumber<T>
    {
        var (dir, len) = instruction;
        var (dx, dy) = dir switch
        {
            _ when dir == Offset.Left => (-len, 0),
            _ when dir == Offset.Right => (+len, 0),
            _ when dir == Offset.Up => (0, -len),
            _ when dir == Offset.Down => (0, +len)
        };

        return new Node<T>(position.Add(position.X, dx), position.Add(position.Y, dy));
    }
}