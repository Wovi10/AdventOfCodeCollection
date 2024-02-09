using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day07;

public enum Card
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    T,
    J,
    Q,
    K,
    A
}

public static class CardExtensions
{
    public static Card Parse(char card)
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

    public static bool? IsHigherThan(this Card card1, Card card2)
        => MathUtils.IsGreaterThan((int)card1, (int)card2);
}