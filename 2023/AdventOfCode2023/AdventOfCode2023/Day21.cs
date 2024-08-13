using System.Numerics;
using AdventOfCode2023_1.Models.Day21;

namespace AdventOfCode2023_1;

public class Day21() : DayBase("21", "Step counter")
{
    protected override Task<object> PartOne()
    {
        const int numberOfSteps = IsReal ? 64 : 6;
        var result = CountReachableGardenPlots(numberOfSteps);

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        const int numberOfSteps = IsReal ? 26501365 : 100;

        // Input but all in 1 string (Add newline for each row)
        var input = string.Join("\n", Input);

        var steps = Steps(ParseMap(input)).Take(328).ToArray();

        (decimal x0, decimal y0) = (65, steps[65]);
        (decimal x1, decimal y1) = (196, steps[196]);
        (decimal x2, decimal y2) = (327, steps[327]);

        var y01 = (y1 - y0) / (x1 - x0);
        var y12 = (y2 - y1) / (x2 - x1);
        var y012 = (y12 - y01) / (x2 - x0);

        const int n = 26501365;
        var solution = decimal.Round(y0 + y01 * (n - x0) + y012 * (n - x0) * (n - x1));

        // var result = CountReachableGardenPlots(numberOfSteps);

        return Task.FromResult<object>(solution);
    }

    IEnumerable<long> Steps(HashSet<Complex> map)
    {
        var positions = new HashSet<Complex> {new Complex(65, 65)};

        while (true)
        {
            yield return positions.Count;
            positions = Step(map, positions);
        }
    }

    HashSet<Complex> Step(HashSet<Complex> map, HashSet<Complex> positions)
    {
        Complex[] dirs = [1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

        var res = new HashSet<Complex>();
        foreach (var pos in positions)
        {
            foreach (var dir in dirs)
            {
                var posT = pos + dir;
                var tileCol = Mod(posT.Real, 131);
                var tileRow = Mod(posT.Imaginary, 131);
                if (map.Contains(new Complex(tileCol, tileRow)))
                {
                    res.Add(posT);
                }
            }
        }

        return res;
    }

    double Mod(double n, int m) => ((n % m) + m) % m;

    HashSet<Complex> ParseMap(string input)
    {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol] != '#'
            select new Complex(icol, irow)
        ).ToHashSet();
    }

    private long CountReachableGardenPlots(int numberOfSteps)
    {
        var garden = new Garden(Input);

        var result = garden.CalculateReachableGardenPlots(numberOfSteps);
        // garden.PrintReachableTilesInGarden();

        return result;
    }
}