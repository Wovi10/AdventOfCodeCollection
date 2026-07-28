using AOC.Utils;

namespace AOC.Challenges.Years._2025;

public class Day02() : DayBase("02", "Gift Shop")
{
    protected override Task<object> PartOne()
    {
        var result = SumInvalidIds();
        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        throw new NotImplementedException();
    }

    private long SumInvalidIds()
    {
        var input = string.Concat(GetInput()).Split(',', StringSplitOptions.RemoveEmptyEntries);
        var invalidIds = new List<long>();

        foreach (var range in input)
        {
            var ids = range.Split('-');
            for (var i = long.Parse(ids[0]); i <= long.Parse(ids[1]); i++)
            {
                var id = i.ToString();
                if (IsInvalidId(id))
                    invalidIds.Add(i);
            }
        }

        return invalidIds.Sum();
    }

    private static bool IsInvalidId(string id)
    {
        if (id.Length % 2 != 0)
            return false;

        var halfLength = id.Length / 2;
        return id[..(halfLength)] == id[halfLength..];
    }
}
