using UtilsCSharp.Enums;

namespace _2024.Models.Day16;

public class Maze
{
    private readonly Dictionary<Coordinate, ObjectType> _maze = new();
    private readonly ReindeerPositioning _reindeerPositioning;
    private readonly Dictionary<Coordinate, long> _coordinateWeights = new();

    public long MinimumScore => _coordinateWeights[_endCoordinate];
    private readonly Coordinate _endCoordinate;
    private readonly Coordinate _startCoordinate;

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

        _endCoordinate = _maze.First(x => x.Value == ObjectType.End).Key;
        _startCoordinate = _maze.First(x => x.Value == ObjectType.Start).Key;
    }

    public void Run()
    {
        _coordinateWeights[_reindeerPositioning.Position] = 0;
        DoMovement(_reindeerPositioning);
        // PrintWithWeights();
    }

    private const int StepWeight = 1;
    private const int RotateWeight = 1000;

    private void DoMovement(ReindeerPositioning current)
    {
        var neighbours = current.GetNeighbouringCoordinatesWithDirection()
            .Where(n => GetObjectType(n.Position) != ObjectType.Wall && !n.IsOppositeDirection(current.Facing));

        foreach (var neighbour in neighbours)
        {
            var currentWeight = GetWeight(current.Position);
            var weightToAdd = neighbour.Facing == current.Facing ? StepWeight : StepWeight + RotateWeight;
            var possibleNewWeight = currentWeight + weightToAdd;

            var currentNeighbourWeight = _coordinateWeights.GetValueOrDefault(neighbour.Position, long.MaxValue);
            var minimum = Math.Min(possibleNewWeight, currentNeighbourWeight);

            if (minimum == currentNeighbourWeight)
                continue;

            _coordinateWeights[neighbour.Position] = minimum;

            DoMovement(neighbour);
        }
    }

    private ObjectType GetObjectType(Coordinate coordinate)
        => _maze[coordinate];

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

    public int GetBestPathsDistinctLength()
    {
        FindAllBestPaths();

        return _bestPaths
                .SelectMany(p => p)
                .Select(p => p.Position)
                .Distinct()
                .Count();
    }

    private readonly List<ReindeerPositioning[]> _bestPaths = new();
    private void FindAllBestPaths()
    {
        var startingReindeerPositioning = new ReindeerPositioning(_startCoordinate, Direction.Right);
        GetAllPaths([startingReindeerPositioning]);
    }

    private void GetAllPaths(ReindeerPositioning[] positionings)
    {
        var current = positionings.Last();
        var nextWeightStraight = _coordinateWeights[current.Position] + StepWeight;
        var nextWeightStraight2 = _coordinateWeights[current.Position] - RotateWeight + StepWeight;
        var nextWeightRotate = _coordinateWeights[current.Position] + StepWeight + RotateWeight;
        var nextWeightRotate2 = _coordinateWeights[current.Position] - StepWeight + RotateWeight;


        var neighbours =
            current
                .GetNeighbouringCoordinatesWithDirection()
                .Where(neighbour => GetObjectType(neighbour.Position) != ObjectType.Wall &&
                                    !neighbour.IsOppositeDirection(current.Facing) &&
                                    !_bestPaths.Any(path => path.Contains(neighbour)) &&
                                    _coordinateWeights.ContainsKey(neighbour.Position) &&
                                    (_coordinateWeights[neighbour.Position] == nextWeightRotate ||
                                    _coordinateWeights[neighbour.Position] == nextWeightRotate2 ||
                                    _coordinateWeights[neighbour.Position] == nextWeightStraight2 ||
                                     _coordinateWeights[neighbour.Position] == nextWeightStraight));

        foreach (var neighbour in neighbours)
        {
            var possiblePath = new ReindeerPositioning[positionings.Length + 1];
            Array.Copy(positionings, possiblePath, positionings.Length);
            possiblePath[^1] = neighbour;

            if (GetPathScore(possiblePath) > _coordinateWeights[_endCoordinate])
                continue;

            if (neighbour.Position == _endCoordinate)
                _bestPaths.Add(possiblePath);
            else
                GetAllPaths(possiblePath);
        }
    }

    private static int GetPathScore(ReindeerPositioning[] path)
    {
        var previousPositioning = path.First();
        var totalWeight = 0;
        foreach (var positioning in path.Skip(1))
        {
            totalWeight += positioning.Facing == previousPositioning.Facing ? StepWeight : StepWeight + RotateWeight;

            previousPositioning = positioning;
        }

        return totalWeight;
    }

    private void PrintWithWeights()
    {
        var maxX = _maze.Keys.Max(c => c.X);
        var maxY = _maze.Keys.Max(c => c.Y);
        var evenEvenColor = ConsoleColor.Blue;
        var oddEvenColor = ConsoleColor.Green;
        var evenOddColor = ConsoleColor.Yellow;
        var oddOddColor = ConsoleColor.Red;

        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var coordinate = new Coordinate(x, y);
                Console.ForegroundColor =
                    x%2 == 0 && y%2 == 0
                        ? evenEvenColor
                        : x%2 == 0 && y%2 != 0
                            ? evenOddColor
                            : x%2 != 0 && y%2 == 0
                                ? oddEvenColor
                                : oddOddColor;
                if (_coordinateWeights.ContainsKey(coordinate))
                {
                    Console.Write(_coordinateWeights[coordinate]);
                }
                else
                {
                    var objectType = _maze.GetValueOrDefault(coordinate, ObjectType.Empty);
                    Console.Write(objectType.ToChar());
                }
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }
}