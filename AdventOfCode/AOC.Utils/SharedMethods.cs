using System.Runtime.CompilerServices;

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

    // projectRoot lets a caller (DayBase) supply an already-resolved root explicitly.
    // Everyone else (Day classes calling this directly) gets it for free via
    // callerFilePath, which the compiler fills in with their own .cs file's path -
    // this makes input resolution independent of the process's working directory.
    // useMock overrides Constants.IsRealExercise for callers that pick mock/real
    // explicitly (e.g. a GUI toggle) instead of relying on that global switch.
    public static List<string> GetInput(string day, string? projectRoot = null, bool? useMock = null, [CallerFilePath] string callerFilePath = "")
    {
        var root = projectRoot ?? Path.GetDirectoryName(callerFilePath)!;
        var resolvedUseMock = useMock ?? Variables.UseMockInput ?? !Constants.IsRealExercise;
        var filePath = GetFilePath(day, resolvedUseMock);
        var fullPath = Path.Combine(root, filePath);
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

    private static string GetFilePath(string day, bool useMock)
    {
        var basePath = $"{Constants.InputFolderName}/Day{day}/";

        if (useMock)
            basePath += "Mock";

        basePath += $"Day{day}";

        var differentMockDays =
            Constants.RunningYear == 2023
                ? new List<string> {"01", "08", "10", "13", "20"}
                : Constants.RunningYear == 2024
                ? new List<string> {"03", "17"}
                : [];

        if (useMock && Variables.RunningPartOne && differentMockDays.Contains(day))
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