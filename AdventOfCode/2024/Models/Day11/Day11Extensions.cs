using UtilsCSharp.Utils;

namespace _2024.Models.Day11;

public static class Day11Extensions
{
    public static List<long> ToListOfLongs(this string input)
        => input.Split(Constants.Space).Select(long.Parse).ToList();

    public static long StartBlinking(this List<long> stones, int timesBlinked)
    {
        var stoneCounts = stones.GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());
        for (var i = 0; i < timesBlinked; i++)
        {
            var nextCounts = new Dictionary<long, long>();

            foreach (var (stone, count) in stoneCounts)
            {
                if (stone == 0)
                {
                    nextCounts.TryAdd(1, 0);
                    nextCounts[1] += count;
                    continue;
                }

                var numberOfDigits = stone.NumberOfDigits();
                if (numberOfDigits % 2 == 0)
                {
                    var divider = GetDivider(numberOfDigits);
                    var rightHalf = stone.GetRightHalf(divider);
                    var leftHalf = stone.GetLeftHalf(rightHalf, divider);

                    nextCounts.TryAdd(leftHalf, 0);
                    nextCounts.TryAdd(rightHalf, 0);

                    nextCounts[leftHalf] += count;
                    nextCounts[rightHalf] += count;
                    continue;
                }

                var newStone = stone * 2024;

                nextCounts.TryAdd(newStone, 0);
                nextCounts[newStone] += count;
            }

            stoneCounts = nextCounts;
        }

        return stoneCounts.Values.Sum();
    }

    private static int NumberOfDigits(this long number)
        => (int)Math.Floor(Math.Log10(number) + 1);

    private static int GetDivider(int numberOfDigits)
        => (int) Math.Pow(10, numberOfDigits / 2);

    private static long GetRightHalf(this long number, int divider)
        => number % divider;

    private static long GetLeftHalf(this long number, long rightHalf, long divider)
        => (number - rightHalf) / divider;
}