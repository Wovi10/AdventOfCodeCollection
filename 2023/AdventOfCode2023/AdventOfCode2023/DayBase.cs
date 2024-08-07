﻿using System.Diagnostics;
using AdventOfCode2023_1.Shared;
using AdventOfCode2023_1.Shared.Enums;
using NUnit.Framework;

namespace AdventOfCode2023_1;

public abstract class DayBase
{
    protected const bool IsDebug = true;
    public const bool IsReal = true;
    protected static List<string> Input = new();
    private object _expectedAnswer = 0;
    protected string Day = "01";

    public async Task Run(string day, string title, PartsToRun partToRun = PartsToRun.Both)
    {
        Day = day;
        WriteStopwatchStartText();
        var watch = new Stopwatch();
        watch.Start();

        SharedMethods.WriteBeginText(day, title);
        switch (partToRun)
        {
            case PartsToRun.Part1:
                await RunPartOne();
                break;
            case PartsToRun.Part2:
                await RunPartTwo();
                break;
            case PartsToRun.Both:
                await RunPartOne();
                await RunPartTwo();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        watch.Stop();
        WriteStopwatchText(watch.ElapsedMilliseconds);

        Console.WriteLine();
    }

    private static void WriteStopwatchStartText()
    {
        if (IsDebug)
        {
            Console.WriteLine($"Started at {DateTime.Now:HH:mm:ss}");
        }
    }

    private static void WriteStopwatchText(long watchElapsedMilliseconds)
    {
        if (IsDebug)
            Console.WriteLine($"Elapsed time: {watchElapsedMilliseconds} ms");
    }

    private async Task RunPartOne() 
        => await RunPart(true, PartOne);

    private async Task RunPartTwo()
        => await RunPart(false, PartTwo);

    private async Task RunPart(bool runningPartOne, Func<Task<object>> partToRun)
    {
        Variables.RunningPartOne = runningPartOne;
        Input = SharedMethods.GetInput(Day);
        _expectedAnswer = GetExpectedAnswer(runningPartOne);

        var result = await partToRun();
        SharedMethods.PrintAnswer(result);

        if (!IsDebug) Assert.That(result, Is.EqualTo(_expectedAnswer));
    }

    protected abstract Task<object> PartOne();

    protected abstract Task<object> PartTwo();

    private object GetExpectedAnswer(bool partOne)
        => Answers.GetExpectedAnswer(Day, partOne);
}