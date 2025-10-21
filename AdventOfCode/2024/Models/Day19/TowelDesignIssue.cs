namespace _2024.Models.Day19;

public class TowelDesignIssue
{
    private string[] AvailableTowelDesigns { get; set; }
    private string[] TowelDesigns { get; set; }

    private readonly IDictionary<string, bool> _memoization = new Dictionary<string, bool>();

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
}