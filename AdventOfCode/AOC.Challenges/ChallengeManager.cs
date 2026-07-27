using System.Reflection;
using System.Text.RegularExpressions;

namespace AOC.Challenges;

public static class ChallengeManager
{
    // 2023 and 2024 already exist as their own runnable projects; AOC.Challenges only
    // owns the file structure for years added after this project existed.
    private static readonly int[] LegacyYears = [2023, 2024];

    public static bool IsLegacyYear(int year) => LegacyYears.Contains(year);

    public static IReadOnlyList<int> GetAvailableYears()
    {
        var managedYears = Directory.Exists(ChallengePaths.YearsRoot)
            ? Directory.GetDirectories(ChallengePaths.YearsRoot)
                .Select(Path.GetFileName)
                .Where(name => int.TryParse(name, out _))
                .Select(name => int.Parse(name!))
            : [];

        return LegacyYears.Concat(managedYears).Distinct().OrderBy(year => year).ToList();
    }

    public static void CreateYear(int year)
    {
        if (IsLegacyYear(year))
            return;

        Directory.CreateDirectory(ChallengePaths.GetManagedYearFolder(year));
    }

    public static IReadOnlyList<int> GetExistingDays(int year)
        => year switch
        {
            2023 => GetExistingDaysFromAssembly(typeof(_2023.Day01).Assembly),
            2024 => GetExistingDaysFromAssembly(typeof(_2024.Day01).Assembly),
            _ => GetExistingDaysFromFolder(ChallengePaths.GetManagedYearFolder(year))
        };

    private static string GetYearRoot(int year)
        => IsLegacyYear(year) ? ChallengePaths.GetLegacyYearFolder(year) : ChallengePaths.GetManagedYearFolder(year);

    private static string GetDayInputFolder(int year, int day)
        => Path.Combine(GetYearRoot(year), "Input", $"Day{day:D2}");

    private static string GetRealInputFile(int year, int day)
        => Path.Combine(GetDayInputFolder(year, day), $"Day{day:D2}.in");

    // Some legacy days use a differently-named mock file for part 1 (MockDayXXPart01.in);
    // this manages the primary MockDayXX.in only.
    private static string GetMockInputFile(int year, int day)
        => Path.Combine(GetDayInputFolder(year, day), $"MockDay{day:D2}.in");

    public static string GetRealInput(int year, int day)
        => ReadIfExists(GetRealInputFile(year, day));

    public static void SaveRealInput(int year, int day, string content)
        => WriteInput(GetRealInputFile(year, day), content);

    public static bool RealInputExists(int year, int day)
        => File.Exists(GetRealInputFile(year, day));

    public static string GetRealInputFileName(int year, int day)
        => Path.GetFileName(GetRealInputFile(year, day));

    public static string GetMockInput(int year, int day)
        => ReadIfExists(GetMockInputFile(year, day));

    public static void SaveMockInput(int year, int day, string content)
        => WriteInput(GetMockInputFile(year, day), content);

    public static bool MockInputExists(int year, int day)
        => File.Exists(GetMockInputFile(year, day));

    public static string GetMockInputFileName(int year, int day)
        => Path.GetFileName(GetMockInputFile(year, day));

    private static string ReadIfExists(string file)
        => File.Exists(file) ? File.ReadAllText(file) : string.Empty;

    private static void WriteInput(string file, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, content);
    }

    private static readonly Regex DayClassName = new("^Day(?<day>\\d{2})$");

    private static List<int> GetExistingDaysFromAssembly(Assembly assembly)
        => assembly.GetTypes()
            .Select(type => DayClassName.Match(type.Name))
            .Where(match => match.Success)
            .Select(match => int.Parse(match.Groups["day"].Value))
            .OrderBy(day => day)
            .ToList();

    private static List<int> GetExistingDaysFromFolder(string yearFolder)
    {
        if (!Directory.Exists(yearFolder))
            return [];

        return Directory.GetFiles(yearFolder, "Day*.cs")
            .Select(file => DayClassName.Match(Path.GetFileNameWithoutExtension(file)))
            .Where(match => match.Success)
            .Select(match => int.Parse(match.Groups["day"].Value))
            .OrderBy(day => day)
            .ToList();
    }
}
