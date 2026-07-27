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

        Directory.CreateDirectory(ChallengePaths.GetYearFolder(year));
    }

    public static IReadOnlyList<int> GetExistingDays(int year)
        => year switch
        {
            2023 => GetExistingDaysFromAssembly(typeof(_2023.Day01).Assembly),
            2024 => GetExistingDaysFromAssembly(typeof(_2024.Day01).Assembly),
            _ => GetExistingDaysFromFolder(ChallengePaths.GetYearFolder(year))
        };

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
