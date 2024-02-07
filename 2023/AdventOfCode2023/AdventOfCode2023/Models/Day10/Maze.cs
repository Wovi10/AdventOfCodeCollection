namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> tiles)
    {
        Tiles = GetTiles(tiles) ?? throw new ArgumentNullException(nameof(tiles));
    }

    public List<List<Tile>> Tiles { get; set; }
    public int XLength { get; set; }
    public int YLength { get; set; }
    

    private static List<List<Tile>> GetTiles(List<string> tiles)
    {
        throw new NotImplementedException();
    }
}