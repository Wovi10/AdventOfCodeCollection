using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected static List<string> Input = new();

    public void Run(string day, string title, PartsToRun partToRun = Constants.PartToRun)
    {
        Input = SharedMethods.GetInput(day);
        SharedMethods.WriteBeginText(day, title);
        Variables.RunningPartOne = true;
        switch (partToRun)
        {
            case PartsToRun.Part1:
                PartOne();
                break;
            case PartsToRun.Part2:
                Variables.RunningPartOne = false;
                PartTwo();
                break;
            case PartsToRun.Both:
                PartOne();
                Variables.RunningPartOne = false;
                PartTwo();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Console.WriteLine();
    }

    protected abstract void PartOne();

    protected abstract void PartTwo();
}