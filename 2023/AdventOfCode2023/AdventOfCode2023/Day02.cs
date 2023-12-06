using System.Text.RegularExpressions;

namespace AdventOfCode2023_1;

public static class Day02
{
    private static readonly string FilePath = Path.Combine("../../..", "Input/Day02/MockDay02.in");
    private static readonly string FullPath = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
    private static readonly string InputFile = File.ReadAllText(FullPath);

    private static List<Tuple<string, int>> _inputConfig = new()
    {
        new Tuple<string, int>("red", 12),
        new Tuple<string, int>("green", 13),
        new Tuple<string, int>("blue", 14)
    };

    public static void Run()
    {
        Console.WriteLine("Starting day 2 challenge: Cube Conundrum");
        PartOne();
        // PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        var result = GetListPossibleGames();
    }

    private static List<int> GetListPossibleGames()
    {
        var trying = InputFile.Split("\n")
            .Select(inputLine => new {gameIndex = int.Parse(inputLine[5].ToString()), games = inputLine[8..]})
            .Select(inputLine => new {inputLine.gameIndex, games = inputLine.games.Split(";")})
            .Select(game => new {game.gameIndex, sets = game.games.Select(set => set.Split(","))})
            .Select(set => new
            {
                set.gameIndex,
                colours = set.sets.Select(colour =>
                {
                    var colourName = colour[2..].ToString();
                    var numberOfColour = int.Parse(colour[0].ToString());
                    return new Game(new List<Set>
                    {
                    new Set(new List<Grab>
                    {
                    new Grab(colourName, numberOfColour)
                })
            });
                })
            })
            .ToList();
        
        // var listOfValidGames = trying.Select()

        return new List<int>();
        // return InputFile.Split("\n")
        //     .Select(inputLine => new {inputLine, gameIndex = (int)inputLine[5], grabs = inputLine[8..]})
        //     .Select(set => new {set, lastNumber = Regex.Match(t.inputLine, regex, RegexOptions.RightToLeft)})
        //     .Select(t => ParseMatch(t.t.firstNumber.Value) * 10 + ParseMatch(t.lastNumber.Value))
        //     .ToList();
    }

    private static void PartTwo()
    {
        throw new NotImplementedException();
    }
}

internal class Game
{
    internal List<Set> Sets { get; set; }

    public Game(List<Set> sets)
    {
        Sets = sets;
    }
}

internal class Set
{
    public List<Grab> Grabs { get; set; }

    public Set(List<Grab> grabs)
    {
        Grabs = grabs;
    }
}

internal class Grab
{
    public static List<int> Colours = new()
    {
        0,0,0
    };

    public Grab(string colour, int number)
    {
        switch (colour)
        {
            case "red":
                Colours[0] += number;
                return;
            case "blue":
                Colours[1] += number;
                return;
            case "green":
                Colours[2] += number;
                return;
            default:
                return;
        }
    }
}