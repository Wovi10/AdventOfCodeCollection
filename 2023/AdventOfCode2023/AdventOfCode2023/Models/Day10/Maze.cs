using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> inputLines)
    {
        BuildTileDictionary(inputLines);
        CalculateAdjacentTiles();
    }

    public Dictionary<string, Tile> TilesDictionary = new();
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
                TilesDictionary.Add(tile.Coordinates.ToString(), tile);
            }
        }
    }

    private void CalculateAdjacentTiles()
    {
        foreach (var (_, tile) in TilesDictionary)
        {
            switch (tile.TileType)
            {
                case TileType.NorthSouth:
                    AddNorthTile(tile);
                    AddSouthTile(tile);
                    break;
                case TileType.EastWest:
                    AddEastTile(tile);
                    AddWestTile(tile);
                    break;
                case TileType.NorthEast:
                    AddNorthTile(tile);
                    AddEastTile(tile);
                    break;
                case TileType.NorthWest:
                    AddNorthTile(tile);
                    AddWestTile(tile);
                    break;
                case TileType.SouthWest:
                    AddSouthTile(tile);
                    AddWestTile(tile);
                    break;
                case TileType.SouthEast:
                    AddEastTile(tile);
                    AddSouthTile(tile);
                    break;
                case TileType.Ground:
                case TileType.StartingPosition:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (tile.AdjacentTiles.Count == 2 || tile.TileType == TileType.StartingPosition) 
                continue;
            tile.AdjacentTiles.Clear();
        }
    }

    private void AddNorthTile(Tile tile)
    {
        var northTileCoords = tile.NorthTile;
        if (string.IsNullOrWhiteSpace(northTileCoords)) 
            return;
        var northTile = TilesDictionary[northTileCoords];
        if (northTile.TileType == TileType.Ground)
            return;
        
        tile.AddAdjacentTile(northTile.Coordinates);
        if (northTile.TileType == TileType.StartingPosition) 
            northTile.AddAdjacentTile(tile.Coordinates);
    }

    private void AddEastTile(Tile tile)
    {
        var eastTileCoords = tile.EastTile;
        if (string.IsNullOrWhiteSpace(eastTileCoords)) 
            return;
        var eastTile = TilesDictionary[eastTileCoords];
        if (eastTile.TileType == TileType.Ground)
            return;

        tile.AddAdjacentTile(eastTile.Coordinates);
        if (eastTile.TileType == TileType.StartingPosition) 
            eastTile.AddAdjacentTile(tile.Coordinates);
    }

    private void AddSouthTile(Tile tile)
    {
        var southTileCoords = tile.SouthTile;
        if (string.IsNullOrWhiteSpace(southTileCoords)) 
            return;
        var southTile = TilesDictionary[southTileCoords];
        if (southTile.TileType == TileType.Ground)
            return;

        tile.AddAdjacentTile(southTile.Coordinates);
        if (southTile.TileType == TileType.StartingPosition) 
            southTile.AddAdjacentTile(tile.Coordinates);
    }

    private void AddWestTile(Tile tile)
    {
        var westTileCoords = tile.WestTile;
        if (string.IsNullOrWhiteSpace(westTileCoords)) 
            return;
        var westTile = TilesDictionary[westTileCoords];
        if (westTile.TileType == TileType.Ground)
            return;

        tile.AddAdjacentTile(westTile.Coordinates);
        if (westTile.TileType == TileType.StartingPosition) 
            westTile.AddAdjacentTile(tile.Coordinates);
    }

    private void PrintTilesDictionary()
    {
        var currentLine = 0;
        foreach (var (_, tile) in TilesDictionary)
        {
            if (tile.Coordinates.YCoordinate != currentLine)
            {
                Console.WriteLine();
                currentLine = tile.Coordinates.YCoordinate;
            }

            Console.Write(tile.Coordinates + Constants.Tab);
        }
    }
}