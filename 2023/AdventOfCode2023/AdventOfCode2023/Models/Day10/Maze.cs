using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> inputLines)
    {
        BuildTileDictionary(inputLines);
        CalculateAdjacentTiles();
        // FilterNoneLoopPipes(); // Still leaves small loops that can coexist with the main loop
        FilterNoneMainLoopPipes(); // Removes small loops that coexist with the main loop
        PrintMaze();
    }

    private Dictionary<Coordinates, Tile> _tileDictionary = new();
    private readonly Dictionary<Coordinates, Tile> _mainLoopTileDictionary = new();
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
                _tileDictionary.Add(tile.Coordinates, tile);
            }
        }
    }

    private void CalculateAdjacentTiles()
    {
        foreach (var (_, tile) in _tileDictionary)
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
        _tileDictionary = _tileDictionary.Where(t => t.Value.TileType != TileType.Ground).ToDictionary();
    }

    private void AddNorthTile(Tile tile)
    {
        var northTileCoords = tile.NorthTile;
        if (northTileCoords == null) 
            return;
        var northTile = _tileDictionary.FirstOrDefault(t => t.Key.Equals(northTileCoords)).Value;
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
        var eastTile = _tileDictionary.FirstOrDefault(t => t.Key.Equals(eastTileCoords)).Value;
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
        var southTile = _tileDictionary.FirstOrDefault(t => t.Key.Equals(southTileCoords)).Value;
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
        var westTile = _tileDictionary.FirstOrDefault(t => t.Key.Equals(westTileCoords)).Value;
        if (westTile == null)
            return;

        tile.AddAdjacentTile(westTile.Coordinates);
        if (westTile.TileType == TileType.StartingPosition) 
            westTile.AddAdjacentTile(tile.Coordinates);
    }

    private void FilterNoneLoopPipes()
    {
        var somethingChanged = false;
        foreach (var (_, tile) in _tileDictionary)
        {
            if (tile.TileType == TileType.StartingPosition)
                continue;
            var firstAdjacentTile = _tileDictionary.FirstOrDefault(t => t.Key.Equals(tile.AdjacentTiles[0])).Value;
            var secondAdjacentTile = _tileDictionary.FirstOrDefault(t => t.Key.Equals(tile.AdjacentTiles[1])).Value;

            if (tile.TileIsCoupled(firstAdjacentTile) && tile.TileIsCoupled(secondAdjacentTile)) 
                continue;
            tile.TileType = TileType.Ground;
            somethingChanged = true;
        }
        _tileDictionary = _tileDictionary.Where(t => t.Value.TileType != TileType.Ground).ToDictionary();
        if (somethingChanged)
            FilterNoneLoopPipes();
    }

    private void FilterNoneMainLoopPipes()
    {
        var firstTile = _tileDictionary.First(kvp => kvp.Value.TileType == TileType.StartingPosition).Value;
        _mainLoopTileDictionary.Add(firstTile.Coordinates, firstTile);
        var currentTile = firstTile;
        while (true)
        {
            var nextTile = _tileDictionary.First(t => t.Key.Equals(currentTile.AdjacentTiles[0])).Value;
            if(_mainLoopTileDictionary.ContainsKey(nextTile.Coordinates))
            {
                nextTile = _tileDictionary.First(t => t.Key.Equals(currentTile.AdjacentTiles[1])).Value;
                if (_mainLoopTileDictionary.ContainsKey(nextTile.Coordinates))
                    break;
            }

            _mainLoopTileDictionary.Add(nextTile.Coordinates, nextTile);

            currentTile = nextTile;
        }
    }

    private void PrintMaze()
    {
        for (var i = 0; i < _mazeLength; i++)
        {
            for (var j = 0; j < _mazeWidth; j++)
            {
                var coordToPrint = new Coordinates(j, i);
                var tile = _mainLoopTileDictionary.FirstOrDefault(t => t.Key.Equals(coordToPrint)).Value;
                var tileTypeChar = tile?.TileType.ToChar() ?? TileType.Ground.ToChar();

                if (tile == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(tileTypeChar);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }

    public int GetLoopLength()
    {
        return _mainLoopTileDictionary.Count;
    }
}