using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day04() : DayBase("04", "Printing Department")
{
    protected override Task<object> PartOne()
    {
        long result = CountAccessibleRolls();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = CountTotalAccessibleRolls();

        return Task.FromResult<object>(result);
    }

    private const char PaperRoll = '@';

    private long CountAccessibleRolls()
    {
        var input = GetInput().ToArray();
        var result = 0;

        for (var y = 0; y < input.Length; y++)
        {
            var previousRow = y > 0 ? input[y-1] : null;
            var currentRow = input[y];
            var nextRow = y < input.Length - 1 ? input[y+1] : null;

            for (var x = 0; x < currentRow.Length; x++)
            {
                if (currentRow[x] != PaperRoll)
                    continue;

                var rollCount = 0;
                if (previousRow is not null)
                {
                    rollCount += (x > 0 && previousRow[x - 1] == PaperRoll) ? 1 : 0;
                    rollCount += (previousRow[x] == PaperRoll) ? 1 : 0;
                    rollCount += (x < previousRow.Length - 1 && previousRow[x+1] == PaperRoll) ? 1 : 0;
                }

                rollCount += x > 0 && currentRow[x-1] == PaperRoll ? 1 : 0;
                rollCount += (x < currentRow.Length - 1 && currentRow[x+1] == PaperRoll) ? 1 : 0;

                if (nextRow is not null)
                {
                    rollCount += (x > 0 && nextRow[x - 1] == PaperRoll) ? 1 : 0;
                    rollCount += (nextRow[x] == PaperRoll) ? 1 : 0;
                    rollCount += (x < nextRow.Length - 1 && nextRow[x+1] == PaperRoll) ? 1 : 0;
                }

                result += rollCount < 4 ? 1 : 0;
            }
        }

        return result;
    }

    private long CountTotalAccessibleRolls()
    {
        var input = GetInput().ToArray();

        var hasChanges = true;
        var result = 0;
        while (hasChanges)
        {
            var removableCoords = GetRemovableCoords(input);
            hasChanges = removableCoords.Length > 0;
            result += removableCoords.Length;

            input = RemoveCoords(input, removableCoords);
        }

        return result;
    }

    private (int y, int x)[] GetRemovableCoords(string[] input)
    {
        var result = new List<(int y, int x)>();
        for (var y = 0; y < input.Length; y++)
        {
            var previousRow = y > 0 ? input[y-1] : null;
            var currentRow = input[y];
            var nextRow = y < input.Length - 1 ? input[y+1] : null;

            for (var x = 0; x < currentRow.Length; x++)
            {
                if (currentRow[x] != PaperRoll)
                    continue;

                var rollCount = 0;
                if (previousRow is not null)
                {
                    rollCount += (x > 0 && previousRow[x - 1] == PaperRoll) ? 1 : 0;
                    rollCount += (previousRow[x] == PaperRoll) ? 1 : 0;
                    rollCount += (x < previousRow.Length - 1 && previousRow[x+1] == PaperRoll) ? 1 : 0;
                }

                rollCount += x > 0 && currentRow[x-1] == PaperRoll ? 1 : 0;
                rollCount += (x < currentRow.Length - 1 && currentRow[x+1] == PaperRoll) ? 1 : 0;

                if (nextRow is not null)
                {
                    rollCount += (x > 0 && nextRow[x - 1] == PaperRoll) ? 1 : 0;
                    rollCount += (nextRow[x] == PaperRoll) ? 1 : 0;
                    rollCount += (x < nextRow.Length - 1 && nextRow[x+1] == PaperRoll) ? 1 : 0;
                }

                if (rollCount < 4)
                    result.Add((y, x));
            }
        }

        return result.ToArray();
    }

    private static string[] RemoveCoords(string[] input, (int y, int x)[] removableCoords)
    {
        foreach (var removableCoord in removableCoords)
        {
            var row = input[removableCoord.y];
            var newRow = row.Remove(removableCoord.x, 1).Insert(removableCoord.x, ".");
            input[removableCoord.y] = newRow;
        }
        return input;
    }
}
