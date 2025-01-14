using UtilsCSharp.Enums;

namespace _2024.Models.Day15;

public class WarehouseWithInstructions
{
    public Warehouse Warehouse { get; }
    private List<Direction> Instructions { get; } = new();
    public WarehouseWithInstructions(IEnumerable<string> input)
    {
        var inputAsArray = input as string[] ?? input.ToArray();
        Warehouse = new Warehouse(inputAsArray);

        foreach (var line in inputAsArray)
        {
            if (line.Trim() == string.Empty)
                continue;

            foreach (var direction in line.Select(c => c.ToDirection()).Where(d => d is not null).OfType<Direction>())
                Instructions.Add(direction);
        }
    }

    public WarehouseWithInstructions RunInstructions()
    {
        Instructions.ForEach(instruction => Warehouse.RunInstruction(instruction));

        return this;
    }
}