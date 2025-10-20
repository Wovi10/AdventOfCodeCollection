using _2024.Models.Day16;

namespace _2024.Models.Day18;

public static class Day18Extensions
{
    public static IDictionary<Tuple<int, int>, bool> ToCoordinates(
        this IEnumerable<string> input, int maxDimensions, int numBytesFallen)
    {
        // Dictionary will hold coordinates from 0,0 to maxDimensions,maxDimensions
        // If coordinate is in input, value is true, else false
        var coordinates = new Dictionary<Tuple<int, int>, bool>();
        for (var y = 0; y <= maxDimensions; y++)
        {
            for (var x = 0; x <= maxDimensions; x++)
            {
                var coordinate = new Tuple<int, int>(x, y);
                coordinates[coordinate] = false;
            }
        }

        foreach (var line in input.Take(numBytesFallen))
        {
            var parts = line.Split(',');
            if (parts.Length != 2)
                continue;

            if (!int.TryParse(parts[0], out var x) || !int.TryParse(parts[1], out var y))
                continue;

            var coordinate = new Tuple<int, int>(x, y);
            if (coordinates.ContainsKey(coordinate))
                coordinates[coordinate] = true;
        }

        return coordinates;
    }

    public static IDictionary<Tuple<int, int>, bool> PrintCoordinates(this IDictionary<Tuple<int, int>, bool> coordinates)
    {
        var maxDimensions = coordinates.Keys.Max(k => k.Item1);

        for (var y = 0; y <= maxDimensions; y++)
        {
            for (var x = 0; x <= maxDimensions; x++)
            {
                var coordinate = new Tuple<int, int>(x, y);
                Console.Write(coordinates[coordinate] ? "#" : ".");
            }
            Console.WriteLine();
        }
        return coordinates;
    }

    public static long FindMinimumStepsToExit(this IDictionary<Tuple<int, int>, bool> coordinates)
    {
        return new Ram(coordinates).FindMinimumStepsToExit();
    }
}