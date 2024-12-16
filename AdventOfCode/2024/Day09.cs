using _2024.Models.Day09;
using AOC.Utils;

namespace _2024;

public class Day09():DayBase("09", "Disk Fragmenter")
{
    protected override Task<object> PartOne()
    {
        var result = GetFileSystemChecksum();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetFileSystemChecksum();

        return Task.FromResult<object>(result);
    }

    private long GetFileSystemChecksum()
        => SharedMethods.GetInput(Day).First().ToFileSystem().Defragment().CheckSum();
}