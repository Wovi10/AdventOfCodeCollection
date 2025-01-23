using UtilsCSharp.Enums;

namespace _2024.Models.Day15;

public class WarehouseWithInstructionsPart2
{
    public WarehousePart2 Warehouse { get; }
    private List<Direction> Instructions { get; } = new();
    public WarehouseWithInstructionsPart2(IEnumerable<string> input)
    {
        var inputAsArray = input as string[] ?? input.ToArray();
        Warehouse = new WarehousePart2(inputAsArray);

        foreach (var line in inputAsArray)
        {
            if (line.Trim() == string.Empty)
                continue;

            foreach (var direction in line.Select(c => c.ToDirection()).Where(d => d is not null).OfType<Direction>())
                Instructions.Add(direction);
        }
    }

    public WarehouseWithInstructionsPart2 RunInstructions()
    {
        var counter = 1;
        foreach (var instruction in Instructions)
        {
            Warehouse.RunInstruction(instruction);
            counter++;
        }

        Warehouse.Print();

        // Instructions.ForEach(instruction => Warehouse.RunInstruction(instruction));

        return this;
    }
}