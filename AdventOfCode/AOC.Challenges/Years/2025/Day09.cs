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

    // ── THE PROBLEM WITH THE OBVIOUS APPROACH ──────────────────────────────────────────
    // Part 2 asks: of all the red+green tiles, what's the biggest rectangle you can draw
    // using two red tiles as opposite corners? The straightforward way to answer that is
    // to build a grid, colour in every red and green tile, and check each candidate
    // rectangle cell by cell.
    //
    // That works fine on the tiny example (a ~12x9 grid), but the real puzzle input's red
    // tiles span roughly 98,000 x 98,000. A grid that size would need ~9.6 BILLION cells -
    // that's the "hogs too much memory" problem. We never actually need that many cells
    // though: with only ~500 red tiles, the shape only has ~500 "interesting" spots. Huge
    // stretches of empty or filled space between them all behave identically. The four
    // helper methods below (CompressAxis, MarkEdge, FloodFillOutside, BuildPrefixSum/
    // RegionSum) are all in service of shrinking that 9.6-billion-cell grid down to
    // roughly 990x990 (~1 million cells) without losing any information we actually need.
    private long FindBiggestAreaRedAndGreenTiles()
    {
        // Step 1: read the red tiles in the order they appear in the input. Order matters
        // here - consecutive tiles in the list are connected by a straight green line, and
        // the last tile connects back to the first, forming one big closed loop (a
        // "polygon" made only of horizontal and vertical lines).
        var vertices = GetInput()
            .Select(l => l.Split(',').Select(long.Parse).ToArray())
            .Select(a => (x: a[0], y: a[1]))
            .ToArray();

        // Step 2: shrink the huge coordinate space down to a small one. See CompressAxis
        // for how this works. After this, indexX/indexY let us translate a real x or y
        // coordinate (which could be in the tens of thousands) into a small grid index
        // (0..~990) that we can actually afford to allocate an array over.
        var (sizeX, indexX) = CompressAxis(vertices.Select(v => v.x));
        var (sizeY, indexY) = CompressAxis(vertices.Select(v => v.y));
        Log.Info("Compressed {0} vertices into a {1}x{2} grid", vertices.Length, sizeX, sizeY);

        // Step 3: draw the outline of the shape onto the small grid. isBoundary[x, y] is
        // true wherever a red tile or a straight green connector line passes through.
        var isBoundary = new bool[sizeX, sizeY];
        for (var i = 0; i < vertices.Length; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Length]; // list wraps: last tile connects back to the first
            MarkEdge(isBoundary, indexX[a.x], indexY[a.y], indexX[b.x], indexY[b.y]);
        }

        PrintField(vertices, indexX, indexY, isBoundary, null, sizeX, sizeY);

        // Step 4: now that we have the outline, figure out which of the remaining cells
        // are "outside" the shape entirely (as opposed to being in the filled-in interior,
        // which also counts as green). See FloodFillOutside for how.
        var isOutside = FloodFillOutside(isBoundary, sizeX, sizeY);
        PrintField(vertices, indexX, indexY, isBoundary, isOutside, sizeX, sizeY);

        // Step 5: pre-process isOutside into a "prefix sum" table so that, for any
        // candidate rectangle, we can instantly ask "does this rectangle contain any
        // outside cells at all?" without re-scanning it cell by cell every time. See
        // BuildPrefixSum/RegionSum.
        var outsidePrefix = BuildPrefixSum(isOutside, sizeX, sizeY);

        // Step 6: this part is unchanged in spirit from the original brute-force version -
        // try every pair of red tiles as opposite corners, and keep the biggest rectangle
        // that turns out to be fully red/green. The only thing that changed is HOW we
        // check "is it fully red/green?": instead of an expensive cell-by-cell scan, it's
        // now a single O(1) lookup via RegionSum.
        var biggest = 0L;
        for (var i = 0; i < vertices.Length; i++)
        {
            for (var j = i + 1; j < vertices.Length; j++)
            {
                var (x1, y1) = vertices[i];
                var (x2, y2) = vertices[j];
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

    // Draws one straight boundary line (either vertical or horizontal - never diagonal,
    // per the puzzle's rules) between two points that are already in compressed-grid
    // coordinates. "Drawing a line" here just means: walk from one end to the other, one
    // grid cell at a time, and flip isBoundary to true for every cell we pass through.
    // Because we're operating on the small compressed grid (not the real ~98,000-wide
    // grid), this loop is cheap even though the real-world line it represents could be
    // tens of thousands of tiles long.
    private static void MarkEdge(bool[,] isBoundary, int ax, int ay, int bx, int by)
    {
        if (ax == bx) // same column -> this is a vertical line, walk down the y-axis
            for (var y = Math.Min(ay, by); y <= Math.Max(ay, by); y++)
                isBoundary[ax, y] = true;
        else // same row -> this is a horizontal line, walk along the x-axis
            for (var x = Math.Min(ax, bx); x <= Math.Max(ax, bx); x++)
                isBoundary[x, ay] = true;
    }

    // ── COORDINATE COMPRESSION, EXPLAINED ───────────────────────────────────────────────
    // This is the trick that makes the whole thing feasible. Imagine the real x-axis has
    // red tiles at x = 5, x = 6, and x = 90,000. A full-size grid would need 90,001 columns
    // just to represent that, even though columns 7 through 89,999 are one giant featureless
    // gap - nothing interesting ever happens there, every one of those columns behaves
    // identically to its neighbours.
    //
    // So instead of one grid slot per real column, we only hand out slots for:
    //   - each DISTINCT value that actually appears on a red tile (its own dedicated slot),
    //   - ONE extra slot to represent the entire gap that follows it, if there is one.
    // For the example above: x=5 gets slot 1, then one slot (slot 2) represents the whole
    // gap from x=6 to x=89,999, then x=90,000 gets slot 3. Three slots instead of 90,001.
    //
    // "index" is the lookup table that lets the rest of the code translate a real
    // coordinate (that came from the input) into its compressed slot number. We only ever
    // need to look up real coordinates that belong to an actual red tile - which is
    // exactly the set of values this dictionary contains - so there's no risk of asking
    // for a coordinate that was never given a slot.
    private static (int size, Dictionary<long, int> index) CompressAxis(IEnumerable<long> vertexValues)
    {
        var distinct = vertexValues.Distinct().Order().ToArray();
        var index = new Dictionary<long, int>();
        var nextSlot = 1; // slot 0 is reserved - see FloodFillOutside for why

        for (var i = 0; i < distinct.Length; i++)
        {
            index[distinct[i]] = nextSlot++;

            // If the next distinct value isn't immediately adjacent to this one, there's a
            // gap between them - reserve one slot to represent that whole gap.
            if (i + 1 < distinct.Length && distinct[i + 1] > distinct[i] + 1)
                nextSlot++;
        }

        // +1 for one more sentinel slot past the last real value (mirrors slot 0 at the start).
        return (nextSlot + 1, index);
    }

    // ── FLOOD FILL, EXPLAINED ────────────────────────────────────────────────────────────
    // This is the same idea as the "bucket fill" tool in an image editor: click a spot, and
    // the fill spreads outward in every direction, stopping wherever it hits a boundary
    // line, and everywhere it reaches gets painted.
    //
    // We start at grid position (0, 0). Thanks to how CompressAxis reserves slot 0 as a
    // sentinel below/left of every real coordinate, (0, 0) is guaranteed to sit outside the
    // shape - no red tile or green line can ever be there. From that starting point, we
    // spread out one step at a time (up/down/left/right only, no diagonals) using a queue
    // (this style of "spread outward one layer at a time" search is called a breadth-first
    // search, or BFS). We refuse to step onto any cell marked isBoundary - those are walls
    // we can't cross. Every cell the fill successfully reaches gets marked isOutside = true.
    //
    // Once the fill finishes, any cell that was NEVER reached must be either part of the
    // boundary itself, or sealed inside it - there's no other way it could have avoided the
    // fill, since the boundary forms one unbroken loop. So "not reached" is exactly the same
    // as "red or green", which is exactly what the rest of the algorithm needs to know.
    private static bool[,] FloodFillOutside(bool[,] isBoundary, int sizeX, int sizeY)
    {
        var isOutside = new bool[sizeX, sizeY];
        var queue = new Queue<(int x, int y)>();

        isOutside[0, 0] = true; // slot 0 on each axis is the outside sentinel, never boundary
        queue.Enqueue((0, 0));

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            foreach (var (nx, ny) in new[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) })
            {
                if (nx < 0 || nx >= sizeX || ny < 0 || ny >= sizeY)
                    continue; // fell off the edge of the grid
                if (isOutside[nx, ny] || isBoundary[nx, ny])
                    continue; // already visited, or it's a wall we can't cross

                isOutside[nx, ny] = true;
                queue.Enqueue((nx, ny)); // remember to spread out from here too, later
            }
        }

        return isOutside;
    }

    // ── PREFIX SUMS, EXPLAINED ───────────────────────────────────────────────────────────
    // Later on we need to repeatedly ask "does this rectangle contain any outside cells?"
    // for ~120,000 different candidate rectangles. Counting cell-by-cell every single time
    // would be slow again - exactly the kind of per-cell work we just spent all this effort
    // avoiding.
    //
    // A prefix sum table fixes this. prefix[x, y] stores "how many outside cells are there
    // in the rectangle from the grid's top-left corner up to (x, y)?" - a running total,
    // similar to a running total column in a spreadsheet. Once this table is built (which
    // only requires one pass over the grid), the count of outside cells in ANY rectangle
    // can be worked out from just 4 lookups, no matter how big that rectangle is - see
    // RegionSum below.
    private static int[,] BuildPrefixSum(bool[,] isOutside, int sizeX, int sizeY)
    {
        // The table is 1 bigger in each dimension than the grid itself, so that
        // "up to and including row/column 0" has a well-defined all-zero row/column to
        // start counting from, instead of needing special-case checks near the edges.
        var prefix = new int[sizeX + 1, sizeY + 1];

        for (var x = 0; x < sizeX; x++)
            for (var y = 0; y < sizeY; y++)
                prefix[x + 1, y + 1] = (isOutside[x, y] ? 1 : 0) + prefix[x, y + 1] + prefix[x + 1, y] - prefix[x, y];

        return prefix;
    }

    // Given the running-total table BuildPrefixSum produced, this returns the count of
    // outside cells strictly within the rectangle [loX..hiX] x [loY..hiY] - in other
    // words, the answer to "how many outside cells are in just this rectangle?", not
    // "...from the corner up to here?" like the raw table stores.
    //
    // The formula is the classic "inclusion-exclusion" trick: take the running total up to
    // the rectangle's bottom-right corner, then subtract off everything above it and
    // everything to its left (since those regions aren't part of our rectangle) - but doing
    // that subtracts the corner region above-and-to-the-left twice, so we add it back once.
    private static int RegionSum(int[,] prefix, int loX, int hiX, int loY, int hiY)
        => prefix[hiX + 1, hiY + 1] - prefix[loX, hiY + 1] - prefix[hiX + 1, loY] + prefix[loX, loY];

    // Debug helper only - draws the shape as text using the same '#' (red) / 'X' (green) /
    // '.' (empty) characters the puzzle description uses, so we can visually sanity-check
    // the algorithm against the small example by eye. It operates on the COMPRESSED grid,
    // not real coordinates, so large gaps get squished down to a single character wide -
    // that's fine for the small example (which barely has any gaps to compress anyway),
    // but it's exactly why this is skipped for the real input: even compressed, a ~990x990
    // grid of text is too big to be useful to read, so Variables.UseMockInput guards it.
    private void PrintField(
        (long x, long y)[] vertices, Dictionary<long, int> indexX, Dictionary<long, int> indexY,
        bool[,] isBoundary, bool[,]? isOutside, int sizeX, int sizeY)
    {
        if (Variables.UseMockInput != true) // Real input's compressed grid is still too big to print usefully
            return;

        var redCells = vertices.Select(v => (x: indexX[v.x], y: indexY[v.y])).ToHashSet();

        Log.Debug(string.Empty);
        for (var y = 0; y < sizeY; y++)
        {
            var line = new char[sizeX];
            for (var x = 0; x < sizeX; x++)
            {
                char c;
                if (redCells.Contains((x, y))) c = Red;
                else if (isBoundary[x, y]) c = Green;
                else if (isOutside is null) c = '.'; // interior not classified yet (first call, before flood fill)
                else c = isOutside[x, y] ? '.' : Green; // reached by the flood fill = outside, otherwise interior
                line[x] = c;
            }
            Log.Debug(new string(line));
        }
    }
}
