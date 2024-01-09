using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public static class Day04
{
    private static readonly List<string> Input = SharedMethods.GetInput("04", true);

    public static void Run()
    {
        SharedMethods.WriteBeginText(4, "Scratchcards");
        PartOne();
        // PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        var result = GetSumScratchCardPoints();
        SharedMethods.AnswerPart(1, result);
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
            var gameNumbers = line.Split(Constants.Colon).ToList().Last();
            var winningNumbers = gameNumbers.Split(Constants.Pipe).ToList().First().Trim();
            var cardNumbers = gameNumbers.Split(Constants.Pipe).ToList().Last().Trim();
            var scratchCard = new ScratchCard(winningNumbers, cardNumbers);
            scratchCards.Add(scratchCard);
        }

        return scratchCards;
    }
    #endregion

    private class ScratchCard
    {
        public ScratchCard(string winningNumbers, string cardNumbers)
        {
            WinningNumbers = ConvertToList(winningNumbers);
            CardNumbers = ConvertToList(cardNumbers);
            Points = CalculatePoints();
        }

        private List<int> WinningNumbers { get; set; }
        private List<int> CardNumbers { get; set; }
        public int Points { get; set; }

        private static List<int> ConvertToList(string inputString)
        {
            var separatedString = inputString.Split(Constants.Space).ToList();
            return separatedString.Where(number => int.TryParse(number, out _)).Select(int.Parse).ToList();
        }

        private int CalculatePoints()
        {
            // You get 1 point if 1 number from Cardnumbers is in WinningNumbers. For each additional number your points get doubled.
            var points = 0;
            foreach (var _ in CardNumbers.Where(cardNumber => WinningNumbers.Contains(cardNumber)))
            {
                if (points > 0)
                    points *= 2;
                else
                    points++;
            }

            return points;
        }
    }
}