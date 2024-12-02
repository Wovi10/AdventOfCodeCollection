using _2023.Models.Day07;
using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2023;

public class Day07() : DayBase("07", "Camel Cards")
{
    private List<Hand> _hands = new();

    protected override Task<object> PartOne()
    {
        var result = GetTotalWinnings();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTotalWinnings();

        return Task.FromResult<object>(result);
    }

    private int GetTotalWinnings()
    {
        ProcessInput();
        CalculateWinnings();
        return _hands.Sum(x => x.Winnings);
    }

    private void ProcessInput()
    {
        _hands.Clear();
        foreach (var line in Input)
        {
            var hand = new Hand(0);
            var lineParts = line.Split(Constants.Space).ToList();
            var cards = lineParts.First();
            hand.Bid = int.Parse(lineParts.Last());

            foreach (var card in cards)
                hand.AddCard(card);

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