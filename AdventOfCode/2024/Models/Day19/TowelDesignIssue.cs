namespace _2024.Models.Day19;

public class TowelDesignIssue
{
    private string[] AvailableTowelDesigns { get; set; }
    private string[] TowelDesigns { get; set; }

    public TowelDesignIssue(string[] input)
    {
        AvailableTowelDesigns = input.First().Split(", ");
        TowelDesigns =
            input
                .Skip(2)
                .Select(design => design.Trim())
                .ToArray();
    }

    public long GetNumberOfPossibleDesigns()
        => TowelDesigns.Where(IsPossible).Count();


    private readonly IDictionary<string, bool> _memoization = new Dictionary<string, bool>();
    private bool IsPossible(string design)
    {
        if (design.Length == 0)
            return true;

        if (_memoization.TryGetValue(design, out var possible))
            return possible;

        var options = AvailableTowelDesigns
            .Where(design.StartsWith);

        if (options.Select(option => design[option.Length..]).Any(IsPossible))
        {
            _memoization[design] = true;
            return true;
        }

        _memoization[design] = false;
        return false;
    }

    private readonly IDictionary<string, long> _countMemoization = new Dictionary<string, long>();
    public long GetTotalNumberOfPossibleDesignOptions()
    {
        return TowelDesigns.Sum(CountOptions);

        long CountOptions(string design)
        {
            if (design.Length == 0)
                return 1;

            if (_countMemoization.TryGetValue(design, out var cachedCount))
                return cachedCount;

            var options = AvailableTowelDesigns
                .Where(design.StartsWith);
            var total = options.Sum(option => CountOptions(design[option.Length..]));

            _countMemoization[design] = total;
            return total;
        }
    }
}