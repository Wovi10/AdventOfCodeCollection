using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day08 : DayBase
{
    private const char Left = 'L';
    private readonly List<bool> _instructions = new();
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
        _instructions.Clear();
        GetInstructions();
    }

    private void GetInstructions()
    {
        var instructionsString = Input.First();
        foreach (var instruction in instructionsString)
        {
            _instructions.Add(instruction == Left);
        }
    }
}