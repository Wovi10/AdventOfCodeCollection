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
    private readonly PriorityQueue<Node<int>, int> _priorityQueue = new();
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
        var seen = new List<Node<int>>();
        _priorityQueue.Enqueue(new Node<int>(0,0), 0);

        while (_priorityQueue.Count > 0)
        {
            var currentNode = _priorityQueue.Dequeue();
            var (row, column) = currentNode.Coordinates;

            var isEndNode = row == Height - 1 && column == Width - 1;
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

    private void CheckNeighbours(Node<int> currentNode)
    {
        TryStraight(currentNode);
        TryTurning(currentNode);
    }

    private void TryStraight(Node<int> currentNode)
    {
        if (Constraints.IsSmallerThanMax(currentNode.TimesInDirection) && !currentNode.IsStandingStill())
            CheckNextNode(currentNode, currentNode.DirectionRow, currentNode.DirectionColumn, true);
    }

    private void TryTurning(Node<int> currentNode)
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

    private void CheckNextNode(Node<int> currentNode, int nextDirectionRow, int nextDirectionColumn, bool isSameDirection)
    {
        var newTimesInDirection = isSameDirection ? currentNode.TimesInDirection + 1 : 1;

        var (row, column) = currentNode.Coordinates;

        var nextRow = row + nextDirectionRow;
        var nextColumn = column + nextDirectionColumn;
        var newNode = new Node<int>(0, nextRow, nextColumn, nextDirectionRow,
            nextDirectionColumn, newTimesInDirection);

        if (!newNode.IsValid(Height, Width))
            return;

        var heatLoss = Rows[nextRow][nextColumn] + currentNode.HeatLoss;
        newNode.HeatLoss = heatLoss;
        _priorityQueue.Enqueue(newNode, newNode.HeatLoss);
    }
}