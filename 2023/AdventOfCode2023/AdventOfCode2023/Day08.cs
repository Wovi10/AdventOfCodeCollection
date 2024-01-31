using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day08 : DayBase
{
    public List<bool> Instructions = new();
    protected override void PartOne()
    {
        var result = CalculateSteps();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        var result = 0;
        SharedMethods.AnswerPart(2, result);
    }
    
    private int CalculateSteps()
    {
        ProcessInput();
        return 0;
    }

    private void ProcessInput()
    {
        GetInstructions();
    }

    private void GetInstructions()
    {
        throw new NotImplementedException();
    }
}