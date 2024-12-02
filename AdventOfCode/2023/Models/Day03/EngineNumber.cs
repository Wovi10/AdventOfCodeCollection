namespace _2023.Models.Day03;

public class EngineNumber(int rowIndex, int columnIndex, int numberLength, int number)
{
    public int RowIndex { get; } = rowIndex;
    public int ColumnIndex { get; } = columnIndex;
    public int NumberLength { get; set; } = numberLength;
    public int Number { get; set; } = number;
    public bool IsPartNumber { get; set; }
}