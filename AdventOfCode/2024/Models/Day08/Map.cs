using UtilsCSharp.Utils;

namespace _2024.Models.Day08;

public class Map
{
    public List<Location> Locations { get; set; } = new();
    public IDictionary<char, List<int>> AntennasOfFrequency { get; set; }
    public List<Coordinate> AntinodeCoordinates { get; set; } = new();

    public Map(List<string> input)
    {
        AntennasOfFrequency = new Dictionary<char, List<int>>();
        var numAntennas = 0;

        for (var y = 0; y < input.Count; y++)
        {
            var line = input[y];

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
        foreach (var keyValuePair in AntennasOfFrequency)
        {
            FindAntinodesForFrequency(keyValuePair.Key, keyValuePair.Value);
        }
    }

    private void FindAntinodesForFrequency(char freq, List<int> antennaIds)
    {
        foreach (var antennaId in antennaIds)
        {
            var antenna = Locations.First(l => l.Id == antennaId)
        }
    }
}