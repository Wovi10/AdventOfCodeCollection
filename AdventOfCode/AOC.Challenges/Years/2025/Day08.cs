using AOC.Utils;
using NLog;

namespace AOC.Challenges.Years._2025;

public class Day08() : DayBase("08", "Playground", LogManager.GetCurrentClassLogger())
{
    protected override Task<object> PartOne()
    {
        var result = MulLargestCircuits();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = DistanceToWallForLastTwo();

        return Task.FromResult<object>(result);
    }

    private double MulLargestCircuits()
    {
        var input = GetInput().Select(x => x.Split(',').Select(long.Parse).ToArray()).ToArray();
        var numOfConnectionsToMake = Variables.UseMockInput == true ? 10 : 1000;
        var distancesBetweenBoxes = GetDistanceBetweenBoxes(input, numOfConnectionsToMake).OrderBy(x => x.dist).Take
            (numOfConnectionsToMake).ToArray();

        var circuits = new List<HashSet<int>>();
        foreach (var (_, index1, index2) in distancesBetweenBoxes)
        {
            var circuitWithIndex1 = circuits.FirstOrDefault(c => c.Contains(index1));
            var circuitWithIndex2 = circuits.FirstOrDefault(c => c.Contains(index2));

            if (circuitWithIndex1 is {Count: > 0} && circuitWithIndex2 is {Count: > 0})
            {
                if (circuitWithIndex1 == circuitWithIndex2)
                    continue;

                circuitWithIndex1.UnionWith(circuitWithIndex2);
                circuits.Remove(circuitWithIndex2);
                continue;
            }

            if (circuitWithIndex1 is {Count: > 0})
            {
                circuitWithIndex1.Add(index2);
                continue;
            }

            if (circuitWithIndex2 is {Count: > 0})
            {
                circuitWithIndex2.Add(index1);
                continue;
            }

            circuits.Add(new HashSet<int> { index1, index2 });
        }

        circuits = circuits.OrderByDescending(x => x.Count).ToList();

        return circuits.Take(3).Aggregate(1L, (current, next) => current * next.Count);
    }

    private List<(double dist, int index1, int index2)> GetDistanceBetweenBoxes(long[][] input,
        long numOfConnectionsToMake)
    {
        var result = new List<(double dist, int index1, int index2)>();


        for (var i = 0; i < input.Length; i++)
        {
            var (x1, y1, z1) = (input[i][0], input[i][1], input[i][2]);
            for (var j = i+1; j < input.Length; j++)
            {
                var (x2, y2, z2) = (input[j][0], input[j][1], input[j][2]);
                var distance = CalcDistance(x1, y1, z1, x2, y2, z2);

                if (result.Count > numOfConnectionsToMake && result.All(x => x.dist < distance))
                    continue;

                result.Add((distance, i, j));
            }
        }
        return result;

        static double CalcDistance(long x1, long y1, long z1, long x2, long y2, long z2)
            => Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2));
    }

    private long DistanceToWallForLastTwo()
    {
        var input = GetInput().Select(x => x.Split(',').Select(long.Parse).ToArray()).ToArray();
        var distancesBetweenBoxes = GetDistanceBetweenBoxes(input, long.MaxValue).OrderBy(x => x.dist).ToArray();

        var circuits = new List<HashSet<int>>();
        foreach (var (_, index1, index2) in distancesBetweenBoxes)
        {
            var circuitWithIndex1 = circuits.FirstOrDefault(c => c.Contains(index1));
            var circuitWithIndex2 = circuits.FirstOrDefault(c => c.Contains(index2));

            if (circuitWithIndex1 is {Count: > 0} && circuitWithIndex2 is {Count: > 0})
            {
                if (circuitWithIndex1 == circuitWithIndex2)
                    continue;

                circuitWithIndex1.UnionWith(circuitWithIndex2);
                circuits.Remove(circuitWithIndex2);
                if (circuits.Count == 1 && circuits[0].Count == input.Length)
                    return input[index1][0] * input[index2][0];

                continue;
            }

            if (circuitWithIndex1 is {Count: > 0})
            {
                circuitWithIndex1.Add(index2);
                if (circuits.Count == 1 && circuits[0].Count == input.Length)
                    return input[index1][0] * input[index2][0];
                continue;
            }

            if (circuitWithIndex2 is {Count: > 0})
            {
                circuitWithIndex2.Add(index1);
                if (circuits.Count == 1 && circuits[0].Count == input.Length)
                    return input[index1][0] * input[index2][0];
                continue;
            }

            circuits.Add(new HashSet<int> { index1, index2 });
        }

        return 0L;
    }
}
