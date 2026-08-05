using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day08() : DayBase("08", "Playground")
{
    protected override Task<object> PartOne()
    {
        var result = MulLargestCircuits();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private double MulLargestCircuits()
    {
        var numOfConnectionsToMake = Variables.UseMockInput == true ? 10 : 1000;
        var distancesBetweenBoxes = GetDistanceBetweenBoxes(numOfConnectionsToMake).OrderBy(x => x.dist).Take(numOfConnectionsToMake).ToArray();

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

    private List<(double dist, int index1, int index2)> GetDistanceBetweenBoxes(int numOfConnectionsToMake)
    {
        var result = new List<(double dist, int index1, int index2)>();

        var input = GetInput().Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
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

        static double CalcDistance(int x1, int y1, int z1, int x2, int y2, int z2)
            => Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2));
    }
}
