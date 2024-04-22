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
        foreach (var digInstruction in DigPlan)
        {
            var newX = GetNewX(currentX, digInstruction.Direction, digInstruction.Distance);
            var newY = GetNewY(currentY, digInstruction.Direction, digInstruction.Distance);

            SetNewGridExtremes(newX, newY);
            currentX = newX;
            currentY = newY;
        }

        var grid = CreateGrid();
        foreach (var digInstruction in DigPlan)
        {
            
        }
    }

    private static int GetNewX(int currentX, (int, int) digInstructionDirection, int digInstructionDistance) 
        => currentX + digInstructionDirection.Item1 * digInstructionDistance;

    private static int GetNewY(int currentY, (int, int) digInstructionDirection, int digInstructionDistance) 
        => currentY + digInstructionDirection.Item2 * digInstructionDistance;

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

    private Grid CreateGrid()
    {
        var nodes = new List<Node>();
        var gridHeight = Math.Abs(SmallestY) + Math.Abs(LargestY) + 1;
        var gridWidth = Math.Abs(SmallestX) + Math.Abs(LargestX) + 1;
        for (var i = 0; i < gridWidth; i++)
        {
            for (var j = 0; j < gridHeight; j++)
            {
                nodes.Add(new Node {X = i, Y = j});
            }
        }

        return new Grid(nodes, gridWidth, gridHeight);
    }
}