// using AdventOfCode2023_1.Shared;

using UtilsCSharp.Utils;

namespace _2023.Models.Day04;

public class ScratchCard(string cardId)
{
    public int CardId { get; } = int.Parse(cardId);
    private List<int> WinningNumbers { get; set; } = new();
    private List<int> CardNumbers { get; set; } = new();
    public List<int> MatchingNumbers { get; private set; } = new();
    public int Points { get; private set; }
    public int NumTimesToRun { get; set; } = 1;

    private static async Task<List<int>> ConvertToList(string inputString)
    {
        return await Task.Run(() =>
        {
            var separatedString = inputString.Split(Constants.Space).ToList();
            return separatedString
                .AsParallel()
                .Select(number => int.TryParse(number, out var parsedNumber) ? parsedNumber : (int?) null)
                .Where(parsedNumber => parsedNumber.HasValue)
                .Select(parsedNumber => parsedNumber!.Value)
                .ToList();
        });
    }

    public void SetMatchingNumbers() => MatchingNumbers = CardNumbers.Where(number => WinningNumbers.Contains(number)).ToList();

    public void CalculatePoints()
    {
        if (MatchingNumbers is {Count: 0})
            Points = 0;
        Points = (int) Math.Pow(2, MatchingNumbers.Count - 1);
    }

    public async Task SetWinningNumbers(string trim)
        => WinningNumbers = await ConvertToList(trim);

    public async Task SetCardNumbers(string trim)
        => CardNumbers = await ConvertToList(trim);
}