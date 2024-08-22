using AdventOfCode2023_1.Models.Day23.TileTypes;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day23;

public class SnowIsland
{
    public SnowIsland(List<string> input)
    {
        Tiles = new List<Tile>();

        Height = input.Count;
        var y = 0;
        foreach (var inputLine in input)
        {
            Width = inputLine.Length;
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
    private int Height { get; set; }
    private int Width { get; set; }

    public int DoItAlternativeWay()
    {
        var corners = FindCorners();
        var segments = FindSegmentsBetweenCorners(corners);

        return 0;
    }

    private Dictionary<(int, int, int, int), int> FindSegmentsBetweenCorners(HashSet<(int, int)> corners)
    {
        var pathSegments = new Dictionary<(int, int, int, int), int>();

        foreach (var (cornerX, cornerY) in corners.Except([(Height-1, Width-2)]))
        {
            
        }
    }

    private HashSet<(int,int)> FindCorners()
    {
        var corners = new HashSet<(int, int)>();

        for (var y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var tile = Tiles.First(t => t.X == x && t.Y == y);

                if (tile.Type is Forest || tile == StartTile || tile == EndTile)
                    continue;

                var count = 0;
                var above = Tiles.First(t => t.X == x && t.Y == y - 1);
                var left = Tiles.First(t => t.X == x - 1 && t.Y == y);
                var right = Tiles.First(t => t.X == x + 1 && t.Y == y);
                var below = Tiles.First(t => t.X == x && t.Y == y + 1);
                
                if (above.Type is not Forest && left.Type is not Forest)
                    count++;
                if (above.Type is not Forest && right.Type is not Forest)
                    count++;
                if (below.Type is not Forest && left.Type is not Forest)
                    count++;
                if (below.Type is not Forest && right.Type is not Forest)
                    count++;

                if (count > 1)
                {
                    corners.Add((tile.X, tile.Y));
                }
            }
        }

        corners.Add((StartTile.X, StartTile.Y));
        corners.Add((EndTile.X, EndTile.Y));

        return corners;
    }

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

    private static void EnqueueNewHike(Hike currentHike, Tile tileToAdd, Queue<Hike> queue)
    {
        var newHike = new Hike(currentHike.Tiles);
        if (newHike.AddTile(tileToAdd))
            queue.Enqueue(newHike);
    }
}