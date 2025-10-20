namespace _2024.Models.Day18;

public class Ram(IDictionary<Tuple<int, int>, bool> coordinates)
{
    public int MaxDimension => coordinates.Keys.Max(k => k.Item1);

    private bool IsBlocked(Tuple<int, int> position)
        => coordinates.ContainsKey(position) && coordinates[position];

    private HashSet<Coordinate> VisitedPositions = new();
    public IDictionary<Coordinate, bool> Coordinates => ToCoordinates(coordinates);

    public long FindMinimumStepsToExit()
    {
        var startCoord = new Tuple<int, int>(MaxDimension, MaxDimension);
        var endCoord = new Tuple<int, int>(0, 0);

        var queue = new Queue<(Tuple<int, int> Position, int Steps)>();
        queue.Enqueue((startCoord, 0));

        while (queue.Count > 0)
        {

        }
    }


    private static IDictionary<Coordinate, bool> ToCoordinates(IDictionary<Tuple<int, int>, bool> input)
    {
        var dict = new Dictionary<Coordinate, bool>();
        foreach (var kvp in input)
        {
            var coord = new Coordinate(kvp.Key.Item1, kvp.Key.Item2);
            dict[coord] = kvp.Value;
        }
        return dict;
    }
}