using AdventOfCode2023_1.Models.Day10;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day10 : DayBase
{
    // private Dictionary<string, Tile> _tilesDictionary;
    private List<Tile> _tilesList;
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
        _tilesList = maze.TilesList;
        // _tilesDictionary = maze.TilesDictionary;
        var startingTile = _tilesList.First(x => x.TileType == TileType.StartingPosition);
        TryAllPaths();

        var highestDistance = _tilesList.Max(x => x.DistanceFromStart);
        return highestDistance;
    }

    private void TryAllPaths()
    {
        foreach (var tile in _tilesList)
        {
            var distanceToStart = CalculateDistanceToStart(tile);
        }
    }

    private int CalculateDistanceToStart(Tile tile, Coordinates? previousTile = null)
    {
        var adjacentTiles = tile.AdjacentTiles;
        previousTile ??= tile.Coordinates;
        foreach (var adjacentTile in adjacentTiles)
        {
            if (adjacentTile.Equals(previousTile))
                continue;
            tile.DistanceFromStart++;
            var nextTile = _tilesList.FirstOrDefault(t =>  adjacentTile.Equals(t.Coordinates));
            if (nextTile == null)
                continue;
            CalculateDistanceToStart(nextTile, tile.Coordinates);
        }

        return tile.DistanceFromStart;
    }

    private void CalculateDistances(Coordinates adjacentTile, Coordinates previousTile, int currentDistance = 0)
    {
        var tile = _tilesList.First(x => x.Coordinates == adjacentTile);
        if (tile.TriedEverything)
            return;

        tile.SetDistanceFromStart(++currentDistance);

        if (tile.AdjacentTiles[0] != previousTile)
            CalculateDistances(tile.AdjacentTiles[0], adjacentTile, currentDistance);
        if(tile.AdjacentTiles[1] != previousTile) 
            CalculateDistances(tile.AdjacentTiles[1], adjacentTile, currentDistance);

        tile.TriedEverything = true;
    }
}