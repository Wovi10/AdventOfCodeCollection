namespace AOC.Utils;

public class Constraints
{
    public int MinNumberOfMovements { get; set; }
    public int MaxNumberOfMovements { get; set; }

    public bool IsGreaterThanOrEqualToMin(int value)
        => value >= MinNumberOfMovements;

    public bool IsSmallerThanOrEqualToMax(int value)
        => value <= MaxNumberOfMovements;
    
    public bool IsGreaterThanMin(int value)
        => value > MinNumberOfMovements;
    
    public bool IsSmallerThanMax(int value)
        => value < MaxNumberOfMovements;
}