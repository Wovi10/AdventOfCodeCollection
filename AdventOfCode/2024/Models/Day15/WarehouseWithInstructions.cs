using UtilsCSharp.Enums;

namespace _2024.Models.Day15;

public class WarehouseWithInstructions
{
    public Warehouse Warehouse { get; set; }
    public List<Direction> Instructions { get; set; } = new();
    public WarehouseWithInstructions(IEnumerable<string> input)
    {
        var inputAsArray = input as string[] ?? input.ToArray();
        Warehouse = new Warehouse(inputAsArray);

        foreach (var line in inputAsArray)
        {
            if (line.Trim() == string.Empty)
                continue;

            foreach (var c in line)
            {
                var direction = c.ToDirection();
                if (direction is null)
                    continue;
                Instructions.Add(direction.Value);
            }
        }
    }

    public WarehouseWithInstructions RunInstructions()
    {
        Instructions.ForEach(instruction =>
        {
            Warehouse.RunInstruction(instruction);
        });

        return this;
    }
}