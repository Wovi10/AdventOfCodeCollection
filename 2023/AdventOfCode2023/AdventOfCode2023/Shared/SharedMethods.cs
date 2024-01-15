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

    public static List<string> GetInput(string day, bool isMock = false)
    {
        var inputFile = GetRawInput(day, isMock);
        return SplitInputFile(inputFile);
    }

    public static string GetRawInput(string day, bool isMock = false)
    {
        var filePath = isMock ? GetMockFilePath(day) : GetFilePath(day);
        var fullPath = Directory.GetCurrentDirectory() + filePath;
        var inputFile = File.ReadAllText(fullPath);
        return inputFile;
    }

    private static List<string> SplitInputFile(string inputFile)
    {
        return inputFile.Split(Constants.LineSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }

    private static string GetFilePath(string day)
    {
        return $"{Constants.RootInputPath}/Day{day}/Day{day}.in";
    }

    private static string GetMockFilePath(string day)
    {
        return $"{Constants.RootInputPath}/Day{day}/MockDay{day}.in";
    }
}