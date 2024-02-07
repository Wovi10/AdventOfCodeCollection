using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day08;

public class Node
{
    public Node(string name, string leftNodeName, string rightNodeName)
    {
        Name = name;
        LeftNodeName = leftNodeName;
        RightNodeName = rightNodeName;
    }

    public readonly string Name;
    public readonly string LeftNodeName;
    public readonly string RightNodeName;

    public bool IsStart()
    {
        return (!Variables.RunningPartOne && Name.EndsWith('A')) || (Variables.RunningPartOne && Name == "AAA");
    }

    public bool IsEnd()
    {
        return (!Variables.RunningPartOne && Name.EndsWith('Z')) || (Variables.RunningPartOne && Name == "ZZZ");
    }
}