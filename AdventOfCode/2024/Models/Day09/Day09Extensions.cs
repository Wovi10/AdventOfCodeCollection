namespace _2024.Models.Day09;

public static class Day09Extensions
{
    public static Disk ToFileSystem(this string input)
        => Disk.Parse(input);

    public static Disk Defragment(this Disk disk)
    {
        disk.DefragmentBlocks();

        return disk;
    }
}