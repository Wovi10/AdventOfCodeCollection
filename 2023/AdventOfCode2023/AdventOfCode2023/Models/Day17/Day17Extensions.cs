using AdventOfCode2023_1.Models.Day17.Enums;

namespace AdventOfCode2023_1.Models.Day17;

public static class Day17Extensions
{
    public static List<Coordinates?> GetAdjacentBlocks(this Coordinates coordinates, Direction direction)
    {
        var adjacentBlocks = new List<Coordinates?>();
        var xCoordinate = coordinates.GetXCoordinate();
        var yCoordinate = coordinates.GetYCoordinate();

        switch (direction)
        {
            case Direction.Up:
                adjacentBlocks.Add(null);
                adjacentBlocks.Add(new Coordinates(xCoordinate + 1, yCoordinate));
                adjacentBlocks.Add(new Coordinates(xCoordinate, yCoordinate + 1));
                adjacentBlocks.Add(new Coordinates(xCoordinate - 1, yCoordinate));
                break;
            case Direction.Right:
                adjacentBlocks.Add(new Coordinates(xCoordinate, yCoordinate - 1));
                adjacentBlocks.Add(null);
                adjacentBlocks.Add(new Coordinates(xCoordinate, yCoordinate + 1));
                adjacentBlocks.Add(new Coordinates(xCoordinate - 1, yCoordinate));
                break;
            case Direction.Down:
                adjacentBlocks.Add(new Coordinates(xCoordinate, yCoordinate - 1));
                adjacentBlocks.Add(new Coordinates(xCoordinate + 1, yCoordinate));
                adjacentBlocks.Add(null);
                adjacentBlocks.Add(new Coordinates(xCoordinate - 1, yCoordinate));
                break;
            case Direction.Left:
                adjacentBlocks.Add(new Coordinates(xCoordinate, yCoordinate - 1));
                adjacentBlocks.Add(new Coordinates(xCoordinate + 1, yCoordinate));
                adjacentBlocks.Add(new Coordinates(xCoordinate, yCoordinate + 1));
                adjacentBlocks.Add(null);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return adjacentBlocks;
    }


    public static List<Direction> GetNewDirections(this Direction direction, List<Direction> lastThree)
    {
        var newDirections = new List<Direction>();

        switch (direction)
        {
            case Direction.Up:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Left);
                break;
            case Direction.Right:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Down);
                break;
            case Direction.Down:
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Down);
                newDirections.Add(Direction.Left);
                break;
            case Direction.Left:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Down);
                newDirections.Add(Direction.Left);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (lastThree.Count == 3 && lastThree.All(d => d == direction))
            newDirections.Remove(direction);

        return newDirections;
    }

    public static List<Coordinates> GetNeighbours(this Coordinates currentNode, int height, int width,
        List<Direction> usedDirections)
    {
        var xCoord = currentNode.GetXCoordinate();
        var yCoord = currentNode.GetYCoordinate();

        var lastDirection = usedDirections.Count > 0 ? usedDirections.Last() : Direction.None;
        var lastThreeTheSame = usedDirections.Count > 2 && usedDirections.TakeLast(3).All(d => d == lastDirection);

        List<Coordinates> neighbours = new();

        if (yCoord > 0 && lastDirection != Direction.Down && !(lastThreeTheSame && lastDirection == Direction.Up))
            neighbours.Add(new Coordinates(xCoord, yCoord - 1));

        if (yCoord < height - 1 && lastDirection != Direction.Up &&
            !(lastThreeTheSame && lastDirection == Direction.Down))
            neighbours.Add(new Coordinates(xCoord, yCoord + 1));

        if (xCoord > 0 && lastDirection != Direction.Right && !(lastThreeTheSame && lastDirection == Direction.Left))
            neighbours.Add(new Coordinates(xCoord - 1, yCoord));

        if (xCoord < width - 1 && lastDirection != Direction.Left &&
            !(lastThreeTheSame && lastDirection == Direction.Right))
            neighbours.Add(new Coordinates(xCoord + 1, yCoord));

        return neighbours;
    }

    public static Direction GetDirectionToNeighbour(this Coordinates current, Coordinates neighbour)
    {
        var deltaY = neighbour.GetYCoordinate() - current.GetYCoordinate();
        var deltaX = neighbour.GetXCoordinate() - current.GetXCoordinate();

        return deltaY switch
        {
            -1 when deltaX == 0 => Direction.Up,
            1 when deltaX == 0 => Direction.Down,
            0 when deltaX == -1 => Direction.Left,
            0 when deltaX == 1 => Direction.Right,
            _ => Direction.None
        };
    }

    public static Coordinates Move(this Coordinates current, Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Coordinates(current.GetXCoordinate(), current.GetYCoordinate() - 1),
            Direction.Right => new Coordinates(current.GetXCoordinate() + 1, current.GetYCoordinate()),
            Direction.Down => new Coordinates(current.GetXCoordinate(), current.GetYCoordinate() + 1),
            Direction.Left => new Coordinates(current.GetXCoordinate() - 1, current.GetYCoordinate()),
            _ => current
        };
    }

    public static void SetCoordinate(this Coordinates coordinate, int minimalHeatLoss)
    {
        coordinate.MinimalHeatLoss = minimalHeatLoss;
        coordinate.Visited = true;
    }

    public static List<Coordinates> GetNeighbours(this Coordinates currentNode, List<Coordinates> usedCoordinates,
        int height, int width)
    {
        var lastFourCoordinates = usedCoordinates.TakeLast(4).ToList();
        List<Direction> lastThreeDirections = new();

        for (var i = 0; i < lastFourCoordinates.Count - 1; i++)
        {
            var currentCoordinate = lastFourCoordinates[i];
            var nextCoordinate = lastFourCoordinates[i + 1];
            var direction = currentCoordinate.GetDirectionToNeighbour(nextCoordinate);
            lastThreeDirections.Add(direction);
        }

        var currentDirection = lastThreeDirections.LastOrDefault();
        var canRepeat = lastThreeDirections.Count < 3 || lastThreeDirections.Any(dir => dir != currentDirection);

        var neighbours = new List<Coordinates>
        {
            currentNode.Move(Direction.Up),
            currentNode.Move(Direction.Right),
            currentNode.Move(Direction.Down),
            currentNode.Move(Direction.Left)
        };

        if (!canRepeat)
            neighbours.Remove(currentNode.Move(currentDirection));

        neighbours = neighbours.FilterInvalidNeighbours(currentNode, currentDirection, height, width);
        
        return neighbours.Randomize();
    }

    private static List<Coordinates> FilterInvalidNeighbours(this List<Coordinates> neighbours, Coordinates currentNode, Direction currentDirection, int height, int width)
    {
        switch (currentDirection)
        {
            case Direction.Up:
                neighbours.Remove(currentNode.Move(Direction.Down));
                break;
            case Direction.Right:
                neighbours.Remove(currentNode.Move(Direction.Left));
                break;
            case Direction.Down:
                neighbours.Remove(currentNode.Move(Direction.Up));
                break;
            case Direction.Left:
                neighbours.Remove(currentNode.Move(Direction.Right));
                break;
        }
        return neighbours.Where(c => c.GetXCoordinate() >= 0 && c.GetXCoordinate() < width &&
                                     c.GetYCoordinate() >= 0 && c.GetYCoordinate() < height).ToList();
    }
    
    private static List<Coordinates> Randomize(this List<Coordinates> coordinates)
    {
        var random = new Random();
        return coordinates.OrderBy(_ => random.Next()).ToList();
    }
}