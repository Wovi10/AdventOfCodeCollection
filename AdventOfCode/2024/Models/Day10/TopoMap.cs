﻿using AOC.Utils;
using UtilsCSharp.Enums;

namespace _2024.Models.Day10;

public class TopoMap
{
    private List<Trail> Trails { get; } = new();
    private List<TrailHead> TrailHeads { get; } = new();
    private Dictionary<Coordinate, int> HeightsLookup { get; }

    public TopoMap(List<string> input)
    {
        HeightsLookup = new Dictionary<Coordinate, int>();

        for (var y = 0; y < input.Count; y++)
        {
            var currentLine = input[y];

            for (var x = 0; x < currentLine.Length; x++)
            {
                var currentChar = currentLine[x];
                var currentCharAsDigit = currentChar.AsDigit();
                var currentCoordinate = new Coordinate(x, y);

                HeightsLookup[currentCoordinate] = currentCharAsDigit;

                if (currentCharAsDigit == 0)
                    TrailHeads.Add(new TrailHead(currentCoordinate));
            }
        }
    }

    private static readonly Direction[] Directions = { Direction.Up, Direction.Right, Direction.Down, Direction.Left };

    public int FindScore()
    {
        foreach (var trailHead in TrailHeads)
        {
            var trailHeadCoordinate = trailHead.Coordinate;
            var trail = new Trail(new List<Coordinate> { trailHeadCoordinate });

            ComputeTrail(trail, trailHead);
        }

        return Trails.Count;
    }

    private bool TryGetValueAtCoordinate(Coordinate coordinateToCheck, out int valueAtCoordinate)
        => HeightsLookup.TryGetValue(coordinateToCheck, out valueAtCoordinate);

    private void ComputeTrail(Trail trail, TrailHead trailHead)
    {
        if (trail.Path.Count > 10)
            return;

        var lastCoordinate = trail.Path.Last();
        if (TryGetValueAtCoordinate(lastCoordinate, out var value) && value == 9)
        {
            if (Variables.RunningPartOne && trailHead.TrailEnds.Contains(lastCoordinate))
                return;

            trailHead.TrailEnds.Add(lastCoordinate);
            Trails.Add(trail);
            return;
        }

        var currentCoordinate = trail.Path.Last();
        var valueToLookFor = trail.Path.Count;

        foreach (var direction in Directions)
        {
            var coordinateToCheck = new Coordinate(currentCoordinate.Move(direction));
            if (!TryGetValueAtCoordinate(coordinateToCheck, out var valueAtCoordinate) || valueAtCoordinate != valueToLookFor)
                continue;

            var newTrail = new Trail(trail);
            newTrail.Path.Add(coordinateToCheck);
            ComputeTrail(newTrail, trailHead);
        }
    }
}