namespace AdventOfCode2023_1.Shared;

public class Constraints
{
    public static int MinNumberOfMovements { get; set; }
    public static int MaxNumberOfMovements { get; set; }
    
    public static bool IsAboveMin(int value)
        => value >= MinNumberOfMovements;
    
    public static bool IsBelowMax(int value)
        => value < MaxNumberOfMovements;
}