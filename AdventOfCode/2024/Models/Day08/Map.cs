using UtilsCSharp.Utils;

namespace _2024.Models.Day08;

public class Map
{
    public List<Location> Locations { get; set; } = new();
    public IDictionary<char, List<int>> AntennasOfFrequency { get; set; }
    public List<Coordinate> AntinodeCoordinates { get; set; } = new();
    public int Width { get; set; }
    public int Height { get; set; }

    public Map(List<string> input)
    {
        AntennasOfFrequency = new Dictionary<char, List<int>>();
        var numAntennas = 0;
        Height = input.Count;

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

                Locations.Add(location);

                if (!location.IsAntenna)
                    continue;

                if (!AntennasOfFrequency.ContainsKey(location.Frequency!.Value))
                    AntennasOfFrequency[location.Frequency.Value] = new List<int>();

                AntennasOfFrequency[location.Frequency.Value].Add(location.Id!.Value);
            }
        }
    }

    public List<Coordinate> GetAntinodeCoordinates()
    {
        foreach (var (_, list) in AntennasOfFrequency)
            FindAntinodesForFrequency(list);

        return AntinodeCoordinates.Distinct().ToList();
    }

    private void FindAntinodesForFrequency(List<int> antennaIds)
    {
        foreach (var antennaId in antennaIds)
        {
            var antenna = Locations.First(l => l.Id == antennaId);
            foreach (var otherAntennaId in antennaIds.Where(ai => ai != antennaId))
            {
                var otherAntenna = Locations.First(l => l.Id == otherAntennaId);
                var distance = antenna.DistanceTo(otherAntenna);
                var locationOtherDirection = otherAntenna.Move(distance);
                if (locationOtherDirection.Item1 < 0 || locationOtherDirection.Item2 < 0 ||
                    locationOtherDirection.Item1 >= Width || locationOtherDirection.Item2 >= Height)
                    continue;

                var locationOfAntinode = Locations.First(l => l.Coordinate.X == locationOtherDirection.Item1 && l.Coordinate.Y == locationOtherDirection.Item2);
                AntinodeCoordinates.Add(locationOfAntinode.Coordinate);
            }
        }
    }
}