﻿using UtilsCSharp.Utils;

namespace _2023.Models.Day02;

public class Game
{
    private List<Set> Sets { get; }
    public int GameIndex { get; }
    private Dictionary<char, int> Colours { get; set; }
    public bool IsValid { get; }
    public int Power { get; }

    public Game(string gameLine)
    {
        var gameLineParts = gameLine.Split(Constants.Colon);
        GameIndex = GetIndex(gameLineParts[0]);
        Sets = gameLineParts[1].Split(Constants.SemiColon).Select(set => new Set(set)).ToList();
        Colours = DecideColours(Sets);
        IsValid = IsValidGame();
        Power = CalculatePower();
    }

    private bool IsValidGame()
    {
        var inputConfig = new Dictionary<char, int>()
        {
            {'r', 12}, {'g', 13}, {'b', 14}
        };
        foreach (var (key, value) in Colours)
        {
            if (inputConfig.TryGetValue(key, out int inputConfigValue) && inputConfigValue < value)
                return false;
        }

        return true;
    }

    private static int GetIndex(string gameIndexPart)
    {
        var outputString = gameIndexPart.Split(Constants.Space).Last();
        return int.Parse(outputString);
    }

    private int CalculatePower()
    {
        var values = Colours.Select(colour => colour.Value);
        var aggregate = values.Aggregate((colourValue1, colourValue2) => colourValue1 * colourValue2);
        return aggregate;
    }

    private Dictionary<char, int> DecideColours(List<Set> sets)
    {
        Colours = new Dictionary<char, int>();
        foreach (var set in sets)
        {
            foreach (var (key, colourInt) in set.Colours)
            {
                if (!Colours.TryGetValue(key, out var value))
                    Colours.Add(key, colourInt);
                else if (value < colourInt)
                    Colours[key] = colourInt;
            }
        }

        return Colours;
    }
}