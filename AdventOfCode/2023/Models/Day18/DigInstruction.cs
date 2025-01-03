﻿using System.Numerics;
using AOC.Utils;

namespace _2023.Models.Day18;

public class DigInstruction<T> where T : struct, INumber<T>
{
    public DigInstruction(string inputLine)
    {
        if (Variables.RunningPartOne)
        {
            Offset = inputLine[0].ToOffset();
            Distance = int.Parse(inputLine.Substring(2, inputLine.IndexOf(' ', 2) - 2));
        }
        else
        {
            var colorHex = inputLine.Substring(inputLine.IndexOf('#') + 1, inputLine.IndexOf(')') - inputLine.IndexOf('#') - 1);
            Distance = Convert.ToInt32(colorHex[..^1], 16);
            var lastChar = colorHex.Last().ToString();
            Offset = int.Parse(lastChar).ToOffset();
        }
    }

    public (int, int) Offset { get; }
    public int Distance { get; }

    public override bool Equals(object? obj)
    {
        if (obj is DigInstruction<T> instruction)
            return Equals(instruction);

        return false;
    }

    private bool Equals(DigInstruction<T> other)
        => Offset == other.Offset && Distance == other.Distance;

    public override int GetHashCode() 
        => HashCode.Combine(Offset, Distance);
}