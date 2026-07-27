using System.Runtime.CompilerServices;

namespace AOC.Challenges;

public static class ChallengePaths
{
    public static string ProjectRoot { get; } = GetProjectRoot();

    public static string YearsRoot => Path.Combine(ProjectRoot, "Years");

    public static string GetYearFolder(int year) => Path.Combine(YearsRoot, year.ToString());

    private static string GetProjectRoot([CallerFilePath] string sourceFile = "")
        => Path.GetDirectoryName(sourceFile)!;
}
