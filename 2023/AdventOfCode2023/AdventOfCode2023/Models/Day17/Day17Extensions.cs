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


    public static List<Direction> GetNewDirections(this Direction direction, int timesDirectionUsed)
    {
        var newDirections = new List<Direction>();
        if (timesDirectionUsed > 3)
            return newDirections;
        
        switch (direction)
        {
            case Direction.Up:
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Down);
                newDirections.Add(Direction.Left);
                break;
            case Direction.Right:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Down);
                newDirections.Add(Direction.Left);
                break;
            case Direction.Down:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Left);
                break;
            case Direction.Left:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Down);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        return newDirections;
    }
}