using AdventOfCode2023_1.Models.Day16;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public class ExcavationSite
{
    public ExcavationSite(List<string> input)
    {
        DigPlan = input.Select(x => new DigInstruction(x)).ToList();
    }

    public List<DigInstruction> DigPlan { get; set; }
    public long HoleSize { get; set; }
    public int SmallestX { get; set; } = 0;
    public int SmallestY { get; set; } = 0;
    public int LargestX { get; set; } = 0;
    public int LargestY { get; set; } = 0;
    public Grid Grid { get; set; }

    public void ExecuteDigPlan()
    {
        var currentX = 0;
        var currentY = 0;
        var nodes = new List<Node> {new() {X = currentX, Y = currentY}};
        foreach (var digInstruction in DigPlan)
        {
            for (int i = 0; i < digInstruction.Distance; i++)
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

        var grid = CreateGrid(nodes);
        grid.DecideEdgeTypes();
        grid.DigHole();
        Console.WriteLine(grid.ToString());
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