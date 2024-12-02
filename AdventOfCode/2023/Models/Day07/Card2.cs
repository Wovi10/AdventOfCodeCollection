using UtilsCSharp;

namespace _2023.Models.Day07;

public enum Card2
{
    J,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    T,
    Q,
    K,
    A
}

public static class Card2Extensions
{
    public static Card2 Parse(char card)
    {
        return card switch
        {
            'J' => Card2.J,
            '2' => Card2.Two,
            '3' => Card2.Three,
            '4' => Card2.Four,
            '5' => Card2.Five,
            '6' => Card2.Six,
            '7' => Card2.Seven,
            '8' => Card2.Eight,
            '9' => Card2.Nine,
            'T' => Card2.T,
            'Q' => Card2.Q,
            'K' => Card2.K,
            'A' => Card2.A,
            _ => throw new Exception()
        };
    }

    public static bool IsHigherThan(this Card2 card1, Card2 card2)
        => ((int)card1).IsGreaterThan((int)card2);
    
    public static bool IsLowerThan(this Card2 card1, Card2 card2)
        => ((int)card1).IsLessThan((int)card2);
}