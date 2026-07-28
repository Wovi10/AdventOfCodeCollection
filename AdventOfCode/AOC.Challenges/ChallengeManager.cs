using System.Reflection;
using System.Text.RegularExpressions;
using AOC.Utils;
using AOC.Utils.Enums;

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

    // New years live in AOC.Challenges itself, so each gets its own namespace to keep
    // e.g. 2025's Day01 and 2026's Day01 from colliding.
    private static string GetManagedNamespace(int year) => $"AOC.Challenges.Years._{year}";

    public static void CreateDay(int year, int day, string? title = null)
    {
        if (IsLegacyYear(year))
            return;

        var yearFolder = ChallengePaths.GetManagedYearFolder(year);
        Directory.CreateDirectory(yearFolder);

        var dayFile = Path.Combine(yearFolder, $"Day{day:D2}.cs");
        if (File.Exists(dayFile))
            return;

        File.WriteAllText(dayFile, BuildDayStub(year, day, title));
    }

    private static string BuildDayStub(int year, int day, string? title)
    {
        var dayString = day.ToString("D2");
        var hasTitle = !string.IsNullOrWhiteSpace(title);
        var signature = hasTitle
            ? $"""DayBase("{dayString}", "{title!.Trim().Replace("\"", "\\\"")}")"""
            : $"""DayBase("{dayString}", "") // TODO Title""";

        return $$"""
            using AOC.Utils;

            namespace {{GetManagedNamespace(year)}};

            public class Day{{dayString}}() : {{signature}}
            {
                protected override Task<object> PartOne()
                {
                    throw new NotImplementedException();
                }

                protected override Task<object> PartTwo()
                {
                    throw new NotImplementedException();
                }
            }

            """;
    }

    public static async Task<ChallengeRunResult> RunPart(int year, int day, PartsToRun part, bool useMock = false)
    {
        var type = FindDayType(year, day);
        if (type is null)
            return ChallengeRunResult.Failure(
                $"No compiled implementation found for Day{day:D2} ({year}). " +
                "If you just created this day, rebuild and restart the app first.");

        try
        {
            var instance = (DayBase)Activator.CreateInstance(type)!;
            var result = await instance.RunPartForResult(part, useMock);
            return ChallengeRunResult.Ok(result?.ToString() ?? string.Empty);
        }
        catch (Exception ex)
        {
            return ChallengeRunResult.Failure(ex.Message);
        }
    }

    private static Type? FindDayType(int year, int day)
    {
        var dayName = $"Day{day:D2}";

        var (assembly, expectedNamespace) = year switch
        {
            2023 => (typeof(_2023.Day01).Assembly, "_2023"),
            2024 => (typeof(_2024.Day01).Assembly, "_2024"),
            _ => (typeof(ChallengeManager).Assembly, GetManagedNamespace(year))
        };

        return assembly.GetTypes().FirstOrDefault(t => t.Name == dayName && t.Namespace == expectedNamespace);
    }

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
