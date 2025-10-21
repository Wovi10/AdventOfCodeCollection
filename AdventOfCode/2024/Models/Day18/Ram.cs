namespace _2024.Models.Day18;

public class Ram(IDictionary<Tuple<int, int>, bool> coordinates)
{
    private int MaxDimension => coordinates.Keys.Max(k => k.Item1);

    private bool IsBlocked(Tuple<int, int> position)
        => coordinates.ContainsKey(position) && coordinates[position];

    public long FindMinimumStepsToExit()
    {
        var startCoord = new Tuple<int, int>(0, 0);
        var endCoord = new Tuple<int, int>(MaxDimension, MaxDimension);

        var queue = new Queue<(Tuple<int, int> Position, int Steps)>();
        var visited = new HashSet<Tuple<int, int>>();

        queue.Enqueue((startCoord, 0));
        visited.Add(startCoord);

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

            if (current.Equals(endCoord))
                return steps;

            foreach (var direction in directions)
            {
                var newX = current.Item1 + direction.Item1;
                var newY = current.Item2 + direction.Item2;
                var newCoord = new Tuple<int, int>(newX, newY);

                if (newX < 0 || newY < 0 || newX > MaxDimension || newY > MaxDimension)
                    continue;

                if (IsBlocked(newCoord) || visited.Contains(newCoord))
                    continue;

                queue.Enqueue((newCoord, steps + 1));
                visited.Add(newCoord);
            }
        }

        return -1;
    }

    public bool HasPath()
        => FindMinimumStepsToExit() != -1;
}