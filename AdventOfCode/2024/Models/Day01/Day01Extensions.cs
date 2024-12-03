using UtilsCSharp;
using UtilsCSharp.Utils;

namespace _2024.Models.Day01;

public static class Day01Extensions
{
    public static IEnumerable<(int, int)> GetPairs(this IEnumerable<string> input)
        => input
            .Select(line => line.Split(Constants.Space))
            .Select(ids => (int.Parse(ids.First()), int.Parse(ids.Last())));

    public static Tuple<List<int>,List<int>> GetIds(this IEnumerable<(int, int)> pairs)
    {
        var distanceList1 = new List<int>();
        var distanceList2 = new List<int>();

        foreach (var (id1, id2) in pairs)
        {
            distanceList1.Add(id1);
            distanceList2.Add(id2);
        }

        return new Tuple<List<int>, List<int>>(distanceList1, distanceList2);
    }

    public static Tuple<List<int>,List<int>> Sort(this Tuple<List<int>,List<int>> idLists)
    {
        idLists.Item1.Sort();
        idLists.Item2.Sort();
        return idLists;
    }

    public static List<int> GetDistances(this Tuple<List<int>,List<int>> sortedIdLists)
        => sortedIdLists.Item1.Select((id1, i) => (sortedIdLists.Item2[i] - id1).Abs()).ToList();

    public static IEnumerable<(int, int)> GetAppearanceCounts(this Tuple<List<int>, List<int>> idLists)
        => idLists.Item1
            .Select(id1 => (id1, idLists.Item2.Count(id2 => id2 == id1)));

    public static IEnumerable<int> GetSimilarities(this IEnumerable<(int, int)> appearanceCountList)
        => appearanceCountList
            .Select(pair => pair.Item1 * pair.Item2);
}