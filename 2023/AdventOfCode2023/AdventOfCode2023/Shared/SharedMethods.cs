namespace AdventOfCode2023_1.Shared;

public static class SharedMethods
{
    public static void WriteBeginText(string day, string title)
    {
        ClearCurrentConsoleLine();
        Console.WriteLine($"Starting day {day} challenge: {title}");
    }

    public static void PrintAnswer(object result)
    {
        ClearCurrentConsoleLine();
        Console.WriteLine($"{Constants.LineReturn}Answer of part {GetRunningPart()} is: \n{result}");
    }

    private static string GetRunningPart()
        => Variables.RunningPartOne ? "1" : "2";

    public static List<string> GetInput(string day)
    {
        var filePath = GetFilePath(day);
        var fullPath = Directory.GetCurrentDirectory() + filePath;
        var inputFile = File.ReadAllText(fullPath);
        return SplitInputFile(inputFile);
    }

    public static void PrintPercentage(long current, long max)
    {
        var progress = (double) current / max;
        var percentage = (int) (progress * 100);
        PrintPercentage(percentage);
    }

    public static void PrintPermille(long current, long max)
    {
        var progress = (double) current / max;
        var promille = (int) (progress * 1000);
        PrintPermille(promille);
    }

    private static long? _previousPermille;

    private static void PrintPermille(int permille)
    {
        if (_previousPermille == permille)
            return;

        _previousPermille = permille;

        var promilleDec = permille / 100;

        var spaces = new string(Convert.ToChar(Constants.Space), 100 - promilleDec);
        var promilleString = new string(Convert.ToChar(Constants.HashTag), promilleDec);

        Console.Write($"{Constants.LineReturn}[{promilleString}{spaces}] {permille:D3}‰");
    }

    private static long? _previousPercentage;

    private static void PrintPercentage(int percentage)
    {
        if (_previousPercentage == percentage)
            return;

        _previousPercentage = percentage;

        var percentageDec = percentage / 10;

        var spaces = new string(Convert.ToChar(Constants.Space), 10 - percentageDec);
        var percentageString = new string(Convert.ToChar(Constants.HashTag), percentageDec);

        ClearCurrentConsoleLine();
        Console.Write($"{Constants.LineReturn}[{percentageString}{spaces}] {percentage:D2}%");
    }

    private static List<string> SplitInputFile(string inputFile)
    {
        return inputFile.Split(Constants.LineSeparator).ToList();
    }

    private static string GetFilePath(string day)
    {
        const string mock = Constants.IsMock ? "Mock" : Constants.EmptyString;
        return $"{Constants.RootInputPath}/Day{day}/{mock}Day{day}.in";
    }

    public static void ClearCurrentConsoleLine() 
        => Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
}