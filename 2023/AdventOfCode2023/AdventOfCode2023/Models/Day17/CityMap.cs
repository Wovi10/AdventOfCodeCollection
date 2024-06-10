using AdventOfCode2023_1.Shared;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;
using UtilsCSharp.Utils;

namespace AdventOfCode2023_1.Models.Day17;

public class CityMap
{
    public CityMap(List<string> rows, Constraints constraints)
    {
        for (var y = 0; y < rows.Count; y++)
        {
            var row = rows[y];
            for (var x = 0; x < row.Length; x++)
            {
                var nodeToAdd = new Node(x, y)
                {
                    HeatLoss = int.Parse(row[x].ToString())
                };
                NodesDictionary.Add((x, y), nodeToAdd);
            }
        }

        Height = rows.Count;
        Width = rows[0].Length;
        Constraints = constraints;
    }

    private Dictionary<(int, int), Node> NodesDictionary { get; } = new();
    private int Height { get; }
    private int Width { get; }
    private readonly PriorityQueue<Node, int> _priorityQueue = new();
    private Constraints Constraints { get; }

    private const int DefaultHeatLoss = 0;

    public int GetMinimalHeatLoss()
    {
        var seen = new HashSet<Node>();
        _priorityQueue.Enqueue(new Node(0, 0), 0);

        while (_priorityQueue.Count > 0)
        {
            var currentNode = _priorityQueue.Dequeue();
            var (column, row) = currentNode;

            var isEndNode = row == Height - 1 && column == Width - 1;
            var crucibleCanStop = Constraints.IsGreaterThanOrEqualToMin(currentNode.TimesInDirection);

            if (isEndNode && crucibleCanStop)
                return currentNode.HeatLoss;

            if (seen.Any(n => n.Equals(currentNode)))
                continue;

            seen.Add(currentNode);
            CheckNeighbours(currentNode);
        }

        return DefaultHeatLoss;
    }

    private void CheckNeighbours(Node currentNode)
    {
        TryStraight(currentNode);
        TryTurning(currentNode);
    }

    private void TryStraight(Node currentNode)
    {
        if (Constraints.IsSmallerThanMax(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            CheckNextNode(currentNode, currentNode.Direction, true);
    }

    private void TryTurning(Node currentNode)
    {
        if (!Constraints.IsGreaterThanOrEqualToMin(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            return;

        var (currentDirectionColumn, currentDirectionRow) = currentNode.Direction.ToOffset();

        foreach (var (nextDirectionColumn, nextDirectionRow) in Offset.Offsets)
        {
            var isStraight = nextDirectionRow == currentDirectionRow &&
                             nextDirectionColumn == currentDirectionColumn;
            var isBack = nextDirectionRow == -currentDirectionRow &&
                         nextDirectionColumn == -currentDirectionColumn;

            if (isStraight || isBack)
                continue;

            CheckNextNode(currentNode, (nextDirectionColumn, nextDirectionRow).ToDirection(), false);
        }
    }

    private void CheckNextNode(Node currentNode, Direction nextDirection, bool isSameDirection)
    {
        var newTimesInDirection = isSameDirection ? currentNode.TimesInDirection + 1 : 1;

        var (column, row) = currentNode;

        var (nextDirectionColumn, nextDirectionRow) = nextDirection.ToOffset();

        var nextRow = row + nextDirectionRow;
        var nextColumn = column + nextDirectionColumn;
        var newNode = new Node(DefaultHeatLoss, nextColumn, nextRow, nextDirectionColumn,
            nextDirectionRow, newTimesInDirection);

        if (!newNode.IsValid(Height, Width))
            return;

        var currentHeatLoss = NodesDictionary.TryGetValue((nextColumn, nextRow), out var node)
            ? node.HeatLoss
            : DefaultHeatLoss;

        var heatLoss = currentHeatLoss + currentNode.HeatLoss;
        newNode.HeatLoss = heatLoss;
        _priorityQueue.Enqueue(newNode, newNode.HeatLoss);
    }
}