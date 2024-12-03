namespace AOC.Utils;

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
        Console.WriteLine($"{UtilsCSharp.Utils.Constants.LineReturn}Answer of part {GetRunningPart()} is: \n{result}");
    }

    private static string GetRunningPart()
        => Variables.RunningPartOne ? "1" : "2";

    public static List<string> GetInput(string day)
    {
        var filePath = GetFilePath(day);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        var inputFile = File.ReadAllText(fullPath);
        var splitInput = SplitInputFile(inputFile);
        return splitInput.Select(line => line.Trim()).ToList();
    }

    public static int GetPercentage(long current, long max)
    {
        var progress = (double) current / max;
        var percentage = (int) (progress * 100);
        return percentage;
    }

    public static void PrintPercentage(long current, long max)
    {
        var percentage = GetPercentage(current, max);
        PrintPercentage(percentage);
    }

    public static void PrintPermille(long current, long max)
    {
        var progress = (double) current / max;
        var promille = (int) (progress * 1000);
        PrintPermille(promille);
    }

    public static void PrintProgress(long current, long max)
    {
        Console.Write($"Finished {current} parts of {max}");
    }

    private static long? _previousPermille;

    private static void PrintPermille(int permille)
    {
        if (_previousPermille == permille)
            return;

        _previousPermille = permille;

        var promilleDec = permille / 100;

        var spaces = new string(Convert.ToChar((string)UtilsCSharp.Utils.Constants.Space), 100 - promilleDec);
        var promilleString = new string(Convert.ToChar((string)UtilsCSharp.Utils.Constants.HashTag), promilleDec);

        Console.Write($"{UtilsCSharp.Utils.Constants.LineReturn}[{promilleString}{spaces}] {permille:D3}‰");
    }

    private static long? _previousPercentage;

    private static void PrintPercentage(int percentage)
    {
        if (_previousPercentage == percentage)
            return;

        _previousPercentage = percentage;

        var percentageDec = percentage / 10;

        var spaces = new string(Convert.ToChar((string)UtilsCSharp.Utils.Constants.Space), 10 - percentageDec);
        var percentageString = new string(Convert.ToChar((string)UtilsCSharp.Utils.Constants.HashTag), percentageDec);

        ClearCurrentConsoleLine();
        Console.Write($"{UtilsCSharp.Utils.Constants.LineReturn}[{percentageString}{spaces}] {percentage:D2}%");
    }

    private static List<string> SplitInputFile(string inputFile)
    {
        return inputFile.Split(UtilsCSharp.Utils.Constants.LineSeparator).ToList();
    }

    private static string GetFilePath(string day)
    {
        var basePath = $"{Constants.RootInputPath}/Day{day}/";

        if (!Constants.IsRealExercise)
            basePath += "Mock";
        
        basePath += $"Day{day}";

        var differentMockDays =
            Constants.RunningYear == 2023
                ? new List<string> {"01", "08", "10", "13", "20"}
                : new List<string> {};

        if (!Constants.IsRealExercise && Variables.RunningPartOne && differentMockDays.Contains(day))
            basePath += "Part01";

        return $"{basePath}.in";
    }

    public static void ClearCurrentConsoleLine() 
        => Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");

    public static void ForceExitProgram()
    {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0);
    }
}