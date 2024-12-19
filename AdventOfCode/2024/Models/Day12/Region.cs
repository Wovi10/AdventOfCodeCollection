namespace _2024.Models.Day12;

public class Region
{
    public char PlantType { get; }
    // public Dictionary<Coordinate, char> CoordinateLookup { get; } = new();
    public HashSet<Coordinate> CoordinateLookup { get; } = new();

    private List<Coordinate> Perimeter => GetPerimeter();
    public int PerimeterCount => GetPerimeterCount();
    public int SidesCount => GetSidesCount();

    public Region(char plantType, Coordinate firstCoordinate)
    {
        PlantType = plantType;
        CoordinateLookup.Add(firstCoordinate);
    }

    private int GetPerimeterCount()
        => CoordinateLookup
            .Select(coordinate => coordinate.GetNeighbours())
            .Select(neighbours => neighbours.Count(n => !CoordinateLookup.Contains(n)))
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
        var lowestY = CoordinateLookup.Min(c => c.Y);
        var highestY = CoordinateLookup.Max(c => c.Y);
        var numDifferentY = highestY - lowestY + 1;

        if (numDifferentY == 1)
            return 2;


        for (var y = 0; y < numDifferentY; y++)
        {
            var coordinatesWithSameY = Perimeter
                .Where(c => c.Y == lowestY + y)
                .Where(c => c.GetNeighbours().Any(n => (n.Y == c.Y - 1 || n.Y == c.Y + 1) && !CoordinateLookup.Contains(n)))
                .OrderBy(c => c.X)
                .ToList();

            if (coordinatesWithSameY.Count == 0)
                continue;

            var coordinatesWithNeighbourAbove = coordinatesWithSameY
                .Where(c => c.GetNeighbours().Any(n => n.Y == c.Y - 1 && !CoordinateLookup.Contains(n)))
                .ToList();

            var coordinatesWithNeighbourBelow = coordinatesWithSameY
                .Where(c => c.GetNeighbours().Any(n => n.Y == c.Y + 1 && !CoordinateLookup.Contains(n)))
                .ToList();

            AddSidesFor(coordinatesWithNeighbourAbove);
            AddSidesFor(coordinatesWithNeighbourBelow);
        }

        return sides;

        void AddSidesFor(List<Coordinate> coordinatesWithNeighbour)
        {
            if (coordinatesWithNeighbour.Count == 0)
                return;

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
        var lowestX = CoordinateLookup.Min(c => c.X);
        var highestX = CoordinateLookup.Max(c => c.X);
        var numDifferentX = highestX - lowestX + 1;
        if (numDifferentX == 1)
            return 2;

        for (var x = 0; x < numDifferentX; x++)
        {
            var coordinatesWithSameX = Perimeter
                .Where(c => c.X == lowestX + x)
                .Where(c => c.GetNeighbours().Any(n => (n.X == c.X - 1 || n.X == c.X + 1) && !CoordinateLookup.Contains(n)))
                .OrderBy(c => c.Y)
                .ToList();

            if (coordinatesWithSameX.Count == 0)
                continue;

            var coordinatesWithNeighbourLeft = coordinatesWithSameX
                .Where(c => c.GetNeighbours().Any(n => n.X == c.X - 1 && !CoordinateLookup.Contains(n)))
                .ToList();

            var coordinatesWithNeighbourRight = coordinatesWithSameX
                .Where(c => c.GetNeighbours().Any(n => n.X == c.X + 1 && !CoordinateLookup.Contains(n)))
                .ToList();

            AddSidesFor(coordinatesWithNeighbourLeft);
            AddSidesFor(coordinatesWithNeighbourRight);
        }

        return sides;

        void AddSidesFor(List<Coordinate> coordinatesWithNeighbour)
        {
            if (coordinatesWithNeighbour.Count == 0)
                return;

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

    private List<Coordinate> GetPerimeter()
    {
        return
            (
                from coordinate in CoordinateLookup
                let neighbours = coordinate
                                    .GetNeighbours()
                                    .Where(n => !CoordinateLookup.Contains(n))
                                    .ToList()
                where neighbours.Count != 0
                select coordinate
            ).ToList();
    }

    public void AddCoordinate(Coordinate neighbour)
        => CoordinateLookup.Add(neighbour);
}