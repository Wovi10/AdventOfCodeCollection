using AOC.Utils;

namespace _2024.Models.Day12;

public class Garden
{
    private List<Region> Regions { get; } = new();
    private HashSet<Coordinate> CheckedCoordinatesLookup { get; } = new();
    private Dictionary<Coordinate, char> GardenMapLookup { get; } = new();

    public Garden(List<string> input)
    {
        for (var y = 0; y < input.Count; y++)
        {
            var currentLine = input[y];

            for (var x = 0; x < currentLine.Length; x++)
            {
                GardenMapLookup.Add(new Coordinate(x, y), currentLine[x]);
            }
        }

        foreach (var (coordinate, type) in GardenMapLookup)
        {
            if (CheckedCoordinatesLookup.Contains(coordinate))
                continue;

            var newRegion = new Region(type, coordinate);
            CheckNeighbours(coordinate, newRegion);
            Regions.Add(newRegion);
        }
    }

    private void CheckNeighbours(Coordinate currentCoordinate, Region region)
    {
        var neighbours =
            currentCoordinate
                .GetNeighbours()
                .Where(n => GardenMapLookup.TryGetValue(n, out var value) && value == region.PlantType)
                .Where(n => !CheckedCoordinatesLookup.Contains(n))
                .ToList();

        foreach (var neighbour in neighbours)
        {
            region.AddCoordinate(neighbour);
            CheckedCoordinatesLookup.Add(neighbour);
            CheckNeighbours(neighbour, region);
        }
    }

    public long GetFencingPrice()
        => Variables.RunningPartOne
            ? Regions.Sum(region => region.CoordinateLookup.Count * region.PerimeterCount)
            : Regions.Sum(region => region.CoordinateLookup.Count * region.SidesCount);
}