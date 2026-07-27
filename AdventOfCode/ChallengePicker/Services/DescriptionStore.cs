using System.Runtime.CompilerServices;

namespace ChallengePicker.Services;

// Challenge descriptions live in ChallengePicker itself, not AOC.Challenges -
// they're picker/reference data, not part of the solving logic.
public static class DescriptionStore
{
    private static string ProjectRoot { get; } = GetProjectRoot();

    private static string DescriptionsRoot => Path.Combine(ProjectRoot, "Descriptions");

    private static string GetFile(int year, int day)
        => Path.Combine(DescriptionsRoot, year.ToString(), $"Day{day:D2}.md");

    public static string Get(int year, int day)
    {
        var file = GetFile(year, day);
        return File.Exists(file) ? File.ReadAllText(file) : string.Empty;
    }

    public static void Save(int year, int day, string content)
    {
        var file = GetFile(year, day);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, content);
    }

    // sourceFile is .../ChallengePicker/Services/DescriptionStore.cs - go up one more
    // level to land on the ChallengePicker project root, not the Services folder.
    private static string GetProjectRoot([CallerFilePath] string sourceFile = "")
        => Path.GetDirectoryName(Path.GetDirectoryName(sourceFile)!)!;
}
