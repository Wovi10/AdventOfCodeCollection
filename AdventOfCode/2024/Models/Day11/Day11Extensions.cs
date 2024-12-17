using UtilsCSharp.Utils;

namespace _2024.Models.Day11;

public static class Day11Extensions
{
    public static List<long> ToListOfLongs(this string input)
        => input.Split(Constants.Space).Select(long.Parse).ToList();

    public static async Task<long> StartBlinking(this List<long> stones, int timesBlinked)
    {
        var result = 0L;

        stones.ForEach(async stone => result += await stone.BlinkStone(timesBlinked));

        // foreach (var stone in stones)
        //     result += await stone.BlinkStone(timesBlinked);

        return result;
    }

    private static List<long> Blink(this List<long> stones)
    {
        var newStones = new List<long>();

        foreach (var stone in stones)
        {
            if (stone == 0)
            {
                newStones.Add(1);
                continue;
            }

            if (stone.NumberOfDigits() % 2 == 0)
            {
                var leftHalfOfDigits = stone.ToString()[..(stone.NumberOfDigits() / 2)];
                var rightHalfOfDigits = stone.ToString()[(stone.NumberOfDigits() / 2)..];
                newStones.Add(long.Parse(leftHalfOfDigits));
                newStones.Add(long.Parse(rightHalfOfDigits));
                continue;
            }

            newStones.Add(stone * 2024);
        }

        return newStones;
    }

    private static int NumberOfDigits(this long number)
        => number.ToString().Length;

    private static Task<long> BlinkStone(this long number, int timesBlinked)
    {
        var result = 0L;
        var numberAsList = new List<long> { number };

        for (var i = 0; i < timesBlinked; i++)
        {
            numberAsList = numberAsList.Blink();
        }

        result += numberAsList.Count;

        return Task.FromResult(result);
    }
}