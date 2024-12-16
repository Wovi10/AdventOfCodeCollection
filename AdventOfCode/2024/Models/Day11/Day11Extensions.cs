using UtilsCSharp.Utils;

namespace _2024.Models.Day11;

public static class Day11Extensions
{
    public static List<long> ToListOfLongs(this string input)
        => input.Split(Constants.Space).Select(long.Parse).ToList();

    public static long StartBlinking(this List<long> stones, int timesBlinked)
    {
        for (var i = 0; i < timesBlinked; i++)
            stones = stones.Blink();

        return stones.Count;
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
}