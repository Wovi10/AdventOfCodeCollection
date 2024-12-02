namespace _2023.Models.Day23.TileTypes;

public class RightWardsSlope:ITileType
{
    public List<Tile> GetPossibleNeighbourTilesPart1(List<Tile> tiles, Tile currentTile, Hike currentHike)
    {
        return tiles
            .Where(tile => tile.Type is not Forest)
            .Where(tile => currentHike.Tiles.All(currentHikeTile => currentHikeTile != tile))
            .Where(tile => tile.X == currentTile.X + 1 && tile.Y == currentTile.Y)
            .ToList();
    }

    public List<Tile> GetPossibleNeighbourTilesPart2(List<Tile> tiles, Tile currentTile, Hike currentHike) 
        => tiles
            .Where(tile => tile.Type is not Forest)
            .Where(tile => currentHike.Tiles.All(currentHikeTile => currentHikeTile != tile))
            .Where(tile => (tile.X == currentTile.X - 1 && tile.Y == currentTile.Y) ||
                           (tile.X == currentTile.X + 1 && tile.Y == currentTile.Y) ||
                           (tile.X == currentTile.X && tile.Y == currentTile.Y - 1) ||
                           (tile.X == currentTile.X && tile.Y == currentTile.Y + 1))
            .ToList();
}