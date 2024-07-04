using System.Collections.Concurrent;
using UtilsCSharp;
using UtilsCSharp.Enums;

namespace AdventOfCode2023_1.Models.Day21;

public static class TileExtensions
{
    private static List<Tile> AllTiles { get; set; } = new();

    public static async Task Step(this Tile currentTile, int stepCounter, ConcurrentBag<Tile> reachableTiles,
        int numberOfSteps, ConcurrentBag<int> visitedTiles, List<Tile>? allTiles = null)
    {
        if (stepCounter == numberOfSteps)
        {
            reachableTiles.Add(currentTile);
            return;
        }

        visitedTiles.Add(currentTile.GetHashCode(stepCounter));
        if (stepCounter.IsEven())
            reachableTiles.Add(currentTile);

        if (AllTiles.Count == 0 && allTiles != null)
            AllTiles = allTiles;

        stepCounter++;

        var walkableNeighbourTiles = GetWalkableNeighbourTiles(currentTile);
        var neighbourTiles = walkableNeighbourTiles.Where(tile => !visitedTiles.Contains(tile
            .GetHashCode(stepCounter+1)))
            .ToList();

        foreach (var neighbourTile in neighbourTiles)
            await neighbourTile.Step(stepCounter, reachableTiles, numberOfSteps, visitedTiles);
    }

    private static List<Tile> GetWalkableNeighbourTiles(this Tile currentTile)
    {
        var neighbourTiles = new List<Tile>();

        foreach (var direction in Enum.GetValues<Direction>())
        {
            if (direction == Direction.None)
                continue;

            var (x, y) = currentTile.Move(direction);

            var neighbourTile = AllTiles.FirstOrDefault(t => t.X == x && t.Y == y);

            if (neighbourTile is {IsWalkable: true})
                neighbourTiles.Add(neighbourTile);
        }

        return neighbourTiles;
    }
    
    public static int GetHashCode(this Tile tile, int counter)
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(tile.X);
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(tile.Y);
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(counter);
            return hash;
        }
    }
}