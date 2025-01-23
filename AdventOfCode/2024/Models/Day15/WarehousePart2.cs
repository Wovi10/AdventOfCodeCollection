using System.Text;
using AOC.Utils;
using UtilsCSharp.Enums;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day15;

public class WarehousePart2
{
    private Dictionary<Coordinate, ObjectType2> WarehouseLookup { get; } = new();

    public WarehousePart2(IEnumerable<string> input)
    {
        var inputArray = input.ToArray();
        for (var y = 0; y < inputArray.Length; y++)
        {
            var line = inputArray[y];
            if (line.Trim() == string.Empty)
                return;

            line = line
                .Replace(ObjectType.Wall.ToChar().ToString(), $"{Constants.HashTag}{Constants.HashTag}")
                .Replace(ObjectType.Box.ToChar().ToString(), "[]")
                .Replace(ObjectType.Empty.ToChar().ToString(), $"{ObjectType.Empty.ToChar()}{ObjectType.Empty.ToChar()}")
                .Replace(ObjectType.Robot.ToChar().ToString(), $"{ObjectType.Robot.ToChar()}{ObjectType.Empty.ToChar()}");

            for (var x = 0; x < line.Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                var objectType = line[x].ToObjectType2();
                WarehouseLookup[coordinate] = objectType;
            }
        }
    }

    public long[] GetGpsLocationBoxes()
        => WarehouseLookup
            .Where(kvp => kvp.Value == ObjectType2.BoxLeft)
            .Select(kvp => kvp.Key)
            .Select(GetGpsLocation)
            .ToArray();

    private static long GetGpsLocation(Coordinate coordinate)
        => 100 * coordinate.Y + coordinate.X;

    public void Print()
    {
        var maxY = WarehouseLookup.Keys.Max(c => c.Y);

        for (var y = 0; y <= maxY; y++)
            PrintHorizontalLine(new Coordinate(0, y));
    }

