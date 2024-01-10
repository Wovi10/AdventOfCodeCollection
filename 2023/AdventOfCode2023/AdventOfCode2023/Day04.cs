using System.Diagnostics;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public static class Day04
{
    private static readonly List<string> Input = SharedMethods.GetInput("04");

    public static void Run()
    {
        SharedMethods.WriteBeginText(4, "Scratchcards");
        PartOne();
        PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        var result = GetSumScratchCardPoints();
        SharedMethods.AnswerPart(1, result);
    }

    private static void PartTwo()
    {
        var result = GetTotalNumberCards();
        SharedMethods.AnswerPart(2, result);
    }

    #region Part 1
    private static int GetSumScratchCardPoints()
    {
        return GetScratchCardPoints().Sum();
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
            var gameNumbers = gameLine.Last();
            var winningNumbers = gameNumbers.Split(Constants.Pipe).ToList().First().Trim();
            var cardNumbers = gameNumbers.Split(Constants.Pipe).ToList().Last().Trim();
            var scratchCard = new ScratchCard(gameId, winningNumbers, cardNumbers);
            scratchCards.Add(scratchCard);
        }

        if (!needPoints) return scratchCards;
        
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
                foreach (var scratchCardToAdjust in scratchCard.MatchingNumbers.Select(_ => scratchCards.FirstOrDefault(card => card.CardId == counter)))
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
    
    private class ScratchCard
    {
        public ScratchCard(string cardId, string winningNumbers, string cardNumbers)
        {
            CardId = int.Parse(cardId);
            WinningNumbers = ConvertToList(winningNumbers);
            CardNumbers = ConvertToList(cardNumbers);
            MatchingNumbers = CalculateMatchingNumbers();
        }

        public int CardId { get; }
        private List<int> WinningNumbers { get; }
        private List<int> CardNumbers { get; }
        public List<int> MatchingNumbers { get; }
        public int Points { get; private set; }
        public int NumTimesToRun { get; set; } = 1;

        private static List<int> ConvertToList(string inputString)
        {
            var separatedString = inputString.Split(Constants.Space).ToList();
            return separatedString.Where(number => int.TryParse(number, out _)).Select(int.Parse).ToList();
        }

        private List<int> CalculateMatchingNumbers()
        {
            return CardNumbers.Where(number => WinningNumbers.Contains(number)).ToList();
        }

        public void CalculatePoints()
        {
            if (MatchingNumbers.Count == 0)
                Points = 0;
            Points = (int)Math.Pow(2, MatchingNumbers.Count - 1);
        }
    }
}