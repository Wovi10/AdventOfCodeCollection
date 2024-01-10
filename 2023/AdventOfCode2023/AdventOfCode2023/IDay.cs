using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    public virtual void Run(int day, string title)
    {
        SharedMethods.WriteBeginText(day, title);
        PartOne();
        PartTwo();
        Console.WriteLine();
    }

    public abstract void PartOne();

    public abstract void PartTwo();
}