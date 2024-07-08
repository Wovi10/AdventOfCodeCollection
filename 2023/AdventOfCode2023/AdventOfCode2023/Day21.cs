using AdventOfCode2023_1.Models.Day21;

namespace AdventOfCode2023_1;

public class Day21 : DayBase
{
    protected override Task<object> PartOne()
    {
        const int numberOfSteps = IsReal ? 64 : 6;
        var result = CountReachableGardenPlots(numberOfSteps);

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long CountReachableGardenPlots(int numberOfSteps)
    {
        var garden = new Garden(Input);
        // garden.Print();

        return garden.CalculateReachableGardenPlots(numberOfSteps);
    }
}