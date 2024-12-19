﻿using AOC.Utils;

namespace _2024.Models.Day12;

public class Garden
{
    public List<Region> Regions { get; set; } = new();
    private Dictionary<Coordinate, char> CheckedCoordinates { get; set; } = new();
    private Dictionary<Coordinate, char> GardenMap { get; set; } = new();
    public int Height { get; init; }
    public int Width { get; init; }

    public Garden(List<string> input)
    {
        Height = input.Count;
        for (var y = 0; y < input.Count; y++)
        {
            var currentLine = input[y];
            Width = currentLine.Length;

            for (var x = 0; x < currentLine.Length; x++)
            {
                var currentChar = currentLine[x];
                var currentCoordinate = new Coordinate(x, y);

                GardenMap.Add(currentCoordinate, currentChar);
            }
        }

        for (var y = 0; y < input.Count; y++)
        {
            var currentLine = input[y];

            for (var x = 0; x < currentLine.Length; x++)
            {
                var currentChar = currentLine[x];
                var currentCoordinate = new Coordinate(x, y);

                if (CheckedCoordinates.ContainsKey(currentCoordinate))
                    continue;

                var newRegion = new Region(currentChar, currentCoordinate);
                CheckNeighbours(currentCoordinate, newRegion);
                Regions.Add(newRegion);
            }
        }
    }

    private void CheckNeighbours(Coordinate currentCoordinate, Region region)
    {
        var neighbours = currentCoordinate.GetNeighbours();
        var filteredNeighbours = neighbours.Where(GardenMap.ContainsKey).Where(n => GardenMap[n] == region.PlantType)
            .Where(n => CheckedCoordinates.All(c => c.Key != n)).ToList();
        foreach (var neighbour in filteredNeighbours)
        {
            region.AddToCoordinates(neighbour);
            CheckedCoordinates.Add(neighbour, region.PlantType);
        }

        filteredNeighbours.ForEach(neighbour => CheckNeighbours(neighbour, region));
    }

    public long GetFencingPrice()
    {
        if (Variables.RunningPartOne)
            return Regions.Sum(region => region.Coordinates.Count * region.PerimeterCount);

        return Regions.Sum(region => region.SidesCount * region.Coordinates.Count);
    }
}