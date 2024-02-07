using AdventOfCode2023_1.Models.Day10;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day10 : DayBase
{
    private Dictionary<string, Tile> _tilesDictionary;
    protected override void PartOne()
    {
        var result = CalculateFurthestPosition();
        SharedMethods.AnswerPart(result);
    }

    protected override void PartTwo()
    {
        throw new NotImplementedException();
    }

    private int CalculateFurthestPosition()
    {
        var maze = new Maze(Input);
        _tilesDictionary = maze.TilesDictionary;
        var startingTile = _tilesDictionary.First(x => x.Value.TileType == TileType.StartingPosition).Value;
        var pathTried = new List<string>();
        pathTried = TryAllPaths(startingTile, pathTried);

        var highestDistance = _tilesDictionary.Max(x => x.Value.DistanceFromStart);
        return highestDistance;
    }

    private List<string> TryAllPaths(Tile startingTile, List<string> pathTried)
    {
        var startingTileCoordinates = startingTile.Coordinates.ToString();
        foreach (var adjacentTile in startingTile.AdjacentTiles)
        {
            pathTried.Clear();
            pathTried.Add(startingTileCoordinates);
            CalculateDistances(adjacentTile, startingTileCoordinates);
            foreach (var (_, value) in _tilesDictionary) 
                value.TriedEverything = false;
        }

        return pathTried;
    }

    private void CalculateDistances(string adjacentTile, string previousTile, int currentDistance = 0)
    {
        var tile = _tilesDictionary[adjacentTile];
        if (tile.TriedEverything)
            return;

        tile.SetDistanceFromStart(++currentDistance);

        if (tile.AdjacentTiles[0] != previousTile)
            CalculateDistances(tile.AdjacentTiles[0], adjacentTile, currentDistance);
        else if(tile.AdjacentTiles[1] != previousTile) 
            CalculateDistances(tile.AdjacentTiles[0], adjacentTile, currentDistance);

        tile.TriedEverything = true;
    }
}