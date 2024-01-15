using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day04;

public class ScratchCard
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