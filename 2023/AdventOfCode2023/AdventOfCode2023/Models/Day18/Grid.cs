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

        var nodesToAdd = new ConcurrentBag<Node>();

        Parallel.For(0, Height, y =>
        {
            for (var x = 0; x < Width; x++)
            {
                if (NodeDictionary.ContainsKey((x, y)))
                    continue;

                var newNode = new Node {X = x, Y = y};

                var closestEdge = GetClosestEdge(newNode.X, newNode.Y);
                if (newNode.X != 6460 && newNode.Y != 6460)
                {
                    continue;
                }
                var edgesCrossed = edgeCrossedCalculators[closestEdge](newNode);

                if (edgesCrossed.IsOdd())
                    nodesToAdd.Add(newNode);
            }
        });

        foreach (var node in nodesToAdd)
            Nodes.TryAddNode(node);
    }

    private int CountEdgesCrossedUp(Node node)
        => CountEdgesCrossed(node.Y, 0, node.X, true, true);

    private int CountEdgesCrossedRight(Node node)
        => CountEdgesCrossed(node.X, Width, node.Y, false);

    private int CountEdgesCrossedDown(Node node)
        => CountEdgesCrossed(node.Y + 1, Height, node.X, true);

    private int CountEdgesCrossedLeft(Node node)
        => CountEdgesCrossed(node.X, 0, node.Y, false, true);

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

    private readonly HashSet<NodeType> _typesToSkipForYAxis = new() {NodeType.NorthSouth, NodeType.Enclosed};
    private readonly HashSet<NodeType> _typesToSkipForXAxis = new() {NodeType.EastWest, NodeType.Enclosed};

    private readonly HashSet<NodeType> _startEdgeTypesYAxisDecrement =
        new() {NodeType.EastWest, NodeType.NorthEast, NodeType.NorthWest};

    private readonly HashSet<NodeType> _startEdgeTypesYAxisIncrement =
        new() {NodeType.EastWest, NodeType.SouthEast, NodeType.SouthWest};

    private readonly HashSet<NodeType> _startEdgeTypesXAxisDecrement =
        new() {NodeType.NorthSouth, NodeType.NorthWest, NodeType.SouthWest};

    private readonly HashSet<NodeType> _startEdgeTypesXAxisIncrement =
        new() {NodeType.NorthSouth, NodeType.NorthEast, NodeType.SouthEast};

    private int CountEdgesCrossed(int startPoint, int endPoint, int constantPart, bool isOnYAxis,
        bool shouldDecrement = false)
    {
        var edgesCrossed = 0;

        HashSet<NodeType> typesToSkip;
        HashSet<NodeType> startEdgeTypes;

        if (isOnYAxis)
        {
            typesToSkip = _typesToSkipForYAxis;
            startEdgeTypes = shouldDecrement ? _startEdgeTypesYAxisDecrement : _startEdgeTypesYAxisIncrement;
        }
        else
        {
            typesToSkip = _typesToSkipForXAxis;
            startEdgeTypes = shouldDecrement ? _startEdgeTypesXAxisDecrement : _startEdgeTypesXAxisIncrement;
        }

        var nodesToCheck = GetNodesToCheck(startPoint, endPoint, constantPart, isOnYAxis, shouldDecrement);

        var startOfWall = NodeType.Enclosed;
        foreach (var (key, currentNode) in nodesToCheck)
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

            if (nodeType.IsMatching(startOfWall, isOnYAxis)) // Was running on top of wall
                edgesCrossed--;
        }

        return edgesCrossed;
    }

    private Dictionary<(int, int),Node> GetNodesToCheck(int startPoint, int endPoint, int constantPart, bool isOnYAxis, bool shouldDecrement)
    {
        if (isOnYAxis)
        {
            if (shouldDecrement)
            {
                return NodeDictionary.Where(kvp =>
                        kvp.Key.Item1 == constantPart && kvp.Key.Item2 <= startPoint && kvp.Key.Item2 >= endPoint)
                    .ToDictionary();
            }

            return NodeDictionary.Where(kvp =>
                    kvp.Key.Item1 == constantPart && kvp.Key.Item2 >= startPoint && kvp.Key.Item2 <= endPoint)
                .ToDictionary();
        }

        if (shouldDecrement)
        {
            return NodeDictionary.Where(kvp =>
                    kvp.Key.Item2 == constantPart && kvp.Key.Item1 <= startPoint && kvp.Key.Item1 >= endPoint)
                .ToDictionary();
        }

        return NodeDictionary.Where(kvp =>
                kvp.Key.Item2 == constantPart && kvp.Key.Item1 >= startPoint && kvp.Key.Item1 <= endPoint)
            .ToDictionary();
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