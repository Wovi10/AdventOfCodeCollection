namespace _2024.Models.Day20;

public class Race
{
    public Race(Race race, (int, int) positionToChange)
    {
        RacePositions = race.RacePositions;
        RacePositions[positionToChange] = TileType.Track;
        _maxY = race._maxY;
        _maxX = race._maxX;
    }

    public Dictionary<(int, int), TileType> RacePositions { get; set; } = new();
    public long FastestRoute = long.MaxValue;
    private long _maxX = 0;
    private long _maxY = 0;
    public Race(string[] input)
    {
        _maxX = input.Length-1;
        _maxY = input.First().Length - 1;
        for (var y = 0; y < input.Length; y++)
        {
            var row = input[y];
            for (var x = 0; x < row.Length; x++)
            {
                var tileType = row[x].FromChar();
                RacePositions[(x, y)] = tileType;
            }
        }
    }

    public void SetFastestRoute()
    {
        FastestRoute = FindMinimumStepsToExit();
    }


    private bool IsBlocked((int, int) position)
        => !RacePositions.TryGetValue(position, out var tileType) || tileType == TileType.Wall;

    private long FindMinimumStepsToExit()
    {
        var startPosition = RacePositions.First(x => x.Value == TileType.Start).Key;
        var endPosition = RacePositions.First(x => x.Value == TileType.End).Key;

        var queue = new Queue<((int, int) Position, int Steps)>();
        var visited = new HashSet<(int, int)>();

        queue.Enqueue((startPosition, 0));
        visited.Add(startPosition);

        var directions = new[]
        {
            new Tuple<int, int>(0, 1),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(-1, 0)
        };

        while (queue.Count > 0)
        {
            var (current, steps) = queue.Dequeue();

            if (current.Equals(endPosition))
                return steps;

            foreach (var direction in directions)
            {
                var newX = current.Item1 + direction.Item1;
                var newY = current.Item2 + direction.Item2;
                var newPosition = (newX, newY);

                if (newX < 0 || newY < 0 || newX > _maxX || newY > _maxY)
                    continue;

                if (IsBlocked(newPosition) || visited.Contains(newPosition))
                    continue;

                queue.Enqueue((newPosition, steps + 1));
                visited.Add(newPosition);
            }
        }

        return -1;
    }

    public IEnumerable<(int, int)> GetAllWalls()
    {
        return RacePositions
            .Where(x => x.Value == TileType.Wall)
            .Select(x => x.Key);
    }
}