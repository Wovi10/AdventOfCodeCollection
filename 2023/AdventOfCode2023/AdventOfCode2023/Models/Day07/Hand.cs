namespace AdventOfCode2023_1.Models.Day07;

public class Hand(int bid, bool runningPartOne = true) : IComparable<Hand>
{
    private readonly List<Card> _cards = new();
    private readonly List<Card2> _cards2 = new();
    public int Bid = bid;
    private HandType _type;

    public int Winnings { get; set; }

    public void SetType()
    {
        if ((runningPartOne && _cards.Count != 5) || _cards2.Count != 5)
        {
            throw new Exception();
        }
        
        var distinctCards = _cards.Distinct().ToList();
        var distinctCards2 = _cards2.Distinct().ToList();
        var distinctCardsCount = distinctCards.Count;
        var containsJoker = distinctCards.Contains(Card.J);

        if (!runningPartOne)
        {
            distinctCardsCount = distinctCards2.Count;
            containsJoker = distinctCards2.Contains(Card2.J);
        }
        
        if (runningPartOne && containsJoker) 
            distinctCardsCount -= 1;

        switch (distinctCardsCount)
        {
            case 0:
            case 1:
                _type = HandType.FiveOfAKind;
                return;
            case 2 when _cards.GroupBy(x => x).Any(g => g.Count() == 4):
            case 2 when !runningPartOne && _cards2.GroupBy(x => x).Any(g => g.Count() == 3) && containsJoker:
                _type = HandType.FourOfAKind;
                return;
            case 2:
                _type = HandType.FullHouse;
                return;
            case 3 when _cards.GroupBy(x => x).Any(g => g.Count() == 3):
            case 3 when !runningPartOne && _cards2.GroupBy(x => x).Any(g => g.Count() == 2) && containsJoker:
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

        if (runningPartOne)
        {
            var hand2Cards = hand2._cards;

            for (var i = 0; i < _cards.Count; i++)
            {
                if (_cards[i] > hand2Cards[i])
                {
                    return 1;
                }
                if (_cards[i] < hand2Cards[i])
                {
                    return -1;
                }
            }
            return 0;
        }
        var hand2Cards2 = hand2._cards2;
        
        for (var i = 0; i < _cards2.Count; i++)
        {
            if (_cards2[i] > hand2Cards2[i])
            {
                return 1;
            }
            if (_cards2[i] < hand2Cards2[i])
            {
                return -1;
            }
        }
        return 0;
    }

    public void AddCard(char card)
    {
        if (runningPartOne)
            _cards.Add(CardExtensions.Parse(card));
        else
            _cards2.Add(Card2Extensions.Parse(card));
    }
}