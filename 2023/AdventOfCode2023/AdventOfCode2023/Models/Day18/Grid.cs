using System.Text;
using AdventOfCode2023_1.Models.Day18.Enums;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public class Grid(List<Node> nodes, int width, int height)
{
    public List<Node> Nodes { get; } = nodes;
    private int Height { get; } = height;
    private int Width { get; } = width;

    public void DigHole()
    {
        var nodesToAdd = new List<Node>();
        var nodeSet = new HashSet<(int,int)>(Nodes.Select(n => (n.X, n.Y)));
        var edgeCrossedCalculators = new Dictionary<Direction, Func<Node, int>>
        {
            {Direction.Up, node => CountEdgesCrossed(node.Y - 1, 0, node.X, true, true)},
            {Direction.Right, node => CountEdgesCrossed(node.X, Width, node.Y, false)},
            {Direction.Down, node => CountEdgesCrossed(node.Y, Height, node.X, true)},
            {Direction.Left, node => CountEdgesCrossed(node.X-1, 0, node.Y, false, true)}
        };

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (nodeSet.Contains((x, y)))
                    continue;

                var newNode = new Node {X = x, Y = y};

                var closestEdge = GetClosestEdge(newNode.X, newNode.Y);

                var edgesCrossed = edgeCrossedCalculators[closestEdge](newNode);

                if (edgesCrossed.IsOdd())
                    nodesToAdd.TryAddNode(newNode);
            }
        }

        nodesToAdd.ForEach(node => Nodes.TryAddNode(node));
    }

    private Direction GetClosestEdge(int x, int y)
    {
        var distanceDown = Height - y;
        var distanceRight = Width - x;
        
        var minDistance = MathUtils.GetLowest(MathUtils.GetLowest(y, distanceDown), MathUtils.GetLowest(x, distanceRight));
        
        if (minDistance == distanceRight)
            return Direction.Right;

        if (minDistance == distanceDown)
            return Direction.Down;

        return x < y 
                ? Direction.Left
                : Direction.Up;
    }

    private int CountEdgesCrossed(int startPoint, int endPoint, int constantPart, bool isOnYAxis, bool shouldDecrement = false)
    {
        var edgesCrossed = 0;
        var previousWasEdge = false;

        var typesToSkip = new HashSet<NodeType>{NodeType.Enclosed};
        var startEdgeTypes = new HashSet<NodeType>();

        if (isOnYAxis)
        {
            typesToSkip.Add(NodeType.NorthSouth );

            startEdgeTypes.Add(NodeType.EastWest);
            startEdgeTypes.UnionWith(shouldDecrement
                ? new[] {NodeType.NorthEast, NodeType.NorthWest}
                : new[] {NodeType.SouthEast, NodeType.SouthWest});
        }
        else
        {
            typesToSkip.Add(NodeType.EastWest);

            startEdgeTypes.Add(NodeType.NorthSouth);
            startEdgeTypes.UnionWith(shouldDecrement
                ? new[] {NodeType.NorthWest, NodeType.SouthWest}
                : new[] {NodeType.NorthEast, NodeType.SouthEast});
        }

        var nodeSet = new Dictionary<(int, int), Node>(Nodes.Select(n => new KeyValuePair<(int, int), Node>((n.X, n.Y), n)));

        var startOfWall = NodeType.Enclosed;

        for (var i = startPoint; ShouldStop(i, endPoint);)
        {
            var currentNode = isOnYAxis
                ? nodeSet.GetValueOrDefault((constantPart,i))
                : nodeSet.GetValueOrDefault((i, constantPart));

            if (shouldDecrement) i--;
            else i++;

            if (currentNode == null)
            {
                previousWasEdge = false;
                continue;
            }

            var currentNodeType = currentNode.Type;

            if (!previousWasEdge)
            {
                startOfWall = currentNodeType;
            }

            if (typesToSkip.Contains(currentNodeType))
            {
                previousWasEdge = true;
                continue;
            }

            if (startEdgeTypes.Contains(currentNodeType) && !previousWasEdge) 
                edgesCrossed++;

            if (currentNodeType.IsMatching(startOfWall, isOnYAxis) && previousWasEdge)
            {
                edgesCrossed--;
                continue;
            }

            previousWasEdge = true;
        }

        return edgesCrossed;
    }

    private static bool ShouldStop(int index, int endPoint)
        => endPoint == 0 ? index >= endPoint : index < endPoint;

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
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