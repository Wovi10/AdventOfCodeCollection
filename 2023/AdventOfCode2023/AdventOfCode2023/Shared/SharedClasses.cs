namespace AdventOfCode2023_1.Shared;

public static class SharedClasses
{
    public static void WriteBeginText(int day, string title)
    {
        Console.WriteLine($"Starting day {day} challenge: {title}");
    }

    public static void AnswerPart(int part, object result)
    {
        Console.WriteLine($"Answer of part {part} is: \n{result}");
    }
}