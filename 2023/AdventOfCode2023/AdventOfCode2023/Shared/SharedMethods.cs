﻿namespace AdventOfCode2023_1.Shared;

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
        var differentMockDays = new List<string> {"01", "08", "10"};
        if (differentMockDays.Contains(day) && !Constants.IsReal)
        {
            return $"{Constants.RootInputPath}/Day{day}/MockDay{day}Part01.in";
        }
        const string mock = Constants.IsReal ? Constants.EmptyString : "Mock";
        return $"{Constants.RootInputPath}/Day{day}/{mock}Day{day}.in";
    }

    public static void ClearCurrentConsoleLine() 
        => Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
}