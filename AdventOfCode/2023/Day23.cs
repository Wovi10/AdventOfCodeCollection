using _2023.Models.Day23;

namespace _2023;

public class Day23() : DayBase("23", "A long walk")
{
    protected override async Task<object> PartOne()
    {
        var result = await GetLongestHike();

        return result;
    }

    protected override async Task<object> PartTwo()
    {
        var result = await GetLongestHike();

        return result;
    }

    private static async Task<int> GetLongestHike() 
        => await Input
            .CreateSnowIsland()
            .DoItAlternativeWay()
            // .FindLongestHike()
        ;
}