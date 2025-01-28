namespace _2024.Models.Day16;

public static class Day16Extensions
{
    public static Maze ToMaze(this IEnumerable<string> input)
        => new(input.ToArray());

    public static Maze PrintMaze(this Maze maze)
    {
        maze.Print();
        return maze;
    }

    public static long GetBestPathResult(this Maze maze)
    {
        maze.Run();
        return maze.MinimumScore;
    }
}