using _2023.Models.Day23.TileTypes;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2023.Models.Day23;

public class Tile(int internalX, int internalY, char type) : NodeBase<int>(internalX, internalY)
{
    public ITileType Type { get; init; } = type.ToTileType();

    public List<Tile> GetPossibleNeighbourTiles(List<Tile> tiles, Hike currentHike)
        => Type.GetPossibleNeighbourTiles(tiles, this, currentHike);

    public override (int, int) Move(Direction direction, int distance = 1) 
        => direction switch 
            {
                Direction.Up => (X, Y - distance),
                Direction.Right => (X + distance, Y),
                Direction.Down => (X, Y + distance),
                Direction.Left => (X - distance, Y),
                _ => (X, Y)
            };
    
    public List<Tile> GetNeighbourTiles(List<Tile> tiles)
        => tiles
            .Where(tile => (tile.X == X - 1 && tile.Y == Y) ||
                           (tile.X == X + 1 && tile.Y == Y) ||
                           (tile.X == X && tile.Y == Y - 1) ||
                           (tile.X == X && tile.Y == Y + 1))
            .Where(tile => tile.Type is not Forest)
            .ToList();
    
    public List<Tile> GetNeighbourTilesExcludingPrevious(List<Tile> tiles, Tile previousTile)
        => tiles
            .Where(tile => (tile.X == X - 1 && tile.Y == Y) ||
                           (tile.X == X + 1 && tile.Y == Y) ||
                           (tile.X == X && tile.Y == Y - 1) ||
                           (tile.X == X && tile.Y == Y + 1))
            .Where(tile => tile != previousTile)
            .Where(tile => tile.Type is not Forest)
            .ToList();
}