namespace AdventOfCode2023_1.Shared;

public class Answer
{
    public Answer(int day, int part, bool isReal, object result)
    {
        Day = day;
        Part = part;
        IsReal = isReal;
        Result = result;
    }

    public int Day { get; set; }
    public int Part { get; set; }
    public bool IsReal { get; set; }

    public object Result { get; set; }
}