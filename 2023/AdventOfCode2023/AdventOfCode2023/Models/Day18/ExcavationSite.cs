using System.Collections.Concurrent;
using AdventOfCode2023_1.Models.Day18.Enums;
using AdventOfCode2023_1.Shared.Types;

namespace AdventOfCode2023_1.Models.Day18;

public class ExcavationSite
{
    public ExcavationSite(List<string> input)
    {
        DigPlan = input.Select(x => new DigInstruction(x)).ToList();
    }

    private List<DigInstruction> DigPlan { get; }
    private int SmallestX { get; set; }
    private int SmallestY { get; set; }
    private int LargestX { get; set; }
    private int LargestY { get; set; }
    private Grid? Grid { get; set; }

    public long GetHoleSize()
        => Grid?.NumPoints ?? 0;

    public void CalculateDigPlan()
    {
        var nodes = new List<Node>();
        var currentX = 0;
        var currentY = 0;
        foreach (var digInstruction in DigPlan)
        {
            for (var i = 0; i < digInstruction.Distance; i++)
            {
                var newX = GetNewX(currentX, digInstruction.Direction);
                var newY = GetNewY(currentY, digInstruction.Direction);
                var newNode = new Node(newX, newY);

                nodes.Add(newNode);

                currentX = newX;
                currentY = newY;
            }
        }

        Grid = CreateGrid(nodes);
    }

    public void DigHole()
        => Grid?.DigHole();

    private void SetNewGridExtremes(List<Node> nodes)
    {
        var nodesList = nodes.OrderBy(x => x.Coordinates.X).ThenBy(x => x.Coordinates.Y).Distinct().ToList();
        SmallestX = nodesList.Min(x => x.Coordinates.X);
        LargestX = nodesList.Max(x => x.Coordinates.X);
        SmallestY = nodesList.Min(x => x.Coordinates.Y);
        LargestY = nodesList.Max(x => x.Coordinates.Y);
    }

    private static int GetNewX(int currentX, (int, int) digInstructionDirection)
        => currentX + digInstructionDirection.Item1;

    private static int GetNewY(int currentY, (int, int) digInstructionDirection)
        => currentY + digInstructionDirection.Item2;

    private Grid CreateGrid(List<Node> inputNodes)
    {
        SetNewGridExtremes(inputNodes);
        inputNodes = inputNodes.GroupBy(elem => elem.Coordinates).Select(group => group.First()).ToList();
        var nodes = new ConcurrentBag<Node>();

        Parallel.ForEach(inputNodes, node =>
        {
            var (inputX, inputY) = node.Coordinates;
            var newNode = new Node(inputX + Math.Abs(SmallestX), inputY + Math.Abs(SmallestY));
            nodes.Add(newNode);
        });

        var orderedNodes = nodes.OrderBy(node => node.Coordinates.Y).ThenBy(node => node.Coordinates.X).ToHashSet();

        var gridHeight = Math.Abs(SmallestY) + Math.Abs(LargestY) + 1;
        var gridWidth = Math.Abs(SmallestX) + Math.Abs(LargestX) + 1;

        DecideEdgeTypes(orderedNodes);

        return new Grid(orderedNodes, gridWidth, gridHeight);
    }

    private static void DecideEdgeTypes(HashSet<Node> nodes)
    {
        var points = nodes.Select(x => x.Coordinates).ToHashSet();

        Parallel.ForEach(nodes, node =>
        {
            var neighbours = GetNeighbours(node.Coordinates, points);
            node.DecideType(neighbours);
        });
    }

    private static readonly List<Direction> AllDirections = new()
        {Direction.Up, Direction.Right, Direction.Down, Direction.Left};

    private static readonly List<(int, int)> OffSets = new() {(0, -1), (1, 0), (0, 1), (-1, 0)};

    private static HashSet<Direction> GetNeighbours(Point2D point, HashSet<Point2D> points)
    {
        var neighbours = new HashSet<Direction>();

        for (var i = 0; i < OffSets.Count; i++)
        {
            var (offSetX, offSetY) = OffSets[i];
            var (x, y) = point;
            var neighbourPosition = new Point2D(x + offSetX, y + offSetY);
            if (points.Contains(neighbourPosition))
                neighbours.Add(AllDirections[i]);
        }

        return neighbours;
    }
}