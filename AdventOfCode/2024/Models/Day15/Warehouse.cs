using System.Text;
using UtilsCSharp.Enums;

namespace _2024.Models.Day15;

public class Warehouse
{
    private Dictionary<Coordinate, ObjectType> WarehouseLookup { get; } = new();

    public Warehouse(IEnumerable<string> input)
    {
        var inputArray = input.ToArray();
        for (var y = 0; y < inputArray.Length; y++)
        {
            var line = inputArray[y];
            if (line.Trim() == string.Empty)
                return;

            for (var x = 0; x < line.Length; x++)
            {
                var coordinate = new Coordinate(x, y);
                var objectType = line[x].FromChar();
                WarehouseLookup[coordinate] = objectType;
            }
        }
    }

    public long[] GetGpsLocationBoxes()
        => WarehouseLookup
            .Where(kvp => kvp.Value == ObjectType.Box)
            .Select(kvp => kvp.Key)
            .Select(GetGpsLocation)
            .ToArray();

    private static long GetGpsLocation(Coordinate coordinate)
        => 100 * coordinate.Y + coordinate.X;

    public override string ToString()
    {
        var maxX = WarehouseLookup.Keys.Max(c => c.X);
        var maxY = WarehouseLookup.Keys.Max(c => c.Y);

        var sb = new StringBuilder();
        for (var y = 0; y <= maxY; y++)
        {
            for (var x = 0; x <= maxX; x++)
            {
                var coordinate = new Coordinate(x, y);
                var objectType = WarehouseLookup.GetValueOrDefault(coordinate, ObjectType.Empty);
                sb.Append(objectType.ToChar());
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void RunInstruction(Direction instruction)
    {
        var robotLocation = WarehouseLookup.First(kvp => kvp.Value == ObjectType.Robot).Key;
        var newLocation = robotLocation.Move(instruction).ToCoordinate();
        var objectTypeOfLocation = WarehouseLookup[newLocation];

        switch (objectTypeOfLocation)
        {
            case ObjectType.Wall:
            case ObjectType.Robot:
                return;
            case ObjectType.Empty:
                SwitchLocations(robotLocation, newLocation);
                break;
            case ObjectType.Box:
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
        var nextLocation = newLocation.Move(instruction).ToCoordinate();

        while (WarehouseLookup[nextLocation] == ObjectType.Box)
            nextLocation = nextLocation.Move(instruction).ToCoordinate();

        switch (WarehouseLookup[nextLocation])
        {
            case ObjectType.Wall:
            case ObjectType.Robot:
                return;
            case ObjectType.Box:
                throw new InvalidOperationException("Cannot move box to box");
            case ObjectType.Empty:
                SwitchLocations(newLocation, nextLocation);
                SwitchLocations(robotLocation, newLocation);
                break;
        }
    }
}