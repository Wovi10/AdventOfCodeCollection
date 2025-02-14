﻿using System.Numerics;
using UtilsCSharp;

namespace _2023.Models.Day11;

public class GalaxyPair<T>(Galaxy<T> galaxy, Galaxy<T> galaxy2) where T  : struct, INumber<T>
{
    public T ManhattanDistance 
        => MathUtils.ManhattanDistance((galaxy.X, galaxy.Y), (galaxy2.X, galaxy2.Y));
}