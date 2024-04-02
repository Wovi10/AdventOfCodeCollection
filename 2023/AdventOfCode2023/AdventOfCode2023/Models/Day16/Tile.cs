namespace AdventOfCode2023_1.Models.Day16;

public class Tile
{
    public Tile(char tileType)
    {
        TileType = tileType.ToTileType();
    }

    public TileType TileType { get; set; }
    public bool IsEnergised = false;

    public void SetEnergised()
    {
        IsEnergised = true;
    }
}