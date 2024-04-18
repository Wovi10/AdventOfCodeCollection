using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day17;

public class CityMap
{
    public CityMap(List<string> rows)
    {
        Rows = new List<List<int>>();

        foreach (var rowBlocks in rows.Select(row => row.Select(num => int.Parse(num.ToString())).ToList()))
            Rows.Add(rowBlocks);

        Rows[0][0] = 0;
        Height = Rows.Count;
        Width = Rows[0].Count;
    }

    private List<List<int>> Rows { get; }
    private int Height { get; }
    private int Width { get; }
    private readonly PriorityQueue<Node, int> _priorityQueue = new();

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
            var crucibleCanStop = Constraints.IsAboveMin(currentNode.TimesInDirection);

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
        if (Constraints.IsBelowMax(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            CheckNextNode(currentNode, currentNode.DirectionRow, currentNode.DirectionColumn, true);
    }

    private void TryTurning(Node currentNode)
    {
        if (!Constraints.IsBelowMax(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            return;

        foreach (var (nextDirectionRow, nextDirectionColumn) in _directions)
        {
            var isAhead = nextDirectionRow == currentNode.DirectionRow &&
                          nextDirectionColumn == currentNode.DirectionColumn;
            var isBack = nextDirectionRow == -currentNode.DirectionRow &&
                         nextDirectionColumn == -currentNode.DirectionColumn;

            if (isAhead || isBack)
                continue;

            CheckNextNode(currentNode, nextDirectionRow, nextDirectionColumn, false);
        }
    }

    private void CheckNextNode(Node currentNode, int directionRow, int directionColumn, bool isSameDirection)
    {
        var newTimesInDirection = isSameDirection ? currentNode.TimesInDirection + 1 : 1;

        var nextRow = currentNode.Row + directionRow;
        var nextColumn = currentNode.Column + directionColumn;
        var newNode = new Node(0, nextRow, nextColumn, directionRow,
            directionColumn, newTimesInDirection);

        if (!newNode.IsValid(Height, Width))
            return;

        var heatLoss = Rows[nextRow][nextColumn] + currentNode.HeatLoss;
        newNode.HeatLoss = heatLoss;
        _priorityQueue.Enqueue(newNode, newNode.HeatLoss);
    }
}