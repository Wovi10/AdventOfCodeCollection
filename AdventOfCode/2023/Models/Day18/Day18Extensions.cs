using UtilsCSharp.Utils;

namespace _2023.Models.Day18;

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
}