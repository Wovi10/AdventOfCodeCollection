using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> inputLines)
    {
        BuildTileDictionary(inputLines);
        CalculateAdjacentTiles();
        FilterNoneLoopPipes();
    }

    public Dictionary<Coordinates, Tile> TileDictionary = new();
    private int _mazeWidth;
    private int _mazeLength;

    private void BuildTileDictionary(List<string> inputLines)
    {
        _mazeLength = inputLines.Count;
        for (var mazeLineCounter = 0; mazeLineCounter < inputLines.Count; mazeLineCounter++)
        {
            var line = inputLines[mazeLineCounter].Trim();
            _mazeWidth = _mazeWidth == 0 ? line.Length : _mazeWidth;
            for (var tileCounter = 0; tileCounter < line.Length; tileCounter++)
            {
                var tileChar = line[tileCounter];
                var tile = new Tile(tileChar, mazeLineCounter, tileCounter, _mazeWidth, _mazeLength);
                if(tile.TileType == TileType.Ground)
                    continue;
                TileDictionary.Add(tile.Coordinates, tile);
            }
        }
    }

    private void CalculateAdjacentTiles()
    {
        foreach (var (_, tile) in TileDictionary)
        {
            switch (tile.TileType)
            {
                case TileType.NorthSouth:
                    AddNorthTile(tile);
                    AddSouthTile(tile);
                    tile.EastTile = null;
                    tile.WestTile = null;
                    break;
                case TileType.EastWest:
                    AddEastTile(tile);
                    AddWestTile(tile);
                    tile.NorthTile = null;
                    tile.SouthTile = null;
                    break;
                case TileType.NorthEast:
                    AddNorthTile(tile);
                    AddEastTile(tile);
                    tile.SouthTile = null;
                    tile.WestTile = null;
                    break;
                case TileType.NorthWest:
                    AddNorthTile(tile);
                    AddWestTile(tile);
                    tile.SouthTile = null;
                    tile.EastTile = null;
                    break;
                case TileType.SouthWest:
                    AddSouthTile(tile);
                    AddWestTile(tile);
                    tile.NorthTile = null;
                    tile.EastTile = null;
                    break;
                case TileType.SouthEast:
                    AddEastTile(tile);
                    AddSouthTile(tile);
                    tile.NorthTile = null;
                    tile.WestTile = null;
                    break;
                case TileType.StartingPosition:
                    break;
                case TileType.Ground:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (tile.AdjacentTiles.Count == 2 || tile.TileType == TileType.StartingPosition)
                continue;
            tile.TileType = TileType.Ground;
        }
        TileDictionary = TileDictionary.Where(t => t.Value.TileType != TileType.Ground).ToDictionary();
    }

    private void AddNorthTile(Tile tile)
    {
        var northTileCoords = tile.NorthTile;
        if (northTileCoords == null) 
            return;
        var northTile = TileDictionary.FirstOrDefault(t => t.Key.Equals(northTileCoords)).Value;
        if (northTile == null)
            return;

        tile.AddAdjacentTile(northTile.Coordinates);
        if (northTile.TileType == TileType.StartingPosition) 
            northTile.AddAdjacentTile(tile.Coordinates);
    }

    private void AddEastTile(Tile tile)
    {
        var eastTileCoords = tile.EastTile;
        if (eastTileCoords == null) 
            return;
        var eastTile = TileDictionary.FirstOrDefault(t => t.Key.Equals(eastTileCoords)).Value;
        if (eastTile == null)
            return;

        tile.AddAdjacentTile(eastTile.Coordinates);
        if (eastTile.TileType == TileType.StartingPosition) 
            eastTile.AddAdjacentTile(tile.Coordinates);
    }

    private void AddSouthTile(Tile tile)
    {
        var southTileCoords = tile.SouthTile;
        if (southTileCoords == null) 
            return;
        var southTile = TileDictionary.FirstOrDefault(t => t.Key.Equals(southTileCoords)).Value;
        if (southTile == null)
            return;

        tile.AddAdjacentTile(southTile.Coordinates);
        if (southTile.TileType == TileType.StartingPosition) 
            southTile.AddAdjacentTile(tile.Coordinates);
    }

    private void AddWestTile(Tile tile)
    {
        var westTileCoords = tile.WestTile;
        if (westTileCoords == null) 
            return;
        var westTile = TileDictionary.FirstOrDefault(t => t.Key.Equals(westTileCoords)).Value;
        if (westTile == null)
            return;

        tile.AddAdjacentTile(westTile.Coordinates);
        if (westTile.TileType == TileType.StartingPosition) 
            westTile.AddAdjacentTile(tile.Coordinates);
    }

    private void FilterNoneLoopPipes()
    {
        foreach (var (_, tile) in TileDictionary)
        {
            if (tile.TileType == TileType.StartingPosition)
                continue;
            var firstAdjacentTile = TileDictionary.FirstOrDefault(t => t.Key.Equals(tile.AdjacentTiles[0])).Value;
            var secondAdjacentTile = TileDictionary.FirstOrDefault(t => t.Key.Equals(tile.AdjacentTiles[1])).Value;

            if (!TilesAreCoupled(tile, firstAdjacentTile) || !TilesAreCoupled(tile, secondAdjacentTile))
                tile.TileType = TileType.Ground;
        }
        TileDictionary = TileDictionary.Where(t => t.Value.TileType != TileType.Ground).ToDictionary();
    }

    private static bool TilesAreCoupled(Tile tile, Tile? adjacentTile)
    {
        return adjacentTile != null 
               && (NorthIsCoupled(tile, adjacentTile) ||
                   EastIsCoupled(tile, adjacentTile) ||
                   SouthIsCoupled(tile, adjacentTile)||
                   WestIsCoupled(tile, adjacentTile));
    }

    private static bool NorthIsCoupled(Tile tile, Tile adjacentTile)
    {
        return IsCoupled(tile, adjacentTile, Direction.North);
    }

    private static bool EastIsCoupled(Tile tile, Tile adjacentTile)
    {
        return IsCoupled(tile, adjacentTile, Direction.East);
    }

    private static bool SouthIsCoupled(Tile tile, Tile adjacentTile)
    {
        return IsCoupled(tile, adjacentTile, Direction.South);
    }
    private static bool WestIsCoupled(Tile tile, Tile adjacentTile)
    {
        return IsCoupled(tile, adjacentTile, Direction.West);
    }
    
    private static bool IsCoupled(Tile tile, Tile adjacentTile, Direction direction)
    {
        var coordinates = tile.Coordinates;

        return direction switch
        {
            Direction.North => tile.NorthTile != null && Equals(adjacentTile.SouthTile, coordinates),
            Direction.East => tile.EastTile != null && Equals(adjacentTile.WestTile, coordinates),
            Direction.South => tile.SouthTile != null && Equals(adjacentTile.NorthTile, coordinates),
            Direction.West => tile.WestTile != null && Equals(adjacentTile.EastTile, coordinates),
            _ => throw new ArgumentException("Invalid direction.")
        };
    }
}