namespace AdventOfCode2023_1.Models.Day07;

public class Hand(int bid) : IComparable<Hand>
{
    private readonly List<Card> _cards = new();
    public int Bid = bid;
    private HandType _type;

    public int Winnings { get; set; }

    public void AddCard(Card card)
    {
        if (_cards.Count > 5)
        {
            throw new Exception();
        }
        _cards.Add(card);
    }

    public void SetType()
    {
        if (_cards.Count != 5)
        {
            throw new Exception();
        }
        
        var distinctCards = _cards.Distinct().Count();

        switch (distinctCards)
        {
            case 1:
                _type = HandType.FiveOfAKind;
                return;
            case 2 when _cards.GroupBy(x => x).Any(g => g.Count() == 4):
                _type = HandType.FourOfAKind;
                return;
            case 2:
                _type = HandType.FullHouse;
                return;
            case 3 when _cards.GroupBy(x => x).Any(g => g.Count() == 3):
                _type = HandType.ThreeOfAKind;
                return;
            case 3:
                _type = HandType.TwoPairs;
                return;
            case 4:
                _type = HandType.OnePair;
                return;
            default:
                _type = HandType.HighCard;
                break;
        }
    }

    public int CompareTo(Hand? hand2)
    {
        if (hand2 == null)
        {
            return 1;
        }
        if (_type.IsHigherThan(hand2._type) == true)
        {
            return 1;
        }
        if (_type.IsHigherThan(hand2._type) == false)
        {
            return -1;
        }

        var cards2 = hand2._cards;

        for (var i = 0; i < _cards.Count; i++)
        {
            if (_cards[i] > cards2[i])
            {
                return 1;
            }
            if (_cards[i] < cards2[i])
            {
                return -1;
            }
        }

        return 0;
    }
}