using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public static class Day04
{
    private static readonly string FilePath = Path.Combine(Constants.RootInputPath, "/Day04/Day04.in");
    private static readonly string MockFilePath = Path.Combine(Constants.RootInputPath, "/Day04/MockDay04.in");
    private static readonly string FullPath = Path.Combine(Directory.GetCurrentDirectory(), MockFilePath);
    private static readonly string InputFile = File.ReadAllText(FullPath);
    private static readonly List<string> Input = SharedMethods.GetInput(InputFile);

    public static void Run()
    {
        SharedMethods.WriteBeginText(4, "Scratchcards");
        PartOne();
        // PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        throw new NotImplementedException();
    }
}