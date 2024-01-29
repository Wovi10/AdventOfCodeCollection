namespace AdventOfCode2023_1.Models.Day07;

public enum HandType
{
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}

public static class HandTypeExtensions
{
    public static bool? IsHigherThan(this HandType type1, HandType type2)
    {
        if ((int) type1 > (int) type2)
        {
            return true;
        }

        if ((int) type1 < (int) type2)
        {
            return false;
        }

        return null;
    }
}