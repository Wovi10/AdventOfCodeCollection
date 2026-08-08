using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day09() : DayBase("09", "Movie Theater")
{
    private const char Red = '#';
    private const char Green = 'X';

    protected override Task<object> PartOne()
    {
        var result = FindBiggestAreaRedTiles();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = FindBiggestAreaRedAndGreenTiles();

        return Task.FromResult<object>(result);
    }

    private long FindBiggestAreaRedTiles()
    {
        var input = GetInput().Select(l => l.Split(',').Select(long.Parse).ToArray()).ToArray();

        return GetAreas(input).OrderDescending().First();
    }

    private static List<long> GetAreas(long[][] input)
    {
        var result = new List<long>();
        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var (x1, y1) = (line[0], line[1]);

            for (var j = i + 1; j < input.Length; j++)
            {
                var otherLine = input[j];
                var (x2, y2) = (otherLine[0], otherLine[1]);
                var (biggestX, smallestX) = (Math.Max(x1, x2) + 1, Math.Min(x1, x2));
                var (biggestY, smallestY) = (Math.Max(y1, y2) + 1, Math.Min(y1, y2));

                var area = (biggestX - smallestX) * (biggestY - smallestY);
                result.Add(area);
            }
        }

        return result;
    }

    private long FindBiggestAreaRedAndGreenTiles()
    {
        var redTiles = GetInput()
            .Select(l => l.Split(',').Select(long.Parse).ToArray())
            .Select(a => (x: a[0], y: a[1]))
            .ToArray();

        var (sizeX, indexX) = CompressAxis(redTiles.Select(v => v.x));
        var (sizeY, indexY) = CompressAxis(redTiles.Select(v => v.y));
        Log.Info("Compressed {0} vertices into a {1}x{2} grid", redTiles.Length, sizeX, sizeY);

        var isBoundary = FindEdges(sizeX, sizeY, redTiles, indexX, indexY);
        var isOutside = MarkOutsideBreathFirst(isBoundary, sizeX, sizeY); // Less outside tiles than inside, so this is quicker

        var outsidePrefix = BuildPrefixSum(isOutside, sizeX, sizeY);

        var biggest = 0L;
        for (var i = 0; i < redTiles.Length; i++)
        {
            for (var j = i + 1; j < redTiles.Length; j++)
            {
                var (x1, y1) = redTiles[i];
                var (x2, y2) = redTiles[j];
                var (minX, maxX) = (Math.Min(x1, x2), Math.Max(x1, x2));
                var (minY, maxY) = (Math.Min(y1, y2), Math.Max(y1, y2));

                var outsideCount = RegionSum(outsidePrefix, indexX[minX], indexX[maxX], indexY[minY], indexY[maxY]);
                if (outsideCount != 0)
                    continue; // at least one cell in this rectangle is outside the shape - reject it

                var area = (maxX - minX + 1) * (maxY - minY + 1);
                biggest = Math.Max(area, biggest);
            }
        }

        Log.Info("FindBiggestAreaRedAndGreenTiles finished with area {0}", biggest);
        return biggest;
    }

    private static bool[,] FindEdges(int sizeX, int sizeY, (long x, long y)[] vertices, Dictionary<long, int> indexX,
        Dictionary<long, int> indexY)
    {
        var isBoundary = new bool[sizeX, sizeY];
        for (var i = 0; i < vertices.Length; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Length]; // List wraps: last tile connects back to the first
            MarkEdge(isBoundary, indexX[a.x], indexY[a.y], indexX[b.x], indexY[b.y]);
        }

        return isBoundary;
    }

    private static void MarkEdge(bool[,] isBoundary, int ax, int ay, int bx, int by)
    {
        if (ax == bx) // same column
            for (var y = Math.Min(ay, by); y <= Math.Max(ay, by); y++)
                isBoundary[ax, y] = true;
        else // same row
            for (var x = Math.Min(ax, bx); x <= Math.Max(ax, bx); x++)
                isBoundary[x, ay] = true;
    }

    private static (int size, Dictionary<long, int> index) CompressAxis(IEnumerable<long> vertexValues)
    {
        var distinct = vertexValues.Distinct().Order().ToArray();
        var index = new Dictionary<long, int>();
        var nextSlot = 1;

        for (var i = 0; i < distinct.Length; i++)
        {
            index[distinct[i]] = nextSlot++;

            if (i + 1 < distinct.Length && distinct[i + 1] > distinct[i] + 1)
                nextSlot++;
        }

        return (nextSlot + 1, index);
    }

    private static bool[,] MarkOutsideBreathFirst(bool[,] isBoundary, int sizeX, int sizeY)
    {
        var isOutside = new bool[sizeX, sizeY];
        var queue = new Queue<(int x, int y)>();

        isOutside[0, 0] = true;
        queue.Enqueue((0, 0));

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            foreach (var (nx, ny) in new[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) })
            {
                if (FellOffTheEdge(nx, ny) || VisitedOrWall(nx, ny))
                    continue;

                isOutside[nx, ny] = true;
                queue.Enqueue((nx, ny));
            }
        }

        return isOutside;

        bool VisitedOrWall(int nx, int ny) => isOutside[nx, ny] || isBoundary[nx, ny];
        bool FellOffTheEdge(int nx, int ny) => nx < 0 || nx >= sizeX || ny < 0 || ny >= sizeY;
    }

    private static int[,] BuildPrefixSum(bool[,] isOutside, int sizeX, int sizeY)
    {
        var prefix = new int[sizeX + 1, sizeY + 1];

        for (var x = 0; x < sizeX; x++)
            for (var y = 0; y < sizeY; y++)
                prefix[x + 1, y + 1] = (isOutside[x, y] ? 1 : 0) + prefix[x, y + 1] + prefix[x + 1, y] - prefix[x, y];

        return prefix;
    }

    private static int RegionSum(int[,] prefix, int loX, int hiX, int loY, int hiY)
        => prefix[hiX + 1, hiY + 1] - prefix[loX, hiY + 1] - prefix[hiX + 1, loY] + prefix[loX, loY];
}
