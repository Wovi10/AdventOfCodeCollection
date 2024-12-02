using _2023.Models.Day25;

namespace _2023;

public class Day25() : DayBase("25", "Snowverload")
{
    protected override Task<object> PartOne()
    {
        var result = GetGroupSizeProduct();
        
        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long GetGroupSizeProduct()
    {
        var weatherMachine = new WeatherMachine(Input);
        var reducedGraph = weatherMachine
            .ReduceGraph(3);

        var result = reducedGraph.First().Value * reducedGraph.Last().Value;
        
        return result;
    }
}