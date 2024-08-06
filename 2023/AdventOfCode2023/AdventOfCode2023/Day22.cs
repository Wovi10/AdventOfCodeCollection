using AdventOfCode2023_1.Models.Day22;

namespace AdventOfCode2023_1;

public class Day22() : DayBase("22", "Sand slabs")
{
    protected override Task<object> PartOne()
    {
        var result = GetNumberOfDisintegrableBricks();
        
        return Task.FromResult<object>(result);
    }


    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private int GetNumberOfDisintegrableBricks()
    {
        var brickPile = new BrickPile(Input);
        brickPile.MoveBricksDown();
        brickPile.OrderBricks();
        return brickPile.CountDisintegrableBricks();
        
        brickPile.Print();
        return 0;
    }
}
