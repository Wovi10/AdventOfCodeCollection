using System.Net;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> inputLines)
    {
        BuildTileDictionary(inputLines);
        CalculateAdjacentTiles();
        FilterNoneMainLoopPipes(); // For Part two there is an issue with setting all non-loop items to ground. Only ground should be counted, so this number will be way bigger.
        // PrintMaze();
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
                if (tile.TileType == TileType.Ground)
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
                    tile.AddNorthTile(_tileDictionary);
                    tile.AddSouthTile(_tileDictionary);
                    break;
                case TileType.EastWest:
                    tile.AddEastTile(_tileDictionary);
                    tile.AddWestTile(_tileDictionary);
                    break;
                case TileType.NorthEast:
                    tile.AddNorthTile(_tileDictionary);
                    tile.AddEastTile(_tileDictionary);
                    break;
                case TileType.NorthWest:
                    tile.AddNorthTile(_tileDictionary);
                    tile.AddWestTile(_tileDictionary);
                    break;
                case TileType.SouthWest:
                    tile.AddSouthTile(_tileDictionary);
                    tile.AddWestTile(_tileDictionary);
                    break;
                case TileType.SouthEast:
                    tile.AddEastTile(_tileDictionary);
                    tile.AddSouthTile(_tileDictionary);
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

    private void FilterNoneMainLoopPipes()
    {
        var firstTile = _tileDictionary.First(kvp => kvp.Value.TileType == TileType.StartingPosition).Value;
        _mainLoopTileDictionary.Add(firstTile.Coordinates, firstTile);
        var currentTile = firstTile;
        while (true)
        {
            var nextTile = _tileDictionary.First(t => t.Key.Equals(currentTile.AdjacentTiles[0])).Value;
            if (_mainLoopTileDictionary.ContainsKey(nextTile.Coordinates))
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
                    Console.ForegroundColor = ConsoleColor.Red;

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

    public int CalculateEnclosedTiles()
    {
        var enclosedTiles = 0;
        for (var i = 0; i < _mazeLength; i++)
        {
            for (var j = 0; j < _mazeWidth; j++)
            {
                var coordToCheck = new Coordinates(i, j);
                if (_mainLoopTileDictionary.ContainsKey(coordToCheck))
                    continue;
                var directionToClosestEdge = FindClosestEdge(coordToCheck);
                var edgesCrossed = directionToClosestEdge switch
                {
                    Direction.North => CountEdgeCrosses(coordToCheck, Direction.North),
                    Direction.East => CountEdgeCrosses(coordToCheck, Direction.East),
                    Direction.South => CountEdgeCrosses(coordToCheck, Direction.South),
                    Direction.West => CountEdgeCrosses(coordToCheck, Direction.West),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (edgesCrossed % 2 != 0) 
                    enclosedTiles++;
            }
        }

        return enclosedTiles;
    }

    private int CountEdgeCrosses(Coordinates coordToCheck, Direction direction)
    {
        return direction switch
        {
            Direction.North => CountEdgeCrossesNorth(coordToCheck),
            Direction.East => CountEdgeCrossesEast(coordToCheck),
            Direction.South => CountEdgeCrossesSouth(coordToCheck),
            Direction.West => CountEdgeCrossesWest(coordToCheck),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private int CountEdgeCrossesNorth(Coordinates startCoordinates)
    {
        var edgesCrossed = 0;
        var xCoord = startCoordinates.GetXCoordinate();
        var lastWasWall = false;
        for (var i = startCoordinates.GetYCoordinate(); i > 0; i--)
        {
            var coordinateToCheck = new Coordinates(xCoord, i);
            var tileToCheck = _mainLoopTileDictionary.FirstOrDefault(t => t.Key.Equals(coordinateToCheck)).Value;
            var isWall = tileToCheck is {TileType: TileType.NorthSouth};

            if (tileToCheck != null && !lastWasWall && !isWall) 
                edgesCrossed++;
            lastWasWall = isWall;
        }

        return edgesCrossed;
    }

    private int CountEdgeCrossesEast(Coordinates startCoordinates)
    {
        var edgesCrossed = 0;
        var yCoord = startCoordinates.GetYCoordinate();
        var lastWasWall = false;
        for (var i = startCoordinates.GetXCoordinate(); i < _mazeWidth - 1; i++)
        {
            var coordinateToCheck = new Coordinates(i, yCoord);
            var tileToCheck = _mainLoopTileDictionary.FirstOrDefault(t => t.Key.Equals(coordinateToCheck)).Value;
            var isWall = tileToCheck is {TileType: TileType.EastWest};

            if (tileToCheck != null && !lastWasWall && !isWall) 
                edgesCrossed++;
            lastWasWall = isWall;
        }

        return edgesCrossed;
    }

    private int CountEdgeCrossesSouth(Coordinates startCoordinates)
    {
        var edgesCrossed = 0;
        var xCoord = startCoordinates.GetXCoordinate();
        var lastWasWall = false;
        for (var i = startCoordinates.GetYCoordinate(); i < _mazeLength - 1; i++)
        {
            var coordinateToCheck = new Coordinates(xCoord, i);
            var tileToCheck = _mainLoopTileDictionary.FirstOrDefault(t => t.Key.Equals(coordinateToCheck)).Value;
            var isWall = tileToCheck is {TileType: TileType.NorthSouth};

            if (tileToCheck != null && !lastWasWall && !isWall) 
                edgesCrossed++;
            lastWasWall = isWall;
        }

        return edgesCrossed;
    }

    private int CountEdgeCrossesWest(Coordinates startCoordinates)
    {
        var edgesCrossed = 0;
        var yCoord = startCoordinates.GetYCoordinate();
        var lastWasWall = false;
        for (var i = startCoordinates.GetXCoordinate(); i > 0; i--)
        {
            var coordinateToCheck = new Coordinates(i, yCoord);
            var tileToCheck = _mainLoopTileDictionary.FirstOrDefault(t => t.Key.Equals(coordinateToCheck)).Value;
            var isWall = tileToCheck is {TileType: TileType.EastWest};

            if (tileToCheck != null && !lastWasWall && !isWall)
                edgesCrossed++;
            lastWasWall = isWall;
        }

        return edgesCrossed;
    }

    private Direction FindClosestEdge(Coordinates coordinates)
    {
        var distanceNorth = coordinates.GetYCoordinate();
        var distanceEast = _mazeWidth - coordinates.GetXCoordinate();
        var distanceSouth = _mazeLength - coordinates.GetYCoordinate();
        var distanceWest = coordinates.GetXCoordinate();
        var lowest = MathUtils.GetLowest(distanceNorth, distanceSouth);
        lowest = MathUtils.GetLowest(lowest, distanceEast);
        lowest = MathUtils.GetLowest(lowest, distanceWest);

        if (lowest == distanceNorth)
            return Direction.North;

        if (lowest == distanceEast)
            return Direction.East;

        return lowest == distanceSouth
            ? Direction.South
            : Direction.West;
    }
}