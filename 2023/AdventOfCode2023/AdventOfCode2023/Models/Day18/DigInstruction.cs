﻿using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day18;

public class DigInstruction
{
    public DigInstruction(string inputLine)
    {
        if (Variables.RunningPartOne)
        {
            Direction = inputLine[0].CharToDirection();
            Distance = int.Parse(inputLine.Substring(2, inputLine.IndexOf(' ', 2) - 2));
        }
        else
        {
            var colorHex = inputLine.Substring(inputLine.IndexOf('#') + 1, inputLine.IndexOf(')') - inputLine.IndexOf('#') - 1);
            Distance = Convert.ToInt32(colorHex[..^1], 16);
            Direction = colorHex.Last().IntToDirection();
        }
    }

    public (int, int) Direction { get; }
    public int Distance { get; }
}