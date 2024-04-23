using AdventOfCode2023_1.Models.Day18.Enums;

namespace AdventOfCode2023_1.Models.Day18;

public static class Day18Extensions
{
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

    public static void TryAddNode(this List<Node> nodes, Node? newNode)
    {
        if (newNode == null)
            return;

        if (nodes.Any(x => x.X == newNode.X && x.Y == newNode.Y) == false)
            nodes.Add(newNode);
    }

    public static bool IsMatching(this NodeType type, NodeType otherType, bool isOnYAxis)
    {
        if (isOnYAxis)
        {
            return otherType == type switch
            {
                NodeType.NorthEast => NodeType.SouthEast,
                NodeType.NorthWest => NodeType.SouthWest,
                NodeType.SouthWest => NodeType.NorthWest,
                NodeType.SouthEast => NodeType.NorthEast,
                _ => NodeType.Enclosed
            };
        }

        return otherType == type switch
        {
            NodeType.NorthEast => NodeType.NorthWest,
            NodeType.NorthWest => NodeType.NorthEast,
            NodeType.SouthWest => NodeType.SouthEast,
            NodeType.SouthEast => NodeType.SouthWest,
            _ => NodeType.Enclosed
        };
    }
}