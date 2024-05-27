using System.Collections.Frozen;
using System.Text;
using AdventOfCode2023_1.Models.Day18.Enums;
using AdventOfCode2023_1.Shared;
using AdventOfCode2023_1.Shared.Types;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public class Grid
{
    public Grid(HashSet<Node> nodes, int width, int height)
    {
        Height = height;
        Width = width;
        FrozenNodeDictionary = nodes.ToDictionary(
            n => new Point2D(n.Coordinates.X, n.Coordinates.Y),
            n => n.Type).ToFrozenDictionary();
        NumPoints = FrozenNodeDictionary.Keys.Length;
    }

    public long NumPoints { get; private set; }
    private FrozenDictionary<Point2D, NodeType> FrozenNodeDictionary { get; }
    private int Height { get; }
    private int Width { get; }

    public void DigHole()
    {
        var counter = NumPoints;
        var current = 0;

        // x and y start from 1. The edges are either already added or are not needed
        Parallel.For(0, Height, y =>
        {
            var numPointsInLoop = GetNumPointsInLoop(y, ref current);
            Interlocked.Add(ref counter, numPointsInLoop);
        });

        NumPoints = counter;
    }

    private long GetNumPointsInLoop(int y, ref int current)
    {
        var numPointsInLoop = 0;
        for (var x = 0; x < Width; x++)
        {
            var currenPoint = new Point2D(x, y);
            if (FrozenNodeDictionary.ContainsKey(currenPoint))
                continue;

            var closestEdge = GetClosestEdge(currenPoint);

            var edgesCrossed = closestEdge switch
            {
                Direction.Up => CountEdgesCrossed(Direction.Up, currenPoint.Y, currenPoint.X),
                Direction.Right => CountEdgesCrossed(Direction.Right, currenPoint.X, currenPoint.Y),
                Direction.Down => CountEdgesCrossed(Direction.Down, currenPoint.Y + 1, currenPoint.X),
                Direction.Left => CountEdgesCrossed(Direction.Left, currenPoint.X, currenPoint.Y),
                _ => 0
            };

            if (edgesCrossed.IsInHole())
            {
                numPointsInLoop++;
                (x, numPointsInLoop) = RunUntilWall(numPointsInLoop, currenPoint);
            }
            else
                x = SkipUntilWall(currenPoint);
        }

        Interlocked.Increment(ref current);

        if (current % 1000 != 0)
            return numPointsInLoop;

        SharedMethods.ClearCurrentConsoleLine();
        Console.Write($"Finished {current} parts of {Height}");

        return numPointsInLoop;
    }

    private int SkipUntilWall(Point2D currenPoint)
    {
        var nodeRight = new Point2D(currenPoint.X, currenPoint.Y);
        while (nodeRight.X < Width && !FrozenNodeDictionary.ContainsKey(nodeRight)) 
            nodeRight = new Point2D(nodeRight.X + 1, currenPoint.Y);

        return nodeRight.X;
    }

    private (int x, int numPointsInLoop) RunUntilWall(int numPointsInLoop, Point2D currenPoint)
    {
        var nodeRight = new Point2D(currenPoint.X+1, currenPoint.Y);
        while (nodeRight.X < Width && !FrozenNodeDictionary.ContainsKey(nodeRight))
        {
            numPointsInLoop++;
            nodeRight = new Point2D(nodeRight.X + 1, nodeRight.Y);
        }

        return (nodeRight.X, numPointsInLoop);
    }

    private Direction GetClosestEdge(Point2D point)
    {
        var (x, y) = point;
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

    private static readonly HashSet<NodeType> TypesToSkipYAxis = new() {NodeType.NorthSouth, NodeType.Enclosed};
    private static readonly HashSet<NodeType> TypesToSkipXAxis = new() {NodeType.EastWest, NodeType.Enclosed};

    private static readonly HashSet<NodeType> StartEdgeTypesUp = new()
        {NodeType.EastWest, NodeType.NorthEast, NodeType.NorthWest};

    private static readonly HashSet<NodeType> StartEdgeTypesRight =
        new() {NodeType.NorthSouth, NodeType.NorthEast, NodeType.SouthEast};

    private static readonly HashSet<NodeType> StartEdgeTypesDown =
        new() {NodeType.EastWest, NodeType.SouthEast, NodeType.SouthWest};

    private static readonly HashSet<NodeType> StartEdgeTypesLeft =
        new() {NodeType.NorthSouth, NodeType.NorthWest, NodeType.SouthWest};

    private int CountEdgesCrossed(Direction direction, int startPoint, int constantPart)
    {
        HashSet<NodeType> typesToSkip;
        HashSet<NodeType> startEdgeTypes;

        switch (direction)
        {
            case Direction.Up:
                typesToSkip = TypesToSkipYAxis;
                startEdgeTypes = StartEdgeTypesUp;
                break;
            case Direction.Right:
                typesToSkip = TypesToSkipXAxis;
                startEdgeTypes = StartEdgeTypesRight;
                break;
            case Direction.Down:
                typesToSkip = TypesToSkipYAxis;
                startEdgeTypes = StartEdgeTypesDown;
                break;
            case Direction.Left:
                typesToSkip = TypesToSkipXAxis;
                startEdgeTypes = StartEdgeTypesLeft;
                break;
            default:
                return 0;
        }

        var nodesToCheck = GetNodesToCheck(direction, startPoint, constantPart);

        if (nodesToCheck.Count == 0)
            return 0;

        var edgesCrossed = 0;
        var startOfWall = NodeType.Enclosed;

        foreach (var (_, type) in nodesToCheck)
        {
            if (typesToSkip.Contains(type))
                continue;

            if (startOfWall == NodeType.Enclosed)
            {
                startOfWall = type;

                edgesCrossed++;
                continue;
            }

            if (startEdgeTypes.Contains(type))
            {
                edgesCrossed++;
                continue;
            }

            if (type.WasRunningOnTopOfWall(startOfWall, direction))
                edgesCrossed--;
        }

        return edgesCrossed;
    }

    private Dictionary<Point2D, NodeType> GetNodesToCheck(Direction direction, int startPoint, int constantPart)
    {
        return direction switch
        {
            Direction.Up => FrozenNodeDictionary.Where(kvp =>
                    kvp.Key.X == constantPart && kvp.Key.Y <= startPoint && kvp.Key.Y >= 0)
                .OrderByDescending(kvp => kvp.Key.Y)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Direction.Right => FrozenNodeDictionary.Where(kvp =>
                    kvp.Key.Y == constantPart && kvp.Key.X >= startPoint && kvp.Key.X <= Width)
                .OrderBy(kvp => kvp.Key.X)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Direction.Down => FrozenNodeDictionary.Where(kvp =>
                    kvp.Key.X == constantPart && kvp.Key.Y >= startPoint && kvp.Key.Y <= Height)
                .OrderBy(kvp => kvp.Key.Y)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Direction.Left => FrozenNodeDictionary.Where(kvp =>
                    kvp.Key.Y == constantPart && kvp.Key.X <= startPoint && kvp.Key.X >= 0)
                .OrderByDescending(kvp => kvp.Key.X)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            _ => new Dictionary<Point2D, NodeType>()
        };
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var point = new Point2D(x, y);
                sb.Append(FrozenNodeDictionary.ContainsKey(point)
                    ? Constants.HashTag
                    : Constants.Dot);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}