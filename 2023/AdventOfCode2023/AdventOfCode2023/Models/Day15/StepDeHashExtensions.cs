namespace AdventOfCode2023_1.Models.Day15;

public static class StepDeHashExtensions
{
    public static async Task<int> DeHash(this string step)
    {
        var tasks = step.Select(DeHash).ToList();
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);

        return results.Sum();
    }
    
    private static async Task<int> DeHash(this char character)
    {
        return await Task.FromResult(character);
    }
}