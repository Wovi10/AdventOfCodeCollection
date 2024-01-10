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
        var scratchCards = GetScratchCards();
        return scratchCards.Select(card => card.Points).ToList();
    }

    private static List<ScratchCard> GetScratchCards()
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
        var duplicateScratchCards = new List<ScratchCard>();
        var countDuplicateScratchCards = 0;
        foreach (var scratchCard in scratchCards)
        {
            for (var i = 0; i < scratchCard.NumTimesToRun; i++)
            {
                countDuplicateScratchCards++;
                if (scratchCard.MatchingNumbers.Count == 0)
                    continue;

                var counter = scratchCard.CardId + 1;
                foreach (var _ in scratchCard.MatchingNumbers)
                {
                    var scratchCardToAdjust = scratchCards.FirstOrDefault(card => card.CardId == counter);
                    if (scratchCardToAdjust != null)
                    {
                        scratchCardToAdjust.NumTimesToRun++;
                    }

                    counter++;
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
            Points = CalculatePoints();
        }

        public int CardId { get; set; }
        public List<int> WinningNumbers { get; set; }
        public List<int> CardNumbers { get; set; }
        public List<int> MatchingNumbers { get; set; }
        public int Points { get; set; }
        public bool IsCopy { get; set; } = false;
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
        
        private int CalculatePoints()
        {
            if (MatchingNumbers.Count == 0)
                return 0;
            var points = (int)Math.Pow(2, MatchingNumbers.Count - 1);
            return points;
        }
    }
}