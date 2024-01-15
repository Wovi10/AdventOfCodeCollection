using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public static class Day02
{
    private static readonly List<string> Input = SharedMethods.GetInput("02", true);

    public static readonly Dictionary<char, int> InputConfig = new()
    {
        {'r', 12},{'g', 13}, {'b', 14}
    };

    public static void Run()
    {
        SharedMethods.WriteBeginText(2, "Cube Conundrum");
        PartOne();
        PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        var result = GetListPossibleGames().Sum();
        SharedMethods.AnswerPart(1, result);
    }

    private static void PartTwo()
    {
        var result = GetListPartTwoGames().Sum();
        SharedMethods.AnswerPart(2, result);
    }

    private static List<int> GetListPartTwoGames()
    {
        var games = Input
            .Select(inputLine => new Game2(inputLine))
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
# region
internal class Game
{
    private List<Set> Sets { get; }
    public int GameIndex { get; }
    private Dictionary<char, int> Colours { get; set; }
    public bool IsValid { get; }

    public Game(string gameLine)
    {
        var gameLineParts = gameLine.Split(Constants.Colon);
        GameIndex = GetIndex(gameLineParts[0]);
        Sets = gameLineParts[1].Split(Constants.SemiColon).Select(set => new Set(set)).ToList();
        Colours = DecideColours(Sets);
        IsValid = IsValidGame();
    }

    private bool IsValidGame()
    {
        foreach (var (key, value) in Colours)
        {
            if (Day02.InputConfig.ContainsKey(key) && Day02.InputConfig[key] < value)
                return false;
        }

        return true;
    }

    private static int GetIndex(string gameIndexPart)
    {
        var outputString = gameIndexPart.Split(Constants.Space).Last();
        return int.Parse(outputString);
    }

    private Dictionary<char,int> DecideColours(List<Set> sets)
    {
        Colours = new Dictionary<char, int>();
        foreach (var set in sets)
        {
            foreach (var (key, value) in set.Colours)
            {
                if (!Colours.ContainsKey(key))
                    Colours.Add(key, value);
                else if (Colours[key] < value)
                    Colours[key] = value;
            }
        }

        return Colours;
    }
}

internal class Set
{
    public Dictionary<char, int> Colours { get; private set; }

    public Set(string colours)
    {
        Colours = DecideColours(colours);
    }

    private Dictionary<char, int> DecideColours(string colours)
    {
        Colours = new Dictionary<char, int>();

        var allColours = colours.Split(Constants.Comma);

        foreach (var colourCombo in allColours)
        {
            var colour = colourCombo.Trim().Split(Constants.Space);
            var key = colour[1][0];
            var amount = int.Parse(colour[0]);

            Colours.TryAdd(key, amount);
        }
        return Colours;
    }
}
# endregion

# region
internal class Game2
{
    private List<Set2> Sets { get; }
    private Dictionary<char, int> Colours { get; set; }
    public int Power { get; }

    public Game2(string gameLine)
    {
        var gameLineParts = gameLine.Split(Constants.Colon);
        Sets = gameLineParts[1].Split(Constants.SemiColon).Select(set => new Set2(set)).ToList();
        Colours = DecideColours(Sets);
        Power = CalculatePower();
    }

    private int CalculatePower()
    {
        var values = Colours.Select(colour => colour.Value);
        var aggregate = values.Aggregate((value1, value2) => value1 * value2);
        return aggregate;
    }

    private Dictionary<char,int> DecideColours(List<Set2> sets)
    {
        Colours = new Dictionary<char, int>();
        foreach (var set in sets)
        {
            foreach (var (key, value) in set.Colours)
            {
                if (!Colours.TryGetValue(key, out var currentValue))
                    Colours.Add(key, value);
                else if (currentValue < value)
                    Colours[key] = value;
            }
        }

        return Colours;
    }
}

internal class Set2
{
    public Dictionary<char, int> Colours { get; private set; }

    public Set2(string colours)
    {
        Colours = DecideColours(colours);
    }

    private Dictionary<char, int> DecideColours(string colours)
    {
        Colours = new Dictionary<char, int>();

        var allColours = colours.Split(Constants.Comma);

        foreach (var colourCombo in allColours)
        {
            var colour = colourCombo.Trim().Split(Constants.Space);
            var key = colour[1][0];
            var amount = int.Parse(colour[0]);

            Colours.TryAdd(key, amount);
        }
        return Colours;
    }
}
# endregion