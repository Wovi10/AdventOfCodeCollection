using UtilsCSharp.Utils;

namespace _2024.Models.Day08;

public class Map
{
    public Map(List<string> input)
    {
        Locations = new List<Location>();
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

    public List<Location> Locations { get; set; }
    public IDictionary<char, List<int>> AntennasOfFrequency { get; set; }
}