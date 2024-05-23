using AdventOfCode2023_1.Models.Day18.Enums;

namespace AdventOfCode2023_1.Models.Day18;

public static class Day18Extensions
{
    public static bool WasRunningOnTopOfWall(this NodeType type, NodeType otherType, Direction direction)
    {
        if (direction is Direction.Up or Direction.Down)
        {
            return type switch
            {
                NodeType.NorthEast => otherType == NodeType.SouthEast,
                NodeType.NorthWest => otherType == NodeType.SouthWest,
                NodeType.SouthWest => otherType == NodeType.NorthWest,
                NodeType.SouthEast => otherType == NodeType.NorthEast,
                _ => otherType == NodeType.Enclosed
            };
        }

        return type switch
        {
            NodeType.NorthEast => otherType == NodeType.NorthWest,
            NodeType.NorthWest => otherType == NodeType.NorthEast,
            NodeType.SouthWest => otherType == NodeType.SouthEast,
            NodeType.SouthEast => otherType == NodeType.SouthWest,
            _ => otherType == NodeType.Enclosed
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
}