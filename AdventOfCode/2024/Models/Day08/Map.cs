using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day08;

public class Map
{
    private IDictionary<int, Location> AntennaLookup { get; }
    private IDictionary<(int, int), Location> LocationLookup { get; }
    private Dictionary<char, List<int>> AntennasOfFrequency { get; }
    private List<Coordinate> AntinodeCoordinates { get; } = new();
    private int Width { get; }
    private int Height { get; }

    public Map(List<string> input)
    {
        AntennasOfFrequency = new Dictionary<char, List<int>>();
        var numAntennas = 0;
        Height = input.Count;
        LocationLookup ??= new Dictionary<(int, int), Location>();
        AntennaLookup ??= new Dictionary<int, Location>();

        for (var y = 0; y < input.Count; y++)
        {
            var line = input[y];
            Width = line.Length;

            for (var x = 0; x < line.Length; x++)
            {
                var locationChar = line[x];
                var isAntenna = locationChar != Constants.Dot[0];

                var location = new Location
                {
                    Id = isAntenna ? ++numAntennas : null,
                    Coordinate = new Coordinate(x, y),
                    IsAntenna = isAntenna,
                    Frequency = isAntenna ? locationChar : null
                };

                LocationLookup[(x, y)] = location;

                if (!location.IsAntenna)
                    continue;

                if (!AntennasOfFrequency.ContainsKey(location.Frequency!.Value))
                    AntennasOfFrequency[location.Frequency.Value] = new List<int>();

                AntennasOfFrequency[location.Frequency.Value].Add(location.Id!.Value);
                AntennaLookup[location.Id!.Value] = location;
            }
        }
    }

    public List<Coordinate> GetAntinodeCoordinates()
    {
        foreach (var list in AntennasOfFrequency.Select(aof => aof.Value))
            FindAntinodesForFrequency(list);

        return AntinodeCoordinates.Distinct().OrderBy(c => c.X).ThenBy(c => c.Y).ToList();
    }

    private void FindAntinodesForFrequency(List<int> antennaIds)
    {
        foreach (var antennaId in antennaIds)
        {
            var antenna = GetAntennaLocation(antennaId);
            foreach (var otherAntennaId in antennaIds.Where(ai => ai != antennaId))
            {
                var otherAntenna = GetAntennaLocation(otherAntennaId);
                var distance = antenna.DistanceTo(otherAntenna);

                var locationsOtherDirection = GetLocationsOtherDirection(otherAntenna, distance);

                foreach (var locationOfAntinode
                         in from coordOtherDirection
                             in locationsOtherDirection
                         where coordOtherDirection is {X: >= 0, Y: >= 0} &&
                               coordOtherDirection.X < Width && coordOtherDirection.Y < Height
                         select GetLocation(coordOtherDirection))
                    AntinodeCoordinates.Add(locationOfAntinode.Coordinate);
            }
        }
    }

    private List<Coordinate> GetLocationsOtherDirection(Location otherAntenna, (int, int) distance)
    {
        if (Variables.RunningPartOne)
            return new List<Coordinate>{otherAntenna.Move(distance)};

        var locations = new List<Coordinate> {otherAntenna.Coordinate};
        var nextLocation = otherAntenna.Move(distance);

        while (nextLocation is {X: >= 0, Y: >= 0} && nextLocation.X < Width && nextLocation.Y < Height)
        {
            locations.Add(nextLocation);
            nextLocation = nextLocation.Move(distance);
        }

        return locations;
    }

    private Location GetLocation(Coordinate coordinate)
        => LocationLookup[(coordinate.X, coordinate.Y)];

    private Location GetAntennaLocation(int antennaId)
        => AntennaLookup[antennaId];
}