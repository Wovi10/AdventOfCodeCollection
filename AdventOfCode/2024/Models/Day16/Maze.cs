using UtilsCSharp.Enums;

namespace _2024.Models.Day16;

public class Maze
{
    private readonly Dictionary<Coordinate, ObjectType> _maze = new();
    private readonly ReindeerPositioning _reindeerPositioning;
    private readonly Dictionary<Coordinate, long> _coordinateWeights = new();

    public long MinimumScore => _coordinateWeights[EndCoordinate];
    private Coordinate EndCoordinate => _maze.First(x => x.Value == ObjectType.End).Key;

    public Maze(string[] input)
    {
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y];

            for (var x = 0; x < line.Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                var objectType = line[x].FromChar();
                _maze[coordinate] = objectType;

                if (objectType == ObjectType.Start)
                    _reindeerPositioning = new(coordinate);
            }
        }
    }

    public void Run()
    {
        _coordinateWeights[_reindeerPositioning.Position] = 0;
        DoMovement(_reindeerPositioning);
    }

    private const int StepWeight = 1;
    private const int RotateWeight = 1000;

    public void DoMovement(ReindeerPositioning current)
    {
        var neighbours = current.GetNeighbouringCoordinatesWithDirection()
            .Where(n => GetObjectType(n.Position) != ObjectType.Wall && n.Facing != current.OppositeDirection())
            .ToArray();

        foreach (var neighbour in neighbours)
        {
            var currentWeight = GetWeight(current.Position);
            var weightToAdd = neighbour.Facing == current.Facing ? StepWeight : StepWeight + RotateWeight;
            var possibleNewWeight = currentWeight + weightToAdd;

            var currentNeighbourWeight = _coordinateWeights.GetValueOrDefault(neighbour.Position, long.MaxValue);

            if (Math.Min(possibleNewWeight, currentNeighbourWeight) == currentNeighbourWeight)
                continue;

            _coordinateWeights[neighbour.Position] = possibleNewWeight;

            DoMovement(neighbour);
        }
    }

    private ObjectType GetObjectType(Coordinate coordinate)
        => _maze[coordinate];

    private ObjectType GetCurrentObjectType()
        => _maze[_reindeerPositioning.Position];

    private long GetWeight(Coordinate coordinate)
        => _coordinateWeights[coordinate];

    public void Print()
    {
        var maxX = _maze.Keys.Max(c => c.X);
        var maxY = _maze.Keys.Max(c => c.Y);

        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var coordinate = new Coordinate(x, y);
                var objectType = _maze.GetValueOrDefault(coordinate, ObjectType.Empty);
                Console.Write(objectType.ToChar());
            }

            Console.WriteLine();
        }
    }
}