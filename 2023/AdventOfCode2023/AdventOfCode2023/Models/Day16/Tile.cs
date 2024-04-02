namespace AdventOfCode2023_1.Models.Day16;

public class Tile
{
    public Tile(char tileType)
    {
        TileType = tileType.ToTileType();
    }

    public TileType TileType { get; set; }
    public bool IsEnergised = false;
    private readonly List<Direction> _directionsUsed = new List<Direction>();

    public void SetEnergised()
    {
        IsEnergised = true;
    }

    public bool ShouldStop(Direction inputDirection)
    {
        return _directionsUsed.Contains(inputDirection);
    }

    public void AddDirection(Direction direction)
    {
        _directionsUsed.Add(direction);
    }
}