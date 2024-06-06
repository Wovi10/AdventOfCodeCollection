using System.Numerics;

namespace AdventOfCode2023_1.Models.Day18;

public static class Day18Extensions
{
    public static (int, int) ToOffset(this char direction)
    {
        return direction switch
        {
            'U' => (0, -1),
            'R' => (1, 0),
            'D' => (0, 1),
            'L' => (-1, 0),
            _ => (0, 0)
        };
    }

    public static (int, int) ToOffset(this int direction)
    {
        return direction switch
        {
            0 => (1, 0),
            1 => (0, 1),
            2 => (-1, 0),
            3 => (0, -1),
            _ => (0, 0)
        };
    }

    public static Node<T> Move<T>(this Node<T> position, DigInstruction<T> instruction) where T : ISignedNumber<T>
    {
        var (dir, len) = instruction;
        var (dx, dy) = dir switch
        {
            (0, -1) => (-len, 0),
            (0, 1) => (+len, 0),
            (-1, 0) => (0, -len),
            (1, 0) => (0, +len)
        };

        return new Node<T>(position.Add(position.X, dx), position.Add(position.Y, dy));
    }
}