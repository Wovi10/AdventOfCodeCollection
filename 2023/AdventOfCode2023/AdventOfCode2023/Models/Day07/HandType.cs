using UtilsCSharp;

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
        => MathUtils.IsGreaterThan((int)type1, (int)type2);
}