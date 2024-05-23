using AdventOfCode2023_1.Models.Day18.Enums;
using AdventOfCode2023_1.Shared.Types;

namespace AdventOfCode2023_1.Models.Day18;

public class Node(int x, int y)
{
    public Point2D Coordinates { get; } = new(x, y);
    public NodeType Type { get; private set; }

    public void DecideType(HashSet<Direction> neighbours)
    {
        switch (neighbours.Count)
        {
            case 4:
                Type = NodeType.Enclosed;
                return;
            case 2 when neighbours.Contains(Direction.Up) && neighbours.Contains(Direction.Down):
                Type = NodeType.NorthSouth;
                return;
            case 2 when neighbours.Contains(Direction.Right) && neighbours.Contains(Direction.Left):
                Type = NodeType.EastWest;
                return;
            case 2 when neighbours.Contains(Direction.Up) && neighbours.Contains(Direction.Right):
                Type = NodeType.NorthEast;
                return;
            case 2 when neighbours.Contains(Direction.Up) && neighbours.Contains(Direction.Left):
                Type = NodeType.NorthWest;
                return;
            case 2 when neighbours.Contains(Direction.Down) && neighbours.Contains(Direction.Left):
                Type = NodeType.SouthWest;
                return;
            case 2 when neighbours.Contains(Direction.Down) && neighbours.Contains(Direction.Right):
                Type = NodeType.SouthEast;
                return;
            case 1 when neighbours.Contains(Direction.Up) || neighbours.Contains(Direction.Down):
                Type = NodeType.NorthSouth;
                return;
            case 1 when neighbours.Contains(Direction.Right) || neighbours.Contains(Direction.Left):
                Type = NodeType.EastWest;
                return;
        }
    }
}