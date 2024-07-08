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
        var tilesToProcess = new Queue<Tile>();
        var visitedTiles = new HashSet<int>();
        tilesToProcess.Enqueue(currentTile);

        var highestStepCounter = 0;

        if (AllTiles.Count == 0 && allTiles != null)
            AllTiles = allTiles;

        while (tilesToProcess.Count > 0)
        {
            var tile = tilesToProcess.Dequeue();

            if (tile.StepCounter.IsEven())
                reachableTiles.Add((tile.X, tile.Y));

            if (tile.StepCounter >= numberOfSteps)
                continue;

            highestStepCounter = Comparisons.GetHighest(tile.StepCounter, highestStepCounter);

            var walkableNeighbourTiles = GetWalkableNeighbourTiles(tile);

            foreach (var neighbourTile in walkableNeighbourTiles)
            {
                var nextStepCounter = tile.StepCounter + 1;
                neighbourTile.StepCounter = nextStepCounter;
                var neighbourHashCode = neighbourTile.GetHashCode();

                if (!visitedTiles.Add(neighbourHashCode))
                    continue;

                tilesToProcess.Enqueue(neighbourTile);
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
}