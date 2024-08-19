namespace AdventOfCode2023_1.Models.Day23;

public class Hike
{
    public List<Tile> Tiles { get; set; } = new();
    public bool IsPossible { get; set; } = true;
    public int Length => Tiles?.Count-1 ?? 0;

    public Hike()
    {
    }
    
    public void AddTile(Tile tile)
    {
        if (!IsPossible)
            return;

        if (Tiles.Contains(tile))
        {
            IsPossible = false;
            return;
        }

        Tiles.Add(tile);
    }
}