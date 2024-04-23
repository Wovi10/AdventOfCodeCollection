using System.Text;
using AdventOfCode2023_1.Models.Day18.Enums;
using AdventOfCode2023_1.Shared;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public class Grid
{
    public Grid(List<Node> nodes, int width, int height)
    {
        Nodes = nodes;
        Width = width;
        Height = height;
    }

    public List<Node> Nodes { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public void DigHole()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Nodes.Any(n => n.X == x && n.Y == y))
                    continue;

                var newNode = new Node {X = x, Y = y};

                var distanceUp = y;
                var distanceDown = Height - y;
                var lowest = MathUtils.GetLowest(distanceUp, distanceDown);
                var closestEdgeYAxis = lowest == distanceUp ? Direction.Up : Direction.Down;

                var distanceRight = Width - x;
                var distanceLeft = x;
                lowest = MathUtils.GetLowest(distanceRight, distanceLeft);
                var closestEdgeXAxis = lowest == distanceRight ? Direction.Right : Direction.Left;
                
                var closestEdge = GetClosestEdge(newNode.X, newNode.Y);
                
                var edgesCrossed = closestEdge switch
                {
                    Direction.Up => CountEdgesCrossedUp(newNode),
                    Direction.Right => CountEdgesCrossedRight(newNode),
                    Direction.Down => CountEdgesCrossedDown(newNode),
                    Direction.Left => CountEdgesCrossedLeft(newNode)
                };

                if (edgesCrossed.IsOdd())
                    Nodes.TryAddNode(newNode);
            }
        }
    }

    private Direction GetClosestEdge(int x, int y)
    {
        var distanceDown = Height - y;
        var distanceRight = Width - x;

        var lowestXAxis = MathUtils.GetLowest(distanceRight, x);
        var lowestYAxis = MathUtils.GetLowest(y, distanceDown);
        
        var closestEdgeYAxis = lowestYAxis == y ? Direction.Up : Direction.Down;
        var closestEdgeXAxis = lowestXAxis == distanceRight ? Direction.Right : Direction.Left;

        return lowestXAxis < lowestYAxis ? closestEdgeXAxis : closestEdgeYAxis;
        
    }

    private int CountEdgesCrossedUp(Node node)
        => CountEdgesCrossed(node.Y, 0, node.X, Constants.YAxis);

    private int CountEdgesCrossedRight(Node node)
        => CountEdgesCrossed(node.X + 1, Width, node.Y, Constants.XAxis);

    private int CountEdgesCrossedDown(Node node)
        => CountEdgesCrossed(node.Y + 1, Height, node.X, Constants.YAxis);

    private int CountEdgesCrossedLeft(Node node)
        => CountEdgesCrossed(node.X, 0, node.Y, Constants.XAxis);

    private int CountEdgesCrossed(int startPoint, int endPoint, int constantPart, string workingAxis)
    {
        var isOnYAxis = workingAxis == Constants.YAxis;
        var edgesCrossed = 0;
        var shouldDecrement = endPoint == 0;
        var previousWasEdge = false;
        var typesToSkip = new List<NodeType> {NodeType.Enclosed};
        if (isOnYAxis)
        {
            typesToSkip.Add(NodeType.NorthSouth);
            typesToSkip.Add(NodeType.NoEast);
            typesToSkip.Add(NodeType.NoWest);
        }
        else
        {
            typesToSkip.Add(NodeType.EastWest);
            typesToSkip.Add(NodeType.NoNorth);
            typesToSkip.Add(NodeType.NoSouth);
        }

        var endEdgeTypes = new List<NodeType>();
        if (isOnYAxis)
        {
            if (shouldDecrement)
            {
                endEdgeTypes.Add(NodeType.NoNorth);
                endEdgeTypes.Add(NodeType.SouthEast);
                endEdgeTypes.Add(NodeType.SouthWest);
            }
            else
            {
                endEdgeTypes.Add(NodeType.NoSouth);
                endEdgeTypes.Add(NodeType.NorthEast);
                endEdgeTypes.Add(NodeType.NorthWest);
            }
        }
        else
        {
            if (shouldDecrement)
            {
                endEdgeTypes.Add(NodeType.NoWest);
                endEdgeTypes.Add(NodeType.NorthEast);
                endEdgeTypes.Add(NodeType.SouthEast);
            }
            else
            {
                endEdgeTypes.Add(NodeType.NoEast);
                endEdgeTypes.Add(NodeType.NorthWest);
                endEdgeTypes.Add(NodeType.SouthWest);
            }
        }

        var startEdgeTypes = new List<NodeType>();
        if (isOnYAxis)
        {
            if (shouldDecrement)
            {
                startEdgeTypes.Add(NodeType.NoSouth);
                startEdgeTypes.Add(NodeType.NorthEast);
                startEdgeTypes.Add(NodeType.NorthWest);
            }
            else
            {
                startEdgeTypes.Add(NodeType.NoNorth);
                startEdgeTypes.Add(NodeType.SouthEast);
                startEdgeTypes.Add(NodeType.SouthWest);
            }
        }
        else
        {
            if (shouldDecrement)
            {
                startEdgeTypes.Add(NodeType.NoEast);
                startEdgeTypes.Add(NodeType.NorthWest);
                startEdgeTypes.Add(NodeType.SouthWest);
            }
            else
            {
                startEdgeTypes.Add(NodeType.NoWest);
                startEdgeTypes.Add(NodeType.NorthEast);
                startEdgeTypes.Add(NodeType.SouthEast);
            }
        }

        for (var i = startPoint; ShouldStop(i, endPoint);)
        {
            var currentNode = isOnYAxis
                ? Nodes.FirstOrDefault(n => n.X == constantPart && n.Y == i)
                : Nodes.FirstOrDefault(n => n.X == i && n.Y == constantPart);

            var nextNode = isOnYAxis
                ? Nodes.FirstOrDefault(n => n.X == constantPart && n.Y == i + 1)
                : Nodes.FirstOrDefault(n => n.X == i + 1 && n.Y == constantPart);
            
            var currentNodeIsEdge = currentNode != null;

            if (currentNode.Type)
            {
                
            }
            

        }

        return edgesCrossed;
    }

    private static bool ShouldStop(int index, int endPoint)
        => endPoint == 0 ? index >= endPoint : index < endPoint;

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var node = Nodes.FirstOrDefault(n => n.X == x && n.Y == y);
                sb.Append(node == null ? '.' : '#');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void DecideEdgeTypes()
    {
        foreach (var node in Nodes)
        {
            var neighbours = GetNeighbours(node.X, node.Y);
            node.DecideType(neighbours);
        }
    }

    private List<Direction> GetNeighbours(int x, int y)
    {
        var neighbours = new List<Direction>();
        var upNeighbour = Nodes.FirstOrDefault(n => n.X == x && n.Y == y - 1);
        var rightNeighbour = Nodes.FirstOrDefault(n => n.X == x + 1 && n.Y == y);
        var downNeighbour = Nodes.FirstOrDefault(n => n.X == x && n.Y == y + 1);
        var leftNeighbour = Nodes.FirstOrDefault(n => n.X == x - 1 && n.Y == y);

        if (upNeighbour != null)
            neighbours.Add(Direction.Up);

        if (rightNeighbour != null)
            neighbours.Add(Direction.Right);

        if (downNeighbour != null)
            neighbours.Add(Direction.Down);

        if (leftNeighbour != null)
            neighbours.Add(Direction.Left);

        return neighbours;
    }
}