using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day08;

public class Node(string name, string leftNodeName, string rightNodeName)
{
    public readonly string Name = name;
    public readonly string LeftNodeName = leftNodeName;
    public readonly string RightNodeName = rightNodeName;

    public bool IsStart()
        => (Variables.RunningPartOne && Name == "AAA") ||
           (!Variables.RunningPartOne && Name.EndsWith('A'));

    public bool IsEnd()
        => (Variables.RunningPartOne && Name == "ZZZ") ||
           (!Variables.RunningPartOne && Name.EndsWith('Z'));
}