namespace AdventOfCode2023_1.Models.Day12;

public class Spring
{
    public Spring(char stateChar)
    {
        State = stateChar.ToSpringState();
    }

    private SpringState State { get; }
    
    public static implicit operator bool(Spring spring)
    {
        return spring.State == SpringState.Operational;
    }

    private bool IsOperational()
    {
        return State == SpringState.Operational;
    }

    public static bool IsPossible(int lengthToCheck, List<Spring> springs, int currentIndex)
    {
        if (currentIndex + lengthToCheck > springs.Count)
            return false;

        var currentSpring = springs.ElementAtOrDefault(currentIndex);

        if (currentSpring == null || currentSpring.IsOperational())
            return false;

        lengthToCheck--;

        return lengthToCheck == 0 || IsPossible(lengthToCheck, springs, currentIndex + 1);
    }

    public bool IsDamaged()
    {
        return State == SpringState.Damaged;
    }
}