using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public static class Day04
{
    private static readonly string FilePath = Path.Combine(Constants.RootInputPath, "/Day04/Day04.in");
    private static readonly string MockFilePath = Constants.RootInputPath + "/Day04/MockDay04.in";
    private static readonly string FullPath = Directory.GetCurrentDirectory() + MockFilePath;
    private static readonly string InputFile = File.ReadAllText(FullPath);
    private static readonly List<string> Input = SharedMethods.GetInput(InputFile);

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
    
    private class ScratchCard(string winningNumbers, string cardNumbers)
    {
        public List<int> WinningNumbers { get; set; } = ConvertToList(winningNumbers);
        public List<int> CardNumbers { get; set; } = ConvertToList(cardNumbers);
        public int Points { get; set; }

        private static List<int> ConvertToList(string inputString)
        {
            var separatedString = inputString.Split(Constants.Space).ToList();
            return separatedString.Where(number => int.TryParse(number, out _)).Select(int.Parse).ToList();
        }
    }
}