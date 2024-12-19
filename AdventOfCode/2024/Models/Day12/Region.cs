namespace _2024.Models.Day12;

public class Region
{
    public char PlantType { get; init; }
    public List<Coordinate> Coordinates { get; set; } = new();

    public List<Coordinate> Perimeter => GetPerimeter();
    public int PerimeterCount => GetPerimeterCount();

    public int SidesCount => GetSidesCount();

    public Region(char plantType, Coordinate firstCoordinate)
    {
        PlantType = plantType;
        Coordinates.Add(firstCoordinate);
    }

    public void AddToCoordinates(Coordinate coordinate)
    {
        if (!Coordinates.Contains(coordinate))
            Coordinates.Add(coordinate);
    }

    private int GetPerimeterCount()
        => Coordinates
            .Select(coordinate => coordinate.GetNeighbours())
            .Select(neighbours => neighbours.Count(n => !Coordinates.Contains(n)))
            .Sum();

    private int GetSidesCount()
    {
        var sides = 0;

        sides += CountYSides();
        sides += CountXSides();

        return sides;
    }

    private int CountYSides()
    {
        var sides = 0;
        var numDifferentY = Coordinates.Select(c => c.Y).Distinct().Count();
        var lowestY = Coordinates.Min(c => c.Y);

        // Run over the perimeter coordinates per Y coordinate
        for (var y = 0; y < numDifferentY; y++)
        {
            var coordinatesWithSameY = Perimeter
                .Where(c => c.Y == lowestY + y)
                .ToList();

            // Filter the coordinates that have a neighbour at Y-1 or Y+1 that is not in the region
            var coordinatesWithNeighbourAbove = coordinatesWithSameY
                .Where(c => c.GetNeighbours().Any(n => n.Y == c.Y - 1 && !Coordinates.Contains(n)))
                .OrderBy(c => c.X)
                .ToList();

            var coordinatesWithNeighbourBelow = coordinatesWithSameY
                .Where(c => c.GetNeighbours().Any(n => n.Y == c.Y + 1 && !Coordinates.Contains(n)))
                .OrderBy(c => c.X)
                .ToList();

            Sides(coordinatesWithNeighbourAbove);
            Sides(coordinatesWithNeighbourBelow);
        }

        return sides;

        void Sides(List<Coordinate> coordinatesWithNeighbour)
        {
            int? lastX = null;
            foreach (var coordinate in coordinatesWithNeighbour)
            {
                if (lastX is null)
                {
                    lastX = coordinate.X;
                    sides++;
                    continue;
                }

                if (lastX + 1 == coordinate.X)
                {
                    lastX = coordinate.X;
                    continue;
                }

                sides++;
                lastX = coordinate.X;
            }
        }
    }

    private int CountXSides()
    {
        var sides = 0;
        var numDifferentX = Coordinates.Select(c => c.X).Distinct().Count();
        var lowestX = Coordinates.Min(c => c.X);

        // Run over the perimeter coordinates per Y coordinate
        for (var x = 0; x < numDifferentX; x++)
        {
            var coordinatesWithSameX = Perimeter
                .Where(c => c.X == lowestX + x)
                .ToList();

            // Filter the coordinates that have a neighbour at Y-1 or Y+1 that is not in the region
            var coordinatesWithNeighbourLeft = coordinatesWithSameX
                .Where(c => c.GetNeighbours().Any(n => n.X == c.X - 1 && !Coordinates.Contains(n)))
                .OrderBy(c => c.Y)
                .ToList();

            var coordinatesWithNeighbourRight = coordinatesWithSameX
                .Where(c => c.GetNeighbours().Any(n => n.X == c.X + 1 && !Coordinates.Contains(n)))
                .OrderBy(c => c.Y)
                .ToList();

            Sides(coordinatesWithNeighbourLeft);
            Sides(coordinatesWithNeighbourRight);
        }

        return sides;

        void Sides(List<Coordinate> coordinatesWithNeighbour)
        {
            int? lastY = null;
            foreach (var coordinate in coordinatesWithNeighbour)
            {
                if (lastY is null)
                {
                    lastY = coordinate.Y;
                    sides++;
                    continue;
                }

                if (lastY + 1 == coordinate.Y)
                {
                    lastY = coordinate.Y;
                    continue;
                }

                sides++;
                lastY = coordinate.Y;
            }
        }
    }

    public List<Coordinate> GetPerimeter()
    {
        var perimeter = new List<Coordinate>();

        foreach (var coordinate in Coordinates)
        {
            var neighbours = coordinate
                .GetNeighbours()
                .Where(n => !Coordinates.Contains(n))
                .ToList();
            if (neighbours.Count == 0)
                continue;

            perimeter.Add(coordinate);
        }

        return perimeter;
    }
}