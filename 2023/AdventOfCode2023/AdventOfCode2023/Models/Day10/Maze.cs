using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day10;

public class Maze
{
    public Maze(List<string> inputLines)
    {
        BuildTileDictionary(inputLines);
        CalculateAdjacentTiles();
        FilterNoneMainLoopPipes();
        // PrintMaze();
    }

    private Dictionary<Coordinates, Tile> _tileDictionary = new();
    private readonly Dictionary<Coordinates, Tile> _mainLoopTileDictionary = new();
    private int _mazeWidth;
    private int _mazeLength;
    private const string YAxis = "y";
    private const string XAxis = "x";

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
                var coordToCheck = new Coordinates(j, i);
                if (_mainLoopTileDictionary.ContainsKey(coordToCheck))
                    continue;

                var directionToClosestEdge = FindClosestEdge(coordToCheck);
                var edgesCrossed = directionToClosestEdge switch
                {
                    Direction.North => CountEdgeCrossesNorth(coordToCheck),
                    Direction.East => CountEdgeCrossesEast(coordToCheck),
                    Direction.South => CountEdgeCrossesSouth(coordToCheck),
                    Direction.West => CountEdgeCrossesWest(coordToCheck),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (edgesCrossed % 2 != 0)
                    enclosedTiles++;
            }
        }

        return enclosedTiles;
    }

    private int CountEdgeCrossesNorth(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.GetYCoordinate() - 1;
        const int endPoint = 0;
        var constantCoordinatePart = startCoordinates.GetXCoordinate();

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, YAxis);
    }

    private int CountEdgeCrossesEast(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.GetXCoordinate() + 1;
        var endPoint = _mazeWidth;
        var constantCoordinatePart = startCoordinates.GetYCoordinate();

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, XAxis);
    }

    private int CountEdgeCrossesSouth(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.GetYCoordinate() + 1;
        var endPoint = _mazeLength;
        var constantCoordinatePart = startCoordinates.GetXCoordinate();

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, YAxis);
    }

    private int CountEdgeCrossesWest(Coordinates startCoordinates)
    {
        var startPoint = startCoordinates.GetXCoordinate() - 1;
        const int endPoint = 0;
        var constantCoordinatePart = startCoordinates.GetYCoordinate();

        return CountEdgesCrossed(startPoint, endPoint, constantCoordinatePart, XAxis);
    }

    private int CountEdgesCrossed(int startPoint, int endPoint, int constantCoordinatePart, string workingAxis)
    {
        var isOnYAxis = workingAxis == YAxis;
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
    {
        return endPoint == 0 ? index >= endPoint : index < endPoint;
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