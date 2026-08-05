using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day09() : DayBase("09", "Movie Theater")
{
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

    private const char Red = '#';
    private const char Green = 'X';
    private long FindBiggestAreaRedAndGreenTiles()
    {
        HashSet<(long x, long y, char color)> used =
            GetInput()
                .Select(l => l.Split(',').Select(long.Parse).ToArray())
                .Select(a => (a[0], a[1], Red))
                .ToHashSet();

        used = AddGreenCoords(used);
        used = FillAreaWithGreen(used);

        PrintField(used);

        return GetMaxArea(used);
    }

    private static void PrintField(HashSet<(long x, long y, char color)> used)
    {
        if (Variables.UseMockInput != true) // Real input is too big to print
            return;

        var maxX = used.MaxBy(c => c.x).x + 1;
        var maxY = used.MaxBy(c => c.y).y + 1;

        Console.WriteLine();
        for (var y = 0; y <= maxY; y++)
        {
            var line = string.Empty;
            for (var x = 0; x <= maxX; x++)
            {
                var color = used.FirstOrDefault(c => c.x == x && c.y == y).color;
                line += color != '\0' ? color : '.';
            }

            Console.WriteLine(line);
        }
    }

    private static HashSet<(long x, long y, char color)> AddGreenCoords(HashSet<(long x, long y, char color)> used)
    {
        var result = new HashSet<(long x, long y, char color)>();
        foreach (var coord1 in used)
        {
            foreach (var coord2 in used.Where(c => c.x == coord1.x && c.y != coord1.y)) // Redcoords on same x coord |
            {
                var biggestY = Math.Max(coord1.y, coord2.y) - 1;
                var smallestY = Math.Min(coord1.y, coord2.y) + 1;
                for (var i = smallestY; i <= biggestY; i++)
                    result.Add(coord1 with {y = i, color = Green});
            }

            foreach (var coord2 in used.Where(c => c.y == coord1.y && c.x != coord1.x)) // Redcoords on same y coord -
            {
                var biggestX = Math.Max(coord1.x, coord2.x) - 1;
                var smallestX = Math.Min(coord1.x, coord2.x) + 1;
                for (var i = smallestX; i <= biggestX; i++)
                    result.Add(coord1 with {x = i, color = Green});
            }
        }

        return used.Concat(result).ToHashSet();
    }

    private static HashSet<(long x, long y, char color)> FillAreaWithGreen(HashSet<(long x, long y, char color)> coordsUsed)
    {
        var highestRedY = coordsUsed.MaxBy(c => c.y).y;
        var lowestRedY = coordsUsed.MinBy(c => c.y).y;
        var totalChecks = highestRedY - lowestRedY;
        var currentChecks = 1L;

        for (var yToCheck = lowestRedY + 1; yToCheck < highestRedY; yToCheck++)
        {
            var y = yToCheck;
            var preLine = coordsUsed.Where(c => c.y == y - 1).Select(c => c.x).ToHashSet();
            var curLine = coordsUsed.Where(c => c.y == y).Select(c => c.x).ToHashSet();
            var maxCurLine = curLine.Max();

            for (var xToCheck = curLine.Min()-1; xToCheck < maxCurLine; xToCheck++)
            {
                if (preLine.Contains(xToCheck-1) && preLine.Contains(xToCheck) && curLine.Contains(xToCheck-1))
                    curLine.Add(xToCheck);
            }

            coordsUsed.UnionWith(curLine.Select(x => (x, y, Green)));

            var percentageDone = (double)currentChecks / totalChecks * 100;
            if (percentageDone % 5 == 0)
            {
                Console.WriteLine("After {0} rows there are {1} coordsUsed \t\t {2}% done", currentChecks,
                    coordsUsed.Count, percentageDone);
            }
            currentChecks++;
        }

        return coordsUsed.OrderBy(c => c.x).ThenBy(c => c.y).ToHashSet();
    }

    private static long GetMaxArea(HashSet<(long x, long y, char color)> used)
    {
        var biggest = 0L;

        foreach (var c1 in used.Where(c => c.color == Red))
        {
            foreach (var c2 in used.Where(c => c.color == Red && !(c.x == c1.x && c.y == c1.y)))
            {
                var (biggestX, smallestX) = (Math.Max(c1.x, c2.x), Math.Min(c1.x, c2.x));
                var (biggestY, smallestY) = (Math.Max(c1.y, c2.y), Math.Min(c1.y, c2.y));

                if (!AllFilled(smallestX, biggestX, smallestY, biggestY, used))
                    continue;

                var area = (biggestX - smallestX + 1) * (biggestY - smallestY + 1);
                biggest = Math.Max(area, biggest);
            }
        }

        return biggest;
    }

    private static bool AllFilled(
        long smallestX, long biggestX, long smallestY, long biggestY, HashSet<(long x, long y, char color)> used)
    {
        for (var x = smallestX; x <= biggestX; x++)
            for (var y = smallestY; y <= biggestY; y++)
                if (!used.Contains((x, y, Red)) && !used.Contains((x,y,Green)))
                    return false;
        return true;
    }
}