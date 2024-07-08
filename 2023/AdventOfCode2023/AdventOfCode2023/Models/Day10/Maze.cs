using AdventOfCode2023_1.Models.Day10.Enums;
using AdventOfCode2023_1.Shared;
using UtilsCSharp;
using UtilsCSharp.Enums;

namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> inputLines)
    {
        BuildTileDictionary(inputLines);
        CalculateAdjacentTiles();
        FilterNoneMainLoopPipes();
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

        var startingTile = _tileDictionary.First(t => t.Value.IsStartingPosition).Value;
        startingTile.TileType = startingTile switch
        {
            {NorthTile: not null, SouthTile: not null} => TileType.NorthSouth,
            {NorthTile: not null, EastTile: not null} => TileType.NorthEast,
            {NorthTile: not null, WestTile: not null} => TileType.NorthWest,
            {EastTile: not null, SouthTile: not null} => TileType.SouthEast,
            {EastTile: not null, WestTile: not null} => TileType.EastWest,
            {SouthTile: not null, WestTile: not null} => TileType.SouthWest,
            _ => startingTile.TileType
        };
    }

    private void FilterNoneMainLoopPipes()
    {
        var firstTile = _tileDictionary.First(kvp => kvp.Value.IsStartingPosition).Value;
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

    public int GetLoopLength()
        => _mainLoopTileDictionary.Count;

    public int CalculateEnclosedTiles()
    {
        var enclosedTiles = 0;
        for (var i = 0; i < _mazeLength; i++)
        {
            for (var j = 0; j < _mazeWidth; j++)
            {
                var coordToCheck = new Coordinates(j, i);
                if (_mainLoopTileDictionary.ContainsKey(coordToCheck))
                    continue;

                var directionToClosestEdge = FindClosestEdge(coordToCheck);
                var edgesCrossed = directionToClosestEdge switch
                {
                    WindDirection.North => CountEdgeCrossesNorth(coordToCheck),
                    WindDirection.East => CountEdgeCrossesEast(coordToCheck),
                    WindDirection.South => CountEdgeCrossesSouth(coordToCheck),
                    WindDirection.West => CountEdgeCrossesWest(coordToCheck),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (edgesCrossed % 2 != 0)
                    enclosedTiles++;
            }
        }

        return enclosedTiles;
    }

    private WindDirection FindClosestEdge(Coordinates coordinates)
    {
        var distanceNorth = coordinates.Y;
        var distanceEast = _mazeWidth - coordinates.X;
        var distanceSouth = _mazeLength - coordinates.Y;
        var distanceWest = coordinates.X;
        var lowest = GetLowest(distanceNorth, distanceSouth);
        lowest = GetLowest(lowest, distanceEast);
        lowest = GetLowest(lowest, distanceWest);

        if (lowest == distanceNorth)
            return WindDirection.North;

        if (lowest == distanceEast)
            return WindDirection.East;

        return lowest == distanceSouth
            ? WindDirection.South
            : WindDirection.West;
    }

    private int CountEdgeCrossesNorth(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.Y - 1;
        const int endPoint = 0;
        var constantCoordinatePart = startCoordinates.X;

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, Constants.YAxis);
    }

    private int CountEdgeCrossesEast(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.X + 1;
        var endPoint = _mazeWidth;
        var constantCoordinatePart = startCoordinates.Y;

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, Constants.XAxis);
    }

    private int CountEdgeCrossesSouth(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.Y + 1;
        var endPoint = _mazeLength;
        var constantCoordinatePart = startCoordinates.X;

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, Constants.YAxis);
    }

    private int CountEdgeCrossesWest(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.X - 1;
        const int endPoint = 0;
        var constantCoordinatePart = startCoordinates.Y;

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, Constants.XAxis);
    }

    private int CountEdgesCrossed(int startPoint, int endPoint, int constantCoordinatePart, string workingAxis)
    {
        var isOnYAxis = workingAxis == Constants.YAxis;
        var edgesCrossed = 0;

        TileType tileTypeToSkip;
        TileType tileTypeToCheck;
        switch (isOnYAxis)
        {
            case true:
                tileTypeToSkip = TileType.NorthSouth;
                tileTypeToCheck = TileType.EastWest;
                break;
            case false:
                tileTypeToSkip = TileType.EastWest;
                tileTypeToCheck = TileType.NorthSouth;
                break;
        }

        var isFirstPartOfWall = true;
        var firstInWall = TileType.Ground;
        var decrement = endPoint == 0;

        for (var i = startPoint; ShouldStop(i, endPoint);)
        {
            var coordinateToCheck = isOnYAxis
                ? new Coordinates(constantCoordinatePart, i)
                : new Coordinates(i, constantCoordinatePart);

            if (decrement) i--;
            else i++;

            var tileToCheck = _mainLoopTileDictionary.FirstOrDefault(t => t.Key.Equals(coordinateToCheck)).Value;

            if (tileToCheck == null)
            {
                isFirstPartOfWall = true;
                continue;
            }

            if (tileToCheck.TileType == tileTypeToCheck)
            {
                edgesCrossed++;
                isFirstPartOfWall = true;
                continue;
            }

            if (isFirstPartOfWall)
            {
                edgesCrossed++;
                firstInWall = tileToCheck.TileType;
                isFirstPartOfWall = false;
                continue;
            }

            if (tileToCheck.TileType == tileTypeToSkip)
                continue;

            if (tileToCheck.TileType.IsOpposite(firstInWall))
            {
                isFirstPartOfWall = true;
                continue;
            }

            edgesCrossed--; // Just ran on top of the wall, did not cross it.
            isFirstPartOfWall = true;
        }

        return edgesCrossed;
    }

    private static bool ShouldStop(int index, int endPoint)
        => endPoint == 0 ? index >= endPoint : index < endPoint;
    
    private static int GetLowest(int first, int second)
        => Comparisons.GetLowest(first, second);
}