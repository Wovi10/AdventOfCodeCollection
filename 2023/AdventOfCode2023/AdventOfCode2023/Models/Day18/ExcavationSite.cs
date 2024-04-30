using System.Collections.Concurrent;
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
        var nodes = new ConcurrentDictionary<(int, int), Node>();
        nodes.TryAdd((currentX, currentY), new Node {X = currentX, Y = currentY});
        var tasks = new List<Task>();

        foreach (var digInstruction in DigPlan)
        {
            tasks.Add(Task.Run(() =>
            {
                for (var i = 0; i < digInstruction.Distance; i++)
                {
                    var newX = GetNewX(currentX, digInstruction.Direction);
                    var newY = GetNewY(currentY, digInstruction.Direction);
                    var newNode = new Node {X = newX, Y = newY};

                    nodes.TryAdd((newX, newY), newNode);

                    currentX = newX;
                    currentY = newY;
                }
            }));
        }

        await Task.WhenAll(tasks);

        var orderedNodes = nodes.Values.OrderBy(x => x.X).ThenBy(x => x.Y).Distinct().ToList();
        SetNewGridExtremes(orderedNodes);

        Grid = CreateGrid(orderedNodes);
        Grid.DecideEdgeTypes();
        Console.WriteLine("Starting Dighole");
        Grid.DigHole();
    }

    private void SetNewGridExtremes(List<Node> nodes)
    {
        SmallestX = nodes.Min(x => x.X);
        LargestX = nodes.Max(x => x.X);
        SmallestY = nodes.Min(x => x.Y);
        LargestY = nodes.Max(x => x.Y);
    }

    private static int GetNewX(int currentX, (int, int) digInstructionDirection) 
        => currentX + digInstructionDirection.Item1;

    private static int GetNewY(int currentY, (int, int) digInstructionDirection) 
        => currentY + digInstructionDirection.Item2;

    private Grid CreateGrid(List<Node> inputNodes)
    {
        var nodes = new ConcurrentBag<Node>();

        Parallel.ForEach(inputNodes, inputNode =>
        {
            var newNode = new Node {X = inputNode.X + Math.Abs(SmallestX), Y = inputNode.Y + Math.Abs(SmallestY)};
            nodes.Add(newNode);
        });

        var orderedNodes = nodes.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
        
        var gridHeight = Math.Abs(SmallestY) + Math.Abs(LargestY) + 1;
        var gridWidth = Math.Abs(SmallestX) + Math.Abs(LargestX) + 1;

        return new Grid(orderedNodes, gridWidth, gridHeight);
    }
}