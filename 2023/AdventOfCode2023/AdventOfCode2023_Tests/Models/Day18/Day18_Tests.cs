using System.Numerics;
using AdventOfCode2023_1.Models.Day18;
using AdventOfCode2023_1.Models.Day18.Enums;

namespace AdventOfCode2023_Tests.Models.Day18;

[TestFixture]
[TestOf(typeof(Grid))]
public class Day18_Tests
{
    private readonly List<Node> _nodesList = new();
    private HashSet<Node> _nodesSet = new();
    
    [SetUp]
    public void Setup()
    {
        for (var y = 1; y < 20; y++)
        {
            _nodesList.Add(new Node(0, y));
        }
        
        for (var x = 1; x < 20; x++)
        {
            _nodesList.Add(new Node(x, 0));
        }
        _nodesSet = _nodesList.ToHashSet();
    }

    [Theory]
    public void GetNodesToCheck()
    {
        var grid = new Grid(_nodesSet, 20, 20);
        grid.DigHole();
        var actual = grid.NumPoints;

        Assert.That(actual, Is.EqualTo(119));
    }
}