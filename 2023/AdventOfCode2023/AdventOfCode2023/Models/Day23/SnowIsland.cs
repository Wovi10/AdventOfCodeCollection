using System.Collections.Concurrent;
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

    public async Task<int> DoItAlternativeWay()
    {
        var corners = FindCorners();
        var hikeSegments = await FindHikeSegmentsAsync(corners);
        return FindCompletePaths(hikeSegments);

        // return completePaths.Max(segments => segments.Sum(segment => segment.Length));
    }

    private int FindCompletePaths(HashSet<HikeSegment> hikeSegments)
    {
        var allCompletePaths = new List<List<HikeSegment>>();
        var startSegment = hikeSegments.Where(segment => segment.CornerOne == (StartTile.X, StartTile.Y)).ToList()
            .First();
        var endSegment = hikeSegments.Where(segment => segment.CornerTwo == (EndTile.X, EndTile.Y)).ToList().First();

        var queue = new Queue<(List<HikeSegment>, (int, int))>();
        queue.Enqueue((new List<HikeSegment> {startSegment}, startSegment.CornerTwo));

        var longestPathLength = 0;

        while (queue.Count > 0)
        {
            var (currentPath, tail) = queue.Dequeue();

            if (currentPath.Count(segment => segment.CornerOne == tail || segment.CornerTwo == tail) > 1)
                continue;

            var lastSegment = currentPath.Last();

            var nextSegments =
                hikeSegments
                    .Where(segment =>
                        tail == segment.CornerOne ||
                        tail == segment.CornerTwo)
                    .Where(segment => !segment.Equals(lastSegment) && !currentPath.Contains(segment))
                    .ToList();

            nextSegments.ForEach(nextSegment =>
            {
                var newTail = tail == nextSegment.CornerOne ? nextSegment.CornerTwo : nextSegment.CornerOne;
                var newPath = new List<HikeSegment>(currentPath) {nextSegment};

                if (!nextSegment.Equals(endSegment))
                {
                    queue.Enqueue((newPath, newTail));
                    return;
                }

                longestPathLength = Comparisons.GetHighest(longestPathLength, newPath.Sum(segment => segment.Length));
            });
        }

        return longestPathLength;
    }

    private HashSet<HikeSegment> FindHikeSegments(HashSet<(int, int)> corners)
    {
        var pathSegments = new HashSet<HikeSegment>();

        foreach (var corner in corners)
        {
            var excludingCurrentCorner = corners.Where(otherCorner => otherCorner != corner).ToHashSet();
            foreach (var otherCorner in excludingCurrentCorner)
            {
                var hikeSegmentToAdd = new HikeSegment {CornerOne = corner};
                var path = FindPathBetweenCorners(corner, otherCorner, corners);
                if (path == null)
                    continue;

                hikeSegmentToAdd.SetCornerTwo(otherCorner);
                hikeSegmentToAdd.Length = path.Count - 1;
                pathSegments.Add(hikeSegmentToAdd);
            }
        }

        return pathSegments;
    }

    private async Task<HashSet<HikeSegment>> FindHikeSegmentsAsync(HashSet<(int, int)> corners)
    {
        var pathSegments = new ConcurrentBag<HikeSegment>();

        var tasks = corners.Select(async corner =>
        {
            var excludingCurrentCorner = corners.Where(otherCorner => otherCorner != corner).ToHashSet();
            foreach (var otherCorner in excludingCurrentCorner)
            {
                var hikeSegmentToAdd = new HikeSegment {CornerOne = corner};
                var path = await Task.Run(() => FindPathBetweenCorners(corner, otherCorner, corners));
                if (path == null)
                    continue;

                hikeSegmentToAdd.SetCornerTwo(otherCorner);
                hikeSegmentToAdd.Length = path.Count - 1;
                pathSegments.Add(hikeSegmentToAdd);
            }
        });

        await Task.WhenAll(tasks);

        return new HashSet<HikeSegment>(pathSegments);
    }

    private List<Tile>? FindPathBetweenCorners((int, int) startCorner, (int, int) endCorner,
        HashSet<(int, int)> corners)
    {
        var startTile = Tiles.First(t => (t.X, t.Y) == startCorner);
        var targetTile = Tiles.First(t => (t.X, t.Y) == endCorner);
        var queue = new Queue<(Tile currentTile, List<Tile> path)>();
        var visited = new HashSet<Tile> {startTile};

        queue.Enqueue((startTile, new List<Tile> {startTile}));

        while (queue.Count > 0)
        {
            var (currentTile, path) = queue.Dequeue();

            if (currentTile == targetTile)
                return path;

            var neighbourTiles =
                currentTile.GetNeighbourTilesExcludingPrevious(Tiles, path.Count > 1 ? path[^2] : currentTile);

            foreach (var neighbourTile in neighbourTiles.Where(neighbourTile => !visited.Contains(neighbourTile) &&
                         (!corners.Contains((neighbourTile.X, neighbourTile.Y)) || neighbourTile == targetTile)))
            {
                visited.Add(neighbourTile);
                var newPath = new List<Tile>(path) {neighbourTile};
                queue.Enqueue((neighbourTile, newPath));
            }
        }

        return null;
    }

    private HashSet<(int, int)> FindCorners()
    {
        var corners = new HashSet<(int, int)>();

        foreach (var tile in Tiles.Where(tile => tile.Type is not Forest))
        {
            var neighbourTiles = tile.GetNeighbourTiles(Tiles);
            if (neighbourTiles.Count > 2)
                corners.Add((tile.X, tile.Y));
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