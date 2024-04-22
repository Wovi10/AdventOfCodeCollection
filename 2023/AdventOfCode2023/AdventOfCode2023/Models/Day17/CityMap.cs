using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day17;

public class CityMap
{
    public CityMap(List<string> rows, Constraints constraints)
    {
        Rows = new List<List<int>>();

        foreach (var rowBlocks in rows.Select(row => row.Select(num => int.Parse(num.ToString())).ToList()))
            Rows.Add(rowBlocks);

        Rows[0][0] = 0;
        Height = Rows.Count;
        Width = Rows[0].Count;
        Constraints = constraints;
    }

    private List<List<int>> Rows { get; }
    private int Height { get; }
    private int Width { get; }
    private readonly PriorityQueue<Node, int> _priorityQueue = new();
    private Constraints Constraints { get; set; }

    private readonly List<(int, int)> _directions = new()
    {
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0)
    };

    public int GetMinimalHeatLoss()
    {
        var seen = new List<Node>();
        _priorityQueue.Enqueue(new Node(), 0);

        while (_priorityQueue.Count > 0)
        {
            var currentNode = _priorityQueue.Dequeue();

            var isEndNode = currentNode.Row == Height - 1 && currentNode.Column == Width - 1;
            var crucibleCanStop = Constraints.IsGreaterThanOrEqualToMin(currentNode.TimesInDirection);

            if (isEndNode && crucibleCanStop)
                return currentNode.HeatLoss;

            if (seen.Any(n => n.Equals(currentNode)))
                continue;

            seen.Add(currentNode);
            CheckNeighbours(currentNode);
        }

        return 0;
    }

    private void CheckNeighbours(Node currentNode)
    {
        TryStraight(currentNode);
        TryTurning(currentNode);
    }

    private void TryStraight(Node currentNode)
    {
        if (Constraints.IsSmallerThanMax(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            CheckNextNode(currentNode, currentNode.DirectionRow, currentNode.DirectionColumn, true);
    }

    private void TryTurning(Node currentNode)
    {
        if (!Constraints.IsGreaterThanOrEqualToMin(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            return;

        foreach (var (nextDirectionRow, nextDirectionColumn) in _directions)
        {
            var isStraight = nextDirectionRow == currentNode.DirectionRow &&
                          nextDirectionColumn == currentNode.DirectionColumn;
            var isBack = nextDirectionRow == -currentNode.DirectionRow &&
                         nextDirectionColumn == -currentNode.DirectionColumn;

            if (isStraight || isBack)
                continue;

            CheckNextNode(currentNode, nextDirectionRow, nextDirectionColumn, false);
        }
    }

    private void CheckNextNode(Node currentNode, int nextDirectionRow, int nextDirectionColumn, bool isSameDirection)
    {
        var newTimesInDirection = isSameDirection ? currentNode.TimesInDirection + 1 : 1;

        var nextRow = currentNode.Row + nextDirectionRow;
        var nextColumn = currentNode.Column + nextDirectionColumn;
        var newNode = new Node(0, nextRow, nextColumn, nextDirectionRow,
            nextDirectionColumn, newTimesInDirection);

        if (!newNode.IsValid(Height, Width))
            return;

        var heatLoss = Rows[nextRow][nextColumn] + currentNode.HeatLoss;
        newNode.HeatLoss = heatLoss;
        _priorityQueue.Enqueue(newNode, newNode.HeatLoss);
    }
}