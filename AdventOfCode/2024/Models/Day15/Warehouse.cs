using System.Text;
using UtilsCSharp.Enums;

namespace _2024.Models.Day15;

public class Warehouse
{
    private IDictionary<Coordinate, ObjectType> WarehouseLookup { get; set; } = new Dictionary<Coordinate, ObjectType>();

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
                var objectType = WarehouseLookup.TryGetValue(coordinate, out var value) ? value : ObjectType.Empty;
                sb.Append(objectType.ToChar());
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void RunInstruction(Direction instruction)
    {

    }
}