using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ChallengePicker.Services;

public class DayAnswers
{
    public string Part1Mock { get; set; } = string.Empty;
    public string Part1Real { get; set; } = string.Empty;
    public string Part2Mock { get; set; } = string.Empty;
    public string Part2Real { get; set; } = string.Empty;
}

// Recorded answers live in ChallengePicker itself, next to descriptions - both are
// reference data about a day, not part of the solving logic in AOC.Challenges.
public static class AnswerStore
{
    public const string NotYetFound = "NotYetFound";
    public const string NotApplicable = "NotApplicable";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private static string ProjectRoot { get; } = GetProjectRoot();

    private static string AnswersRoot => Path.Combine(ProjectRoot, "Answers");

    private static string GetFile(int year, int day)
        => Path.Combine(AnswersRoot, year.ToString(), $"Day{day:D2}.json");

    public static DayAnswers Get(int year, int day)
    {
        var file = GetFile(year, day);
        if (!File.Exists(file))
            return new DayAnswers();

        return JsonSerializer.Deserialize<DayAnswers>(File.ReadAllText(file), JsonOptions) ?? new DayAnswers();
    }

    public static void Save(int year, int day, DayAnswers answers)
    {
        var file = GetFile(year, day);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, JsonSerializer.Serialize(answers, JsonOptions));
    }

    // sourceFile is .../ChallengePicker/Services/AnswerStore.cs - go up one more level
    // to land on the ChallengePicker project root, not the Services folder.
    private static string GetProjectRoot([CallerFilePath] string sourceFile = "")
        => Path.GetDirectoryName(Path.GetDirectoryName(sourceFile)!)!;
}
