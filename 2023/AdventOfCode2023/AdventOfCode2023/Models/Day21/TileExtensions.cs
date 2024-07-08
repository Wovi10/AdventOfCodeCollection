using UtilsCSharp;
using UtilsCSharp.Enums;

namespace AdventOfCode2023_1.Models.Day21;

public static class TileExtensions
{
    private static List<Tile> AllTiles { get; set; } = new();

    public static void StepNonRecursive(this Tile currentTile, int numberOfSteps,
        List<Tile> reachableTiles, List<Tile>? allTiles = null)
    {
        var tilesToProcess = new Queue<(Tile tile, int stepCounter)>();
        var minStepCounts = new Dictionary<Tile, int>();
        tilesToProcess.Enqueue((currentTile, 0));
        minStepCounts[currentTile] = 0;

        if (AllTiles.Count == 0 && allTiles != null)
            AllTiles = allTiles;

        while (tilesToProcess.Count > 0)
        {
            var (tile, stepCounter) = tilesToProcess.Dequeue();

            if (stepCounter.IsEven())
                reachableTiles.Add(tile);

            if (stepCounter >= numberOfSteps) 
                continue;

            var walkableNeighbourTiles = GetWalkableNeighbourTiles(tile);
            foreach (var neighbourTile in walkableNeighbourTiles)
            {
                var nextStepCounter = stepCounter + 1;

                if (minStepCounts.TryGetValue(neighbourTile, out var existingStepCount) &&
                    nextStepCounter >= existingStepCount) 
                    continue;

                minStepCounts[neighbourTile] = nextStepCounter;
                tilesToProcess.Enqueue((neighbourTile, nextStepCounter));
            }
        }
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