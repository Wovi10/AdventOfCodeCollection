using AdventOfCode2023_1.Models.Day08;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day08 : DayBase
{
    private const char Left = 'L';
    private readonly List<bool> _instructions = new();
    private readonly List<Node> _nodes = new();
    protected override void PartOne()
    {
        var result = CalculateSteps();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        var result = 0;
        SharedMethods.AnswerPart(2, result);
    }
    
    private int CalculateSteps()
    {
        ProcessInput();
        var result = StartStepping();
        return result;
    }

    private void ProcessInput()
    {
        _instructions.Clear();
        GetInstructions();
        GetNodes();
    }

    private void GetInstructions()
    {
        var instructionsString = Input.First().Trim();
        foreach (var instruction in instructionsString)
        {
            _instructions.Add(instruction == Left);
        }
    }

    private void GetNodes()
    {
        foreach (var line in Input.Skip(2))
        {
            var nodeName = line.Substring(0, 3);
            var leftNodeName = line.Substring(7, 3);
            var rightNodeName = line.Substring(12, 3);
            var node = new Node(nodeName, leftNodeName, rightNodeName);
            _nodes.Add(node);
        }
    }

    private int StartStepping()
    {
        var currentNode = _nodes.First(node => node.IsStart);
        var endReached = false;
        var counter = 0;
        do
        {
            foreach (var isLeft in _instructions)
            {
                counter++;
                var nextNode = isLeft 
                    ? _nodes.First(node => node.Name == currentNode.LeftNodeName) 
                    : _nodes.First(node => node.Name == currentNode.RightNodeName);
                if (nextNode.IsEnd)
                {
                    endReached = true;
                    break;
                }
                currentNode = nextNode;
            }
        }while(!endReached);

        return counter;
    }
}