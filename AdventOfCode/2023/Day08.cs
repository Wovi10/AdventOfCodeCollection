﻿using _2023.Models.Day08;
using AOC.Utils;
using UtilsCSharp;

namespace _2023;

public class Day08() : DayBase("08", "Haunted Wasteland")
{
    private const char Left = 'L';
    private readonly List<bool> _instructions = new();
    private readonly List<Node> _nodes = new();

    protected override Task<object> PartOne()
    {
        var result = CalculateSteps();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        Input = SharedMethods.GetInput(Day);
        var result = CalculateSteps();

        return Task.FromResult<object>(result);
    }

    private long CalculateSteps()
    {
        ProcessInput();
        var result = Variables.RunningPartOne ? StartStepping() : StartSteppingPart2();
        return result;
    }

    private void ProcessInput()
    {
        _instructions.Clear();
        _nodes.Clear();
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

    #region Part 1

    private int StartStepping()
    {
        var currentNode = _nodes.First(node => node.IsStart());
        return CalculateNumberOfSteps(currentNode);
    }

    #endregion

    #region Part 2

    private long StartSteppingPart2()
    {
        var startingNodes = _nodes.Where(node => node.IsStart()).ToList();
        return startingNodes
            .Select(CalculateNumberOfSteps)
            .Aggregate(1L, (current, counter) => MathUtils.Lcm(current, counter));
    }

    private int CalculateNumberOfSteps(Node startingNode)
    {
        var currentNode = startingNode;
        var counter = 0;
        while (!currentNode.IsEnd())
        {
            var isLeft = _instructions[counter++ % _instructions.Count];
            currentNode = isLeft
                ? _nodes.First(node => currentNode.LeftNodeName == node.Name)
                : _nodes.First(node => currentNode.RightNodeName == node.Name);
        }

        return counter;
    }

    #endregion
}