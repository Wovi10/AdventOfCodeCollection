using AdventOfCode2023_1.Models.Day18.Enums;

namespace AdventOfCode2023_1.Models.Day18;

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public NodeType Type { get; set; }

    public void DecideType(List<Direction> neighbours)
    {
        switch (neighbours.Count)
        {
            case 4:
                Type = NodeType.Enclosed;
                return;
            case 3 when !neighbours.Contains(Direction.Up):
                Type = NodeType.NoNorth;
                return;
            case 3 when !neighbours.Contains(Direction.Right):
                Type = NodeType.NoEast;
                return;
            case 3 when !neighbours.Contains(Direction.Down):
                Type = NodeType.NoSouth;
                return;
            case 3 when !neighbours.Contains(Direction.Left):
                Type = NodeType.NoWest;
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