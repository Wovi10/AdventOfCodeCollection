namespace AdventOfCode2023_1.Models.Day12;

public class Spring
{
    public Spring(char stateChar)
    {
        State = stateChar.ToSpringState();
    }

    public SpringState State { get; set; }
}