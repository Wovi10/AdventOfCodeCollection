using UtilsCSharp;

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

    public int GetHoleSize() 
        => Grid?.Nodes.Count ?? 0;

    public async Task ExecuteDigPlan()
    {
        var currentX = 0;
        var currentY = 0;
        var nodes = new List<Node> {new() {X = currentX, Y = currentY}};
        foreach (var digInstruction in DigPlan)
        {
            for (var i = 0; i < digInstruction.Distance; i++)
            {
                var newX = GetNewX(currentX, digInstruction.Direction);
                var newY = GetNewY(currentY, digInstruction.Direction);
                var newNode = new Node {X = newX, Y = newY};

                nodes.TryAddNode(newNode);

                SetNewGridExtremes(newX, newY);
                currentX = newX;
                currentY = newY;
            }
        }

        Grid = CreateGrid(nodes);
        Grid.DecideEdgeTypes();
        await Grid.DigHole();
    }

    private static int GetNewX(int currentX, (int, int) digInstructionDirection) 
        => currentX + digInstructionDirection.Item1;

    private static int GetNewY(int currentY, (int, int) digInstructionDirection) 
        => currentY + digInstructionDirection.Item2;

    private void SetNewGridExtremes(int newX, int newY)
    {
        if (newX.IsLessThan(SmallestX) == true)
            SmallestX = newX;
        else if (newX.IsGreaterThan(LargestX) == true)
            LargestX = newX;

        if (newY.IsLessThan(SmallestY) == true)
            SmallestY = newY;
        else if (newY.IsGreaterThan(LargestY) == true)
            LargestY = newY;
    }

    private Grid CreateGrid(List<Node> inputNodes)
    {
        var nodes = new List<Node>();

        foreach (var inputNode in inputNodes)
        {
            var newNode = new Node {X = inputNode.X + Math.Abs(SmallestX), Y = inputNode.Y + Math.Abs(SmallestY)};
            nodes.TryAddNode(newNode);
        }

        var gridHeight = Math.Abs(SmallestY) + Math.Abs(LargestY) + 1;
        var gridWidth = Math.Abs(SmallestX) + Math.Abs(LargestX) + 1;

        return new Grid(nodes, gridWidth, gridHeight);
    }
}