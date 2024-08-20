using System.Text;

namespace AdventOfCode2023_1.Models.Day23;

public class Hike
{
    public List<Tile> Tiles { get; set; } = new();
    public int Length => Tiles?.Count-1 ?? 0;

    public Hike(List<Tile> preExistingTiles)
    {
        Tiles.AddRange(preExistingTiles);
    }

    public Hike(Tile preExistingTile)
    {
        AddTile(preExistingTile);
    }

    public Hike() { }
    
    public bool AddTile(Tile tile)
    {
        if (Tiles.Contains(tile))
            return false;

        Tiles.Add(tile);
        return true;
    }
    
    // public override string ToString()
    // {
    //     var sb = new StringBuilder();
    //     foreach (var tile in Tiles)
    //     {
    //         sb.Append($"{tile} ->");
    //     }
    //     
    //     return sb.ToString();
    // }
}