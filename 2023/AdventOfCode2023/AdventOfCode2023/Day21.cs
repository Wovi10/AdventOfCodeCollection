using AdventOfCode2023_1.Models.Day21;

namespace AdventOfCode2023_1;

public class Day21 : DayBase
{
    protected override async Task<object> PartOne()
    {
        const int numberOfSteps = IsReal ? 64 : 6;
        var result = await CountReachableGardenPlots(numberOfSteps);

        return result;
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private Task<long> CountReachableGardenPlots(int numberOfSteps)
    {
        var garden = new Garden(Input);
        // garden.Print();

        return garden.CalculateReachableGardenPlots(numberOfSteps);
    }
}