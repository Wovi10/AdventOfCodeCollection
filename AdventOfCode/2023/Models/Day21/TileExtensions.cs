using _2023.Models.Day21.Enums;
using UtilsCSharp;
using UtilsCSharp.Enums;

namespace _2023.Models.Day21;

public static class TileExtensions
{
    private static Dictionary<(int, int), Tile> AllTiles { get; set; } = new();

    public static void Step(this Tile startTile, int numberOfSteps,
        List<(int, int)> reachableTiles, Dictionary<(int X, int Y), Tile> allTiles)
    {
        var tilesToProcess = new Queue<Tile>();
        var visitedTiles = new Dictionary<(int, int), int>();
        var numberOfStepsIsEven = numberOfSteps.IsEven();

        tilesToProcess.Enqueue(startTile);

        if (AllTiles.Count == 0)
            AllTiles = allTiles;

        while (tilesToProcess.Count > 0)
        {
            var tile = tilesToProcess.Dequeue();

            if (tile.StepCounter.IsEven() == numberOfStepsIsEven)
                reachableTiles.Add((tile.ActualX, tile.ActualY));

            if (tile.StepCounter >= numberOfSteps)
                continue;

            var walkableNeighbourTiles = GetWalkableNeighbourTiles(tile);

            foreach (var neighbourTile in walkableNeighbourTiles)
            {
                neighbourTile.StepCounter = tile.StepCounter + 1;

                var dictionaryKey = (neighbourTile.ActualX, neighbourTile.ActualY);
                var exists = visitedTiles.TryGetValue(dictionaryKey, out var dictionaryEntry);

                if (exists && dictionaryEntry <= neighbourTile.StepCounter)
                    continue;

                visitedTiles[dictionaryKey] = neighbourTile.StepCounter;

                tilesToProcess.Enqueue(neighbourTile);
            }
        }
    }

    private static List<Tile> GetWalkableNeighbourTiles(this Tile currentTile)
    {
        var neighbourTiles = new List<Tile>();

        foreach (var direction in Enum.GetValues<Direction>())
        {
            if (direction is Direction.None)
                continue;

            var (actualX, actualY) = currentTile.Move(direction);

            if (actualX > Garden.StartingTile.ActualX || actualY > Garden.StartingTile.ActualY)
                continue;

            var (newX, newY) = ((actualX % Garden.Width + Garden.Width) % Garden.Width,
                (actualY % Garden.Height + Garden.Height) % Garden.Height);


            var neighbourTile = AllTiles.FirstOrDefault(t => t.Key.Item1 == newX && t.Key.Item2 == newY).Value;

            if (neighbourTile is not {IsWalkable: true})
                continue;

            var newTile = new Tile(newX, newY, neighbourTile.Type.ToTileChar())
            {
                ActualX = actualX,
                ActualY = actualY
            };

            neighbourTiles.Add(newTile);
        }

        return neighbourTiles;
    }
}