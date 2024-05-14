using System.Collections.Concurrent;
using AdventOfCode2023_1.Models.Day18.Enums;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public class Grid
{
    public Grid(List<Node> nodes, int width, int height)
    {
        Nodes = nodes;
        Height = height;
        Width = width;
        NodeDictionary = nodes.ToDictionary(n => (n.X, n.Y), n => n);
    }

    public List<Node> Nodes { get; }
    private Dictionary<(int, int), Node> NodeDictionary { get; }
    private int Height { get; }
    private int Width { get; }

    public void DigHole()
    {
        var edgeCrossedCalculators = new Dictionary<Direction, Func<Node, int>>
        {
            {Direction.Up, CountEdgesCrossedUp},
            {Direction.Right, CountEdgesCrossedRight},
            {Direction.Down, CountEdgesCrossedDown},
            {Direction.Left, CountEdgesCrossedLeft}
        };

        // x and y start from 1. The edges are either already added or are not needed
        Parallel.For(1, Height, y =>
        {
            var nodesToAdd = new List<Node>();
            for (var x = 1; x < Width; x++)
            {
                if (NodeDictionary.ContainsKey((x, y)))
                    continue;

                var newNode = new Node {X = x, Y = y};
                var closestEdge = GetClosestEdge(newNode.X, newNode.Y);
                
                var edgesCrossed = edgeCrossedCalculators[closestEdge](newNode);

                if (edgesCrossed.IsOdd())
                    nodesToAdd.Add(newNode);
            }

            foreach (var node in nodesToAdd)
                Nodes.Add(node);
            nodesToAdd.Clear();
        });
    }

    private int CountEdgesCrossedUp(Node node)
        => CountEdgesCrossed(Direction.Up, node.Y, node.X);

    private int CountEdgesCrossedRight(Node node)
        => CountEdgesCrossed(Direction.Right, node.X, node.Y);

    private int CountEdgesCrossedDown(Node node)
        => CountEdgesCrossed(Direction.Down, node.Y + 1, node.X);

    private int CountEdgesCrossedLeft(Node node)
        => CountEdgesCrossed(Direction.Left, node.X, node.Y);

    private Direction GetClosestEdge(int x, int y)
    {
        var distanceDown = Height - y;
        var distanceRight = Width - x;

        var minDistance =
            MathUtils.GetLowest(MathUtils.GetLowest(y, distanceDown), MathUtils.GetLowest(x, distanceRight));

        if (minDistance == distanceRight)
            return Direction.Right;

        if (minDistance == distanceDown)
            return Direction.Down;

        return x < y
            ? Direction.Left
            : Direction.Up;
    }

    private int CountEdgesCrossed(Direction direction, int startPoint, int constantPart)
    {
        HashSet<NodeType> typesToSkip;
        HashSet<NodeType> startEdgeTypes;

        switch (direction)
        {
            case Direction.Up:
                typesToSkip = new HashSet<NodeType> {NodeType.NorthSouth, NodeType.Enclosed};
                startEdgeTypes = new HashSet<NodeType> {NodeType.EastWest, NodeType.NorthEast, NodeType.NorthWest};
                break;
            case Direction.Right:
                typesToSkip = new HashSet<NodeType> {NodeType.EastWest, NodeType.Enclosed};
                startEdgeTypes = new HashSet<NodeType> {NodeType.NorthSouth, NodeType.NorthEast, NodeType.SouthEast};
                break;
            case Direction.Down:
                typesToSkip = new HashSet<NodeType> {NodeType.NorthSouth, NodeType.Enclosed};
                startEdgeTypes = new HashSet<NodeType> {NodeType.EastWest, NodeType.SouthEast, NodeType.SouthWest};
                break;
            case Direction.Left:
                typesToSkip = new HashSet<NodeType> {NodeType.EastWest, NodeType.Enclosed};
                startEdgeTypes = new HashSet<NodeType> {NodeType.NorthSouth, NodeType.NorthWest, NodeType.SouthWest};
                break;
            default:
                typesToSkip = new HashSet<NodeType>();
                startEdgeTypes = new HashSet<NodeType>();
                break;
        }

        var nodesToCheck = GetNodesToCheck(direction, startPoint, constantPart);

        if (nodesToCheck.Count == 0)
            return 0;

        var edgesCrossed = 0;
        var startOfWall = NodeType.Enclosed;

        foreach (var (_, currentNode) in nodesToCheck)
        {
            var nodeType = currentNode.Type;
            if (typesToSkip.Contains(nodeType))
                continue;

            if (startOfWall == NodeType.Enclosed)
            {
                startOfWall = nodeType;

                edgesCrossed++;
                continue;
            }

            if (startEdgeTypes.Contains(nodeType))
            {
                edgesCrossed++;
                continue;
            }

            if (nodeType.IsMatching(startOfWall, direction)) // Was running on top of wall
                edgesCrossed--;
        }

        return edgesCrossed;
    }

    private Dictionary<(int, int), Node> GetNodesToCheck(Direction direction, int startPoint, int constantPart)
    {
        return direction switch
        {
            Direction.Up => NodeDictionary.Where(kvp =>
                    kvp.Key.Item1 == constantPart && kvp.Key.Item2 <= startPoint && kvp.Key.Item2 >= 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Direction.Right => NodeDictionary.Where(kvp =>
                    kvp.Key.Item2 == constantPart && kvp.Key.Item1 >= startPoint && kvp.Key.Item1 <= Width)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Direction.Down => NodeDictionary.Where(kvp =>
                    kvp.Key.Item1 == constantPart && kvp.Key.Item2 >= startPoint && kvp.Key.Item2 <= Height)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Direction.Left => NodeDictionary.Where(kvp =>
                    kvp.Key.Item2 == constantPart && kvp.Key.Item1 <= startPoint && kvp.Key.Item1 >= 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            _ => new Dictionary<(int, int), Node>()
        };
    }

    public void DecideEdgeTypes()
    {
        Parallel.ForEach(NodeDictionary.Keys, key =>
        {
            var neighbours = GetNeighbours(key);
            NodeDictionary[key].DecideType(neighbours);
        });
    }

    private List<Direction> GetNeighbours((int, int) key)
    {
        var neighbours = new List<Direction>();
        var offSets = new List<(int, int)> {(0, -1), (1, 0), (0, 1), (-1, 0)};
        var directions = new List<Direction> {Direction.Up, Direction.Right, Direction.Down, Direction.Left};

        for (var i = 0; i < offSets.Count; i++)
        {
            var (offSetX, offSetY) = offSets[i];
            var neighbourPosition = (key.Item1 + offSetX, key.Item2 + offSetY);
            if (NodeDictionary.ContainsKey(neighbourPosition))
                neighbours.Add(directions[i]);
        }

        return neighbours;
    }
}