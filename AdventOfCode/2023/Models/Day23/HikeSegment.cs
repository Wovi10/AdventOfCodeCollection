namespace _2023.Models.Day23;

public class HikeSegment
{
    public (int, int) CornerOne { get; set; }
    public (int, int) CornerTwo { get; set; }
    public int Length { get; set; }
    
    public void SetCornerTwo((int, int) cornerTwo)
    {
        if ((CornerOne.Item1 > cornerTwo.Item1) || (CornerOne.Item1 == cornerTwo.Item1 && CornerOne.Item2 > cornerTwo.Item2))
        {
            (CornerOne, CornerTwo) = (cornerTwo, CornerOne);
            return;
        }
        
        CornerTwo = cornerTwo;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not HikeSegment other)
            return false;

        return CornerOne == other.CornerOne && CornerTwo == other.CornerTwo && Length == other.Length;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CornerOne, CornerTwo, Length);
    }
}