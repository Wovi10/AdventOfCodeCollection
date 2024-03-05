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
        var nextOperationalIndex = springs.FindIndex(currentIndex, spring => spring.IsOperational());
        var hasEnoughSpace = nextOperationalIndex - currentIndex >= lengthToCheck;

        if (!hasEnoughSpace)
            return false;

        var previousWasDamaged = springs.ElementAtOrDefault(currentIndex - 1)?.IsDamaged() ?? false;

        if (previousWasDamaged)
            return false;

        var lastSpringInLength = springs.ElementAtOrDefault(currentIndex + lengthToCheck - 1);
        var lastSpringInLengthIsDamaged = lastSpringInLength?.IsDamaged() ?? false;

        return !lastSpringInLengthIsDamaged;
    }
    
    public static bool IsPossible(Spring? previousSpring, Spring currentSpring, Spring lastInLength)
    {
        var previousIsDamaged = previousSpring?.IsDamaged() ?? false;
        var currentIsOperational = currentSpring.IsOperational();
        var lastInLengthIsDamaged = lastInLength.IsDamaged();

        return !previousIsDamaged && !currentIsOperational && !lastInLengthIsDamaged;
    }

    public bool IsDamaged()
    {
        return State == SpringState.Damaged;
    }
}