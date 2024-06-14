using AdventOfCode2023_1.Models.Day04;
using UtilsCSharp.Utils;

namespace AdventOfCode2023_1;

public class Day04 : DayBase
{
    protected override Task<object> PartOne()
    {
        var result = GetSumScratchCardPoints();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTotalNumberCards();

        return Task.FromResult<object>(result);
    }

    #region Part 1

    private static int GetSumScratchCardPoints()
    {
        var scratchCardPoints = GetScratchCardPoints();
        return scratchCardPoints.Sum();
    }

    private static List<int> GetScratchCardPoints()
    {
        var scratchCards = GetScratchCards(true);

        return scratchCards.Select(card => card.Points).ToList();
    }

    private static List<ScratchCard> GetScratchCards(bool needPoints = false)
    {
        var scratchCards = new List<ScratchCard>();
        foreach (var line in Input)
        {
            var gameLine = line.Split(Constants.Colon).ToList();
            var gameId = gameLine.First().Split(Constants.Space).Last();
            var scratchCard = new ScratchCard(gameId);

            var gameNumbers = gameLine.Last();
            var taskWinningNumbers = scratchCard.SetWinningNumbers(gameNumbers.Split(Constants.Pipe).ToList().First().Trim());
            var taskCardNumbers = scratchCard.SetCardNumbers(gameNumbers.Split(Constants.Pipe).ToList().Last().Trim());

            Task.WhenAll(taskWinningNumbers, taskCardNumbers).Wait();

            scratchCard.SetMatchingNumbers();

            scratchCards.Add(scratchCard);
        }

        if (!needPoints)
            return scratchCards;

        foreach (var scratchCard in scratchCards)
            scratchCard.CalculatePoints();

        return scratchCards;
    }

    #endregion

    #region Part 2

    private static int GetTotalNumberCards()
    {
        var scratchCards = GetScratchCards();
        var numScratchCards = CountDuplicateScratchCards(scratchCards);
        return numScratchCards;
    }

    private static int CountDuplicateScratchCards(List<ScratchCard> scratchCards)
    {
        var initialCount = scratchCards.Count;
        var countDuplicateScratchCards = 0;
        foreach (var scratchCard in scratchCards)
        {
            countDuplicateScratchCards += scratchCard.NumTimesToRun;
            for (var i = 0; i < scratchCard.NumTimesToRun; i++)
            {
                if (scratchCard.MatchingNumbers.Count == 0)
                    continue;

                var counter = scratchCard.CardId + 1;

                if (counter > initialCount)
                    continue;

                foreach (var scratchCardToAdjust in scratchCard.MatchingNumbers.Select(_ =>
                             scratchCards.FirstOrDefault(card => card.CardId == counter)))
                {
                    counter++;
                    if (scratchCardToAdjust != null)
                        scratchCardToAdjust.NumTimesToRun++;
                }
            }
        }

        return countDuplicateScratchCards;
    }

    #endregion
}