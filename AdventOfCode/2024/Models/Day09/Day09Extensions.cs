namespace _2024.Models.Day09;

public static class Day09Extensions
{
    public static FileSystem ToFileSystem(this string input)
        => new(input);
}