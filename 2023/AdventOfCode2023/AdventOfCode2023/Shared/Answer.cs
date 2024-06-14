namespace AdventOfCode2023_1.Shared;

public class Answer(int day, int part, bool isReal, object result)
{
    public int Day { get; } = day;
    public int Part { get; } = part;
    public bool IsReal { get; } = isReal;

    public object Result { get; } = result;
}