using AdventOfCode2023_1.Models.Day02;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day02:DayBase
{
    public static readonly Dictionary<char, int> InputConfig = new()
    {
        {'r', 12},{'g', 13}, {'b', 14}
    };

    protected override void PartOne()
    {
        var result = GetListPossibleGames().Sum();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        var result = GetListPartTwoGames().Sum();
        SharedMethods.AnswerPart(2, result);
    }

    private static List<int> GetListPartTwoGames()
    {
        var games = Input
            .Select(inputLine => new Game(inputLine))
            .ToList();

        return games.Select(game => game.Power).ToList();
    }

    private static List<int> GetListPossibleGames()
    {
        var games = Input
            .Select(inputLine => new Game(inputLine))
            .ToList();

        var validGames = games.Where(game => game.IsValid).ToList();

        return validGames.Select(game => game.GameIndex).ToList();
    }
}