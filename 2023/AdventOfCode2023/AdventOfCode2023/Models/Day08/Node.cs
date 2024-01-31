namespace AdventOfCode2023_1.Models.Day08;

public class Node
{
    public Node(string name, string leftNodeName, string rightNodeName)
    {
        Name = name;
        LeftNodeName = leftNodeName;
        RightNodeName = rightNodeName;
        IsStart = name == "AAA";
        IsEnd = name == "ZZZ";
    }

    public readonly string Name;
    public readonly string LeftNodeName;
    public readonly string RightNodeName;
    public readonly bool IsStart;
    public readonly bool IsEnd;
}