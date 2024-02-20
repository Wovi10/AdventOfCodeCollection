namespace AdventOfCode2023_1.Models.Day12;

public class Spring
{
    public Spring(char stateChar)
    {
        State = stateChar.ToSpringState();
    }

    public SpringState State { get; set; }
    
    public static implicit operator bool(Spring spring)
    {
        return spring.State == SpringState.Operational;
    }

    public bool IsOperational()
    {
        return State == SpringState.Operational;
    }

    public bool IsPossible(int lengthToCheck, List<Spring> springs, int currentIndex)
    {
        if (IsOperational())
            return false;

        lengthToCheck--;

        if (lengthToCheck == 0)
            return true;

        return springs[currentIndex].IsPossible(lengthToCheck, springs, currentIndex + 1);
    }
}