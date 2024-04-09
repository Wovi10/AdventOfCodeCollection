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

    public static List<Coordinates> GetNeighbours(this Coordinates currentNode, int height, int width, List<Direction> usedDirections)
    {
        var xCoord = currentNode.GetXCoordinate();
        var yCoord = currentNode.GetYCoordinate();

        var lastDirection = usedDirections.Count == 3 ? usedDirections.Last() : Direction.None;
        var timesDirectionUsed = 0;
        var previousDirection = usedDirections.Count == 3 ? usedDirections.First() : Direction.None;
        foreach (var usedDirection in usedDirections.Skip(1))
        {
            if (usedDirection.Equals(previousDirection))
                timesDirectionUsed++;
            else
                break;
        }

        List<Coordinates> neighbours = new();

        if (yCoord > 0 && (lastDirection != Direction.Up || timesDirectionUsed < 3))
            neighbours.Add(new Coordinates(xCoord, yCoord - 1));

        if (yCoord < height - 1 && (lastDirection != Direction.Down || timesDirectionUsed < 3))
            neighbours.Add(new Coordinates(xCoord, yCoord + 1));

        if (xCoord > 0 && (lastDirection != Direction.Left || timesDirectionUsed < 3)) 
            neighbours.Add(new Coordinates(xCoord - 1, yCoord));

        if (xCoord < width - 1 && (lastDirection != Direction.Right || timesDirectionUsed < 3)) 
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
}