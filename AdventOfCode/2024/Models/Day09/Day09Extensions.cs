namespace _2024.Models.Day09;

public static class Day09Extensions
{
    public static Disk ToFileSystem(this string input)
        => Disk.Parse(input);
}