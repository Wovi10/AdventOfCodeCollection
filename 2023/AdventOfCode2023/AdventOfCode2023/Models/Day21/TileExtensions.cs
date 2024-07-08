using AdventOfCode2023_1.Models.Day21.Enums;
using UtilsCSharp;
using UtilsCSharp.Enums;

namespace AdventOfCode2023_1.Models.Day21;

public static class TileExtensions
{
    private static List<Tile> AllTiles { get; set; } = new();

    public static void StepNonRecursive(this Tile currentTile, int numberOfSteps,
        List<(int, int)> reachableTiles, List<Tile>? allTiles = null)
    {
        var tilesToProcess = new Queue<(Tile tile, int stepCounter)>();
        var visitedTiles = new HashSet<int>();
        tilesToProcess.Enqueue((currentTile, 0));

        var highestStepCounter = 0;

        if (AllTiles.Count == 0 && allTiles != null)
            AllTiles = allTiles;

        while (tilesToProcess.Count > 0)
        {
            var (tile, stepCounter) = tilesToProcess.Dequeue();

            if (stepCounter.IsEven())
                reachableTiles.Add((tile.ActualX, tile.ActualY));

            if (stepCounter >= numberOfSteps)
                continue;

            highestStepCounter = Comparisons.GetHighest(stepCounter, highestStepCounter);

            var walkableNeighbourTiles = GetWalkableNeighbourTiles(tile);

            foreach (var neighbourTile in walkableNeighbourTiles)
            {
                var nextStepCounter = stepCounter + 1;
                var neighbourHashCode = neighbourTile.GetHashCode(nextStepCounter);

                if (!visitedTiles.Add(neighbourHashCode))
                    continue;

                tilesToProcess.Enqueue((neighbourTile, nextStepCounter));
            }
        }

        Console.WriteLine($"Highest step counter: {highestStepCounter}");
    }

    private static List<Tile> GetWalkableNeighbourTiles(this Tile currentTile)
    {
        var neighbourTiles = new List<Tile>();

        foreach (var direction in Enum.GetValues<Direction>())
        {
            if (direction == Direction.None)
                continue;

            var (x, y) = currentTile.Move(direction);
            var (newX, newY) = (x, y);

            if (newX < 0)
                newX += Garden.Width - 1;
            else if (newX >= Garden.Width)
                newX -= Garden.Width;

            if (newY < 0)
                newY += Garden.Height - 1;
            else if (newY >= Garden.Height)
                newY -= Garden.Height;

            var neighbourTile = AllTiles.FirstOrDefault(t => t.X == newX && t.Y == newY);

            if (neighbourTile is not {IsWalkable: true})
                continue;

            var newTile = new Tile(newX, newY, neighbourTile.Type.ToTileChar());
            neighbourTiles.Add(newTile);
        }

        return neighbourTiles;
    }

    private static int GetHashCode(this Tile tile, int stepCounter)
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(tile.ActualX);
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(tile.ActualY);
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(stepCounter);
            return hash;
        }
    }
}