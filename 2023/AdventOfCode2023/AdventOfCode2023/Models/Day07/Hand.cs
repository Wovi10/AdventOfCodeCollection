using AdventOfCode2023_1.Models.Day07.Enums;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day07;

public class Hand(int bid) : IComparable<Hand>
{
    private readonly List<Card> _cards = new();
    private readonly List<Card2> _cards2 = new();
    public int Bid = bid;
    private HandType _type;
    private bool _containsJoker;

    public int Winnings { get; set; }

    public void SetType()
    {
        if (Variables.RunningPartOne)
            _containsJoker = false;

        var distinctCards = _cards.Distinct().ToList();
        var distinctCardGroups = _cards.GroupBy(c => c).ToList();

        var distinctCards2 = _cards2.Distinct().ToList();
        var distinctCardGroups2 = _cards2.GroupBy(c => c).ToList();
        var distinctCardsCount = Variables.RunningPartOne
            ? distinctCards.Count
            : distinctCards2.Count;
        var numJokers = _containsJoker ? _cards2.Count(c => c == Card2.J) : 0;

        switch (distinctCardsCount)
        {
            case 1: // Five of the same
            case 2 when _containsJoker: // Two types of cards of which one is a Joker
                _type = HandType.FiveOfAKind;
                return;
            case 2
                when Variables.RunningPartOne && distinctCardGroups.Any(g => g.Count() == 4)
                : // Four the same and a different one
            case 2
                when !Variables.RunningPartOne && distinctCardGroups2.Any(g => g.Count() == 4)
                : // Four the same and a different one
            case 3
                when _containsJoker && distinctCardGroups2.Any(g => g.Count() == 3)
                : // Three the same, Joker as fourth and a different one
            case 3
                when _containsJoker && distinctCardGroups2.Any(g => g.Count() == 2) && numJokers == 2
                : // Three the same, Joker as fourth and a different one
                _type = HandType.FourOfAKind;
                return;
            case 2
                when Variables.RunningPartOne && distinctCardGroups.Any(g => g.Count() == 3)
                : // Three the same and two the same
            case 2
                when !Variables.RunningPartOne && distinctCardGroups2.Any(g => g.Count() == 3)
                : // Three the same and two the same
            case 3 when _containsJoker: // Two the same, Joker as third and two different ones
                _type = HandType.FullHouse;
                return;
            case 3
                when Variables.RunningPartOne && distinctCardGroups.Any(g => g.Count() == 3)
                : // Three the same and two different ones
            case 3
                when !Variables.RunningPartOne && distinctCardGroups2.Any(g => g.Count() == 3)
                : // Three the same and two different ones
            case 4 when _containsJoker: // Two the same, Joker as third and two different ones
                _type = HandType.ThreeOfAKind;
                return;
            case 3: // Three different cards, no Joker
                _type = HandType.TwoPairs;
                return;
            case 4: // Four different cards, no Joker
            case 5 when _containsJoker: // Five different cards, of which one is a Joker
                _type = HandType.OnePair;
                return;
            case 5: // Five different cards, no Joker
                _type = HandType.HighCard;
                break;
        }
    }

    public int CompareTo(Hand? hand2)
    {
        if (hand2 == null)
            return 1;

        if (_type.IsHigherThan(hand2._type) == true)
            return 1;

        if (_type.IsHigherThan(hand2._type) == false)
            return -1;

        if (Variables.RunningPartOne)
        {
            var cardsCount = _cards.Count;
            var hand2Cards = hand2._cards;
            for (var i = 0; i < cardsCount; i++)
            {
                if (_cards[i].IsHigherThan(hand2Cards[i]) == true)
                    return 1;
                if (_cards[i].IsHigherThan(hand2Cards[i]) == false)
                    return -1;
            }

            return 0;
        }

        var cards2Count = _cards2.Count;
        var hand2Cards2 = hand2._cards2;

        for (var i = 0; i < cards2Count; i++)
        {
            if (_cards2[i].IsHigherThan(hand2Cards2[i]) == true)
                return 1;

            if (_cards2[i].IsHigherThan(hand2Cards2[i]) == false)
                return -1;
        }

        return 0;
    }

    public void AddCard(char card)
    {
        if (Variables.RunningPartOne)
        {
            _cards.Add(card.ToCard());
            return;
        }

        var parsedCard = Card2Extensions.Parse(card);
        _containsJoker = _containsJoker || parsedCard == Card2.J;
        _cards2.Add(parsedCard);
    }
}