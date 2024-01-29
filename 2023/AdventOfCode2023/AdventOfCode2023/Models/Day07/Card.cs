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

    public static char Print(this Card card)
    {
        return card switch
        {
            Card.Two => '2',
            Card.Three => '3',
            Card.Four => '4',
            Card.Five => '5',
            Card.Six => '6',
            Card.Seven => '7',
            Card.Eight => '8',
            Card.Nine => '9',
            Card.T => 'T',
            Card.J => 'J',
            Card.Q => 'Q',
            Card.K => 'K',
            Card.A => 'A',
            _ => throw new Exception()
        };
    }

    public static bool? IsHigherThan(this Card card1, Card card2)
    {
        if ((int) card1 > (int) card2)
        {
            return true;
        }

        if ((int) card1 < (int) card2)
        {
            return false;
        }

        return null;
    }
}