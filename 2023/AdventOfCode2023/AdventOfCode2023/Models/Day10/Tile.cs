using AdventOfCode2023_1.Models.Day10.Enums;

namespace AdventOfCode2023_1.Models.Day10;

public class Tile
{
    public Tile(char tileChar, int mazeLineCounter, int tileCounter, int mazeWidth, int mazeLength)
    {
        TileType = tileChar.ToTileType();
        if (TileType == TileType.StartingPosition)
        {
            IsStartingPosition = true;
        }

        var isTopLineAndNorth = mazeLineCounter == 0 &&
                                TileType is TileType.NorthEast or TileType.NorthSouth or TileType.NorthWest;
        var isLeftLineAndWest = tileCounter == 0 &&
                                TileType is TileType.EastWest or TileType.NorthWest or TileType.SouthWest;
        var isBottomLineAndSouth = mazeLineCounter == mazeLength - 1 &&
                                   TileType is TileType.SouthEast or TileType.SouthWest or TileType.NorthSouth;
        var isRightLineAndEast = tileCounter == mazeWidth - 1 &&
                                 TileType is TileType.EastWest or TileType.SouthEast or TileType.NorthEast;

        var pointsOut = isTopLineAndNorth || isLeftLineAndWest || isBottomLineAndSouth || isRightLineAndEast;
        if (pointsOut)
            TileType = TileType.Ground;

        if (TileType == TileType.Ground)
            return;

        Coordinates = new Coordinates(tileCounter, mazeLineCounter);

        SetAdjacentTiles(mazeLineCounter, tileCounter, mazeWidth, mazeLength);
    }

    private void SetAdjacentTiles(int mazeLineCounter, int tileCounter, int mazeWidth, int mazeLength)
    {
        switch (TileType)
        {
            case TileType.NorthSouth:
                NorthTile = mazeLineCounter > 0 ? new Coordinates(tileCounter, mazeLineCounter - 1) : null;
                SouthTile = mazeLineCounter < mazeLength - 1 ? new Coordinates(tileCounter, mazeLineCounter + 1) : null;
                break;
            case TileType.EastWest:
                EastTile = tileCounter < mazeWidth - 1 ? new Coordinates(tileCounter + 1, mazeLineCounter) : null;
                WestTile = tileCounter > 0 ? new Coordinates(tileCounter - 1, mazeLineCounter) : null;
                break;
            case TileType.NorthEast:
                NorthTile = mazeLineCounter > 0 ? new Coordinates(tileCounter, mazeLineCounter - 1) : null;
                EastTile = tileCounter < mazeWidth - 1 ? new Coordinates(tileCounter + 1, mazeLineCounter) : null;
                break;
            case TileType.NorthWest:
                NorthTile = mazeLineCounter > 0 ? new Coordinates(tileCounter, mazeLineCounter - 1) : null;
                WestTile = tileCounter > 0 ? new Coordinates(tileCounter - 1, mazeLineCounter) : null;
                break;
            case TileType.SouthWest:
                SouthTile = mazeLineCounter < mazeLength - 1 ? new Coordinates(tileCounter, mazeLineCounter + 1) : null;
                WestTile = tileCounter > 0 ? new Coordinates(tileCounter - 1, mazeLineCounter) : null;
                break;
            case TileType.SouthEast:
                SouthTile = mazeLineCounter < mazeLength - 1 ? new Coordinates(tileCounter, mazeLineCounter + 1) : null;
                EastTile = tileCounter < mazeWidth - 1 ? new Coordinates(tileCounter + 1, mazeLineCounter) : null;
                break;
            case TileType.Ground:
            case TileType.StartingPosition:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public TileType TileType;
    public Coordinates? NorthTile;
    public Coordinates? EastTile;
    public Coordinates? SouthTile;
    public Coordinates? WestTile;
    public readonly bool IsStartingPosition;
    public readonly List<Coordinates> AdjacentTiles = new();

    public readonly Coordinates Coordinates;

    public void AddAdjacentTile(Coordinates? coordinates)
    {
        if (coordinates == null)
            return;
        if (!AdjacentTiles.Contains(coordinates))
            AdjacentTiles.Add(coordinates);
    }
}

public static class TileExtensions
{
    public static void AddNorthTile(this Tile tile, Dictionary<Coordinates, Tile> tileDictionary)
    {
        var northTileCoords = tile.NorthTile;
        if (northTileCoords == null)
            return;
        var northTile = tileDictionary.FirstOrDefault(t => t.Key.Equals(northTileCoords)).Value;
        if (northTile == null)
            return;

        tile.AddAdjacentTile(northTile.Coordinates);

        if (northTile.TileType != TileType.StartingPosition)
            return;
        northTile.AddAdjacentTile(tile.Coordinates);
        northTile.SouthTile = tile.Coordinates;
    }

    public static void AddEastTile(this Tile tile, Dictionary<Coordinates, Tile> tileDictionary)
    {
        var eastTileCoords = tile.EastTile;
        if (eastTileCoords == null)
            return;
        var eastTile = tileDictionary.FirstOrDefault(t => t.Key.Equals(eastTileCoords)).Value;
        if (eastTile == null)
            return;

        tile.AddAdjacentTile(eastTile.Coordinates);

        if (eastTile.TileType != TileType.StartingPosition)
            return;
        eastTile.AddAdjacentTile(tile.Coordinates);
        eastTile.WestTile = tile.Coordinates;
    }

    public static void AddSouthTile(this Tile tile, Dictionary<Coordinates, Tile> tileDictionary)
    {
        var southTileCoords = tile.SouthTile;
        if (southTileCoords == null)
            return;
        var southTile = tileDictionary.FirstOrDefault(t => t.Key.Equals(southTileCoords)).Value;
        if (southTile == null)
            return;

        tile.AddAdjacentTile(southTile.Coordinates);

        if (southTile.TileType != TileType.StartingPosition)
            return;
        southTile.AddAdjacentTile(tile.Coordinates);
        southTile.NorthTile = tile.Coordinates;
    }

    public static void AddWestTile(this Tile tile, Dictionary<Coordinates, Tile> tileDictionary)
    {
        var westTileCoords = tile.WestTile;
        if (westTileCoords == null)
            return;
        var westTile = tileDictionary.FirstOrDefault(t => t.Key.Equals(westTileCoords)).Value;
        if (westTile == null)
            return;

        tile.AddAdjacentTile(westTile.Coordinates);

        if (westTile.TileType != TileType.StartingPosition)
            return;
        westTile.AddAdjacentTile(tile.Coordinates);
        westTile.EastTile = tile.Coordinates;
    }
}