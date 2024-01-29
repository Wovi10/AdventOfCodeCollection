namespace AdventOfCode2023_1.Models.Day07;

public class Hand : IComparable<Hand>
{
    public readonly List<Card> Cards = new();
    public int Bid;
    public HandType Type;

    public Hand(int bid)
    {
        Bid = bid;
    }

    public int Winnings { get; set; }

    public void AddCard(Card card)
    {
        if (Cards.Count > 5)
        {
            throw new Exception();
        }
        Cards.Add(card);
    }
    
    public void SetType()
    {
        if (Cards.Count != 5)
        {
            throw new Exception();
        }
        
        var distinctCards = Cards.Distinct().Count();

        switch (distinctCards)
        {
            case 1:
                Type = HandType.FiveOfAKind;
                return;
            case 2 when Cards.GroupBy(x => x).Any(g => g.Count() == 4):
                Type = HandType.FourOfAKind;
                return;
            case 2:
                Type = HandType.FullHouse;
                return;
            case 3 when Cards.GroupBy(x => x).Any(g => g.Count() == 3):
                Type = HandType.ThreeOfAKind;
                return;
            case 3:
                Type = HandType.TwoPairs;
                return;
            case 4:
                Type = HandType.OnePair;
                return;
            default:
                Type = HandType.HighCard;
                break;
        }
    }

    public int CompareTo(Hand? hand2)
    {
        if (hand2 == null)
        {
            return 1;
        }
        if (Type.IsHigherThan(hand2.Type) == true)
        {
            return 1;
        }
        if (Type.IsHigherThan(hand2.Type) == false)
        {
            return -1;
        }

        var cards2 = hand2.Cards;

        for (var i = 0; i < Cards.Count; i++)
        {
            if (Cards[i] > cards2[i])
            {
                return 1;
            }
            if (Cards[i] < cards2[i])
            {
                return -1;
            }
        }

        return 0;
    }
}