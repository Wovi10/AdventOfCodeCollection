using AdventOfCode2023_1.Models.Day18.Enums;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public static class Day18Extensions
{
    public static bool WasRunningOnTopOfWall(this NodeType currentType, NodeType startOfWall, Direction direction)
    {
        return direction switch
        {
            Direction.Up => startOfWall switch
            {
                NodeType.NorthEast => currentType == NodeType.SouthEast,
                NodeType.NorthWest => currentType == NodeType.SouthWest,
                _ => false
            },
            Direction.Right => startOfWall switch
            {
                NodeType.NorthEast => currentType == NodeType.NorthWest,
                NodeType.SouthEast => currentType == NodeType.SouthWest,
                _ => false
            },
            Direction.Down => startOfWall switch
            {
                NodeType.SouthEast => currentType == NodeType.NorthEast,
                NodeType.SouthWest => currentType == NodeType.NorthWest,
                _ => false
            },
            Direction.Left => startOfWall switch
            {
                NodeType.NorthWest => currentType == NodeType.NorthEast,
                NodeType.SouthWest => currentType == NodeType.SouthEast,
                _ => false
            },
            _ => false
        };
    }

    public static (int, int) CharToDirection(this char direction)
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

    public static (int, int) IntToDirection(this char direction)
    {
        return direction switch
        {
            '0' => (1, 0),
            '1' => (0, 1),
            '2' => (-1, 0),
            '3' => (0, -1),
            _ => (0, 0)
        };
    }

    public static bool IsInHole(this int edgesCrossed) 
        => edgesCrossed.IsOdd();
}