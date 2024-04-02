namespace AdventOfCode2023_1.Models.Day03;

public class EngineSymbol(int rowIndex, int columnIndex, char symbol)
{
    public int RowIndex { get; } = rowIndex;
    public int ColumnIndex { get; } = columnIndex;
    public string Symbol { get; } = symbol.ToString();
    public bool IsGear { get; set; }
    public List<EngineNumber>? AdjacentPartNumbers { get; set; }
    public int GearRatio { get; set; }

    internal int GetGearRatio()
        => AdjacentPartNumbers?
               .Aggregate(1, (current, partNumber) => current * partNumber.Number)
           ?? 0;
}