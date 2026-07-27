using System.Runtime.CompilerServices;

namespace AOC.Challenges;

public static class ChallengePaths
{
    public static string ProjectRoot { get; } = GetProjectRoot();

    // AOC.Challenges sits next to the legacy year projects (AdventOfCode/2023, AdventOfCode/2024).
    private static string SolutionRoot => Path.Combine(ProjectRoot, "..");

    public static string YearsRoot => Path.Combine(ProjectRoot, "Years");

    public static string GetManagedYearFolder(int year) => Path.Combine(YearsRoot, year.ToString());

    public static string GetLegacyYearFolder(int year) => Path.Combine(SolutionRoot, year.ToString());

    private static string GetProjectRoot([CallerFilePath] string sourceFile = "")
        => Path.GetDirectoryName(sourceFile)!;
}
