using _2023.Models.Day07.Enums;
using UtilsCSharp;

namespace _2023.Models.Day07;

public static class Day07Extensions
{
    public static Card ToCard(this char card)
    {
        return card switch
        {
            '2' => Card.Two,
            '3' => Card.Three,
            '4' => Card.Four,
            '5' => Card.Five,
            '6' => Card.Six,
            '7' => Card.Seven,
            '8' => Card.Eight,
            '9' => Card.Nine,
            'T' => Card.T,
            'J' => Card.J,
            'Q' => Card.Q,
            'K' => Card.K,
            'A' => Card.A,
            _ => throw new Exception()
        };
    }

    public static bool IsHigherThan(this Card card1, Card card2)
        => ((int)card1).IsGreaterThan((int)card2);
    
    public static bool IsLowerThan(this Card card1, Card card2)
        => ((int)card1).IsLessThan((int)card2);
}