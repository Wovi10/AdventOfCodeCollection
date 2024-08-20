using AdventOfCode2023_1.Models.Day23.TileTypes;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day23;

public class SnowIsland
{
    public SnowIsland(List<string> input)
    {
        Tiles = new List<Tile>();

        var y = 0;
        foreach (var inputLine in input)
        {
            var x = 0;
            foreach (var tile in inputLine)
            {
                Tiles.Add(new Tile(x, y, tile));
                x++;
            }

            y++;
        }

        StartTile = Tiles.First(t => t.Type is DefaultPath);
        EndTile = Tiles.Last(t => t.Type is DefaultPath);
    }

    private List<Tile> Tiles { get; set; }
    private Tile StartTile { get; set; }
    private Tile EndTile { get; set; }
    private int LongestHikeLength { get; set; }

    public int FindLongestHike()
    {
        RunOverHikes();
        return LongestHikeLength;
    }

    private void RunOverHikes()
    {
        var queue = SetupQueue();

        while (queue.Count > 0)
        {
            var currentHike = queue.Dequeue();

            var lastTile = currentHike.Tiles.Last();

            if (lastTile == EndTile)
            {
                var isLonger = currentHike.Length > LongestHikeLength;
                LongestHikeLength = isLonger ? currentHike.Length : LongestHikeLength;
                if (isLonger)
                {
                    Console.WriteLine($"New longest hike found: {currentHike.Length}");
                }
                continue;
            }

            var neighbourTiles = lastTile.GetPossibleNeighbourTiles(Tiles, currentHike);

            if (neighbourTiles.Count == 0)
                continue;

            neighbourTiles.ForEach(neighbourTile => EnqueueNewHike(currentHike, neighbourTile, queue));
        }
    }
 
    private Queue<Hike> SetupQueue()
    {
        var queue = new Queue<Hike>();
        queue.Enqueue(new Hike(StartTile));
        return queue;
    }

    private static void EnqueueNewHike(Hike currentHike, Tile neighbourTile, Queue<Hike> queue)
    {
        var newHike = new Hike(currentHike.Tiles);
        if (newHike.AddTile(neighbourTile))
            queue.Enqueue(newHike);
    }
}