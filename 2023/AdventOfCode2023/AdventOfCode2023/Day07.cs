using AdventOfCode2023_1.Models.Day07;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day07 : DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("07");
    private List<Hand> _hands = new();
    protected override void PartOne()
    {
        var result = GetTotalWinnings();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        throw new NotImplementedException();
    }

    private int GetTotalWinnings()
    {
        ProcessInput();
        CalculateWinnings();
        return _hands.Sum(x => x.Winnings);
    }

    private void ProcessInput()
    {
        foreach (var line in Input)
        {
            var hand = new Hand(0);
            var lineParts = line.Split(Constants.Space).ToList();
            var cards = lineParts.First();
            hand.Bid = int.Parse(lineParts.Last());

            foreach (var card in cards)
            {
                hand.AddCard(CardExtensions.Parse(card));
            }
            hand.SetType();
            _hands.Add(hand);
        }
    }

    private void CalculateWinnings()
    {
        OrderHands();
        for (var i = 0; i < _hands.Count; i++) 
            _hands[i].Winnings = _hands[i].Bid * (i + 1);
    }

    private void OrderHands()
    {
        _hands = _hands.Order().ToList();
    }
}