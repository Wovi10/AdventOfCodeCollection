using UtilsCSharp.Utils;

namespace _2024.Models.Day09;

public class File(int id, int value, bool isFile)
{
    public int? Id { get; } = isFile ? id : null;
    public int NumBlocks { get; set; } = value;
    public bool FullyUsed => NumBlocks == 0;

    public override string ToString()
    {
        var blockChar = Id?.ToString() ?? Constants.Dot[0].ToString();
        return new string(blockChar[0], NumBlocks);
    }

    public void RemoveBlock()
        => NumBlocks--;
}