namespace AdventOfCode2023_1.Shared;

public static class SharedMethods
{
    public static void WriteBeginText(int day, string title)
    {
        Console.WriteLine($"Starting day {day} challenge: {title}");
    }

    public static void AnswerPart(int part, object result)
    {
        Console.WriteLine($"Answer of part {part} is: \n{result}");
    }
    
    public static List<string> GetInput(string inputFile)
    {
        return inputFile.Split(Constants.LineSeparator).ToList();
    }
}