    public void RunInstruction(Direction instruction)
    {
        var robotLocation = WarehouseLookup.First(kvp => kvp.Value == ObjectType2.Robot).Key;
        var newLocation = robotLocation.Move(instruction).ToCoordinate();
        var objectTypeOfLocation = WarehouseLookup[newLocation];

        switch (objectTypeOfLocation)
        {
            case ObjectType2.Wall:
            case ObjectType2.Robot:
                return;
            case ObjectType2.Empty:
                SwitchLocations(robotLocation, newLocation);
                break;
            case ObjectType2.BoxLeft:
            case ObjectType2.BoxRight:
                MoveBoxes(robotLocation, newLocation, instruction);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SwitchLocations(Coordinate robotLocation, Coordinate newLocation)
        => (WarehouseLookup[robotLocation], WarehouseLookup[newLocation]) = (WarehouseLookup[newLocation], WarehouseLookup[robotLocation]);

    private void MoveBoxes(Coordinate robotLocation, Coordinate newLocation, Direction instruction)
    {
        switch (instruction)
        {
            case Direction.Up:
            case Direction.Down:
                MoveBoxesVertical(robotLocation, newLocation, instruction);
                break;
            case Direction.Right or Direction.Left:
                MoveBoxesHorizontal(robotLocation, newLocation, instruction);
                break;
            case Direction.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null);
        }
    }

    private void PrintVerticalLine(Coordinate robotLocation)
    {
        var maxY = WarehouseLookup.Keys.Max(c => c.Y);
        for (var y = 0; y <= maxY; y++)
        {
            var coordinate = new Coordinate(robotLocation.X, y);
            var objectType = WarehouseLookup.GetValueOrDefault(coordinate, ObjectType2.Empty);
            Console.WriteLine(objectType.ToChar());
        }
    }

    private void PrintHorizontalLine(Coordinate robotLocation)
    {
        var maxX = WarehouseLookup.Keys.Max(c => c.X);
        var sb = new StringBuilder();
        for (var x = 0; x <= maxX; x++)
        {
            var coordinate = new Coordinate(x, robotLocation.Y);
            var objectType = WarehouseLookup.GetValueOrDefault(coordinate, ObjectType2.Empty);
            sb.Append(objectType.ToChar());
        }

        Console.WriteLine(sb.ToString());
    }

    private void MoveBoxesVertical(Coordinate robotLocation, Coordinate boxLocation, Direction direction)
    {
        if (!BoxCanMove(boxLocation, direction))
            return;

        MoveBox(boxLocation, direction);
        SwitchLocations(robotLocation, boxLocation);
    }

    private void MoveBox(Coordinate nextLocation, Direction direction)
    {
        var leftPart = WarehouseLookup[nextLocation] == ObjectType2.BoxLeft ? nextLocation : nextLocation.Move(Direction.Left).ToCoordinate();
        var rightPart = WarehouseLookup[nextLocation] == ObjectType2.BoxRight ? nextLocation : nextLocation.Move(Direction.Right).ToCoordinate();

        MoveHalfBox(leftPart, direction);
        MoveHalfBox(rightPart, direction);
    }

    private void MoveHalfBox(Coordinate boxPart, Direction direction)
    {
        var nextLocation = boxPart.Move(direction).ToCoordinate();

        // var currentObjectType = WarehouseLookup[boxPart];

        if (WarehouseLookup[nextLocation] == ObjectType2.Empty)
            SwitchLocations(boxPart, nextLocation);
        else
        {
            MoveBox(nextLocation, direction);
            SwitchLocations(boxPart, nextLocation);
        }
    }

    private bool BoxCanMove(Coordinate boxLocation, Direction direction)
    {
        var leftPart = WarehouseLookup[boxLocation] == ObjectType2.BoxLeft ? boxLocation : boxLocation.Move(Direction.Left).ToCoordinate();
        var rightPart = WarehouseLookup[boxLocation] == ObjectType2.BoxRight ? boxLocation : boxLocation.Move(Direction.Right).ToCoordinate();

        return CanMove(leftPart, direction) && CanMove(rightPart, direction);
    }

    private bool CanMove(Coordinate leftCoordinate, Direction direction)
    {
        var nextLocation = leftCoordinate.Move(direction).ToCoordinate();
        return WarehouseLookup[nextLocation] switch
        {
            ObjectType2.Wall or ObjectType2.Robot => false,
            ObjectType2.BoxLeft or ObjectType2.BoxRight => BoxCanMove(nextLocation, direction),
            ObjectType2.Empty => true,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void MoveBoxesDown(Coordinate robotLocation, Coordinate newLocation, Direction direction)
    {
        throw new NotImplementedException();
    }

    private void MoveBoxesHorizontal(Coordinate robotLocation, Coordinate boxLocation, Direction direction)
    {
        var nextLocation = boxLocation.Move(direction).ToCoordinate();

        while (WarehouseLookup[nextLocation] is ObjectType2.BoxLeft or ObjectType2.BoxRight)
            nextLocation = nextLocation.Move(direction).ToCoordinate();

        switch (WarehouseLookup[nextLocation])
        {
            case ObjectType2.Wall:
            case ObjectType2.Robot:
                return;
            case ObjectType2.BoxLeft:
            case ObjectType2.BoxRight:
                throw new InvalidOperationException("Cannot move box to box");
            case ObjectType2.Empty:
                SwitchBoxLocations(robotLocation, nextLocation, direction);
                break;
        }
    }

    private void SwitchBoxLocations(Coordinate robotLocation, Coordinate lastLocation, Direction direction)
    {
        direction = GetOppositeDirection(direction);
        var nextLocation = lastLocation.Move(direction).ToCoordinate();

        do
        {
            SwitchLocations(lastLocation, nextLocation);
            lastLocation = nextLocation;
            nextLocation = lastLocation.Move(direction).ToCoordinate();
        } while (nextLocation != robotLocation);
        SwitchLocations(robotLocation, lastLocation);
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}