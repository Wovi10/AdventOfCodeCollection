using _2023.Models.Day15;
using UtilsCSharp.Utils;

namespace _2023;

public class Day15() : DayBase("15", "Lens Library")
{
    protected override async Task<object> PartOne()
    {
        var result = await GetSumHashes();

        return result;
    }

    protected override Task<object> PartTwo()
    {
        var result = GetFocusingPower();

        return Task.FromResult<object>(result);
    }

    private static async Task<int> GetSumHashes()
    {
        var steps = GetStepsFromInput();

        var hashes = await GetHashes(steps);

        return hashes.Sum();
    }

    private static async Task<int[]> GetHashes(List<string> steps)
    {
        var tasks = steps.Select(Day15Extensions.Hash).ToList();
        return await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private static List<string> GetStepsFromInput()
    {
        var input = Input[0].Split(Constants.Comma);
        return input.ToList();
    }

    private static long GetFocusingPower()
    {
        var boxes = new Box[256];
        for (var i = 0; i < 256; i++)
            boxes[i] = new Box();

        var steps = GetStepsFromInput();

        var lenses = steps.Select(step => new Lens(step)).ToList();
        lenses.ForEach(lens => lens.SetHash());

        foreach (var lens in lenses)
        {
            var boxToUse = boxes[lens.Hash];
            boxToUse.DoOperation(lens);
        }

        return boxes.Select((t, i) => t.GetFocusingPower(i + 1)).Sum();
    }
}