namespace AdventOfCode2023_1.Models.Day08;

public class Node
{
    public Node(string name, string leftNodeName, string rightNodeName, bool runningPartOne)
    {
        Name = name;
        LeftNodeName = leftNodeName;
        RightNodeName = rightNodeName;
        RunningPartOne = runningPartOne;
    }

    public readonly bool RunningPartOne;

    public readonly string Name;
    public readonly string LeftNodeName;
    public readonly string RightNodeName;

    public bool IsStart()
    {
        return (!RunningPartOne && Name.EndsWith('A')) || (RunningPartOne && Name == "AAA");
    }

    public bool IsEnd()
    {
        return (!RunningPartOne && Name.EndsWith('Z')) || (RunningPartOne && Name == "ZZZ");
    }
}