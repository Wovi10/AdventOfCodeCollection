using AdventOfCode2023_1.Models.Day02;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day02() : DayBase("02", "Cube Conundrum")
{
    protected override Task<object> PartOne()
    {
        var result = GetListPossibleGames().Sum();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetListPartTwoGames().Sum();

        return Task.FromResult<object>(result);
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