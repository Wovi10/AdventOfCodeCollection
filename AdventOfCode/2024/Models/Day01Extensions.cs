using UtilsCSharp;
using UtilsCSharp.Utils;

namespace _2024.Models;

public static class Day01Extensions
{
    public static List<(int, int)> GetPairs(this List<string> input)
        => input
            .Select(line => line.Split(Constants.Space))
            .Select(ids => (int.Parse(ids.First()), int.Parse(ids.Last())))
            .ToList();

    public static Tuple<List<int>,List<int>> GetIdLists(this List<(int, int)> pairs)
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

    public static List<(int, int)> GetAppearanceCountList(this Tuple<List<int>, List<int>> idLists)
        => idLists.Item1
            .Select(id1 => (id1, idLists.Item2.Count(id2 => id2 == id1)))
            .ToList();

    public static List<int> GetSimilarityList(this List<(int, int)> appearanceCountList)
        => appearanceCountList
            .Select(pair => pair.Item1 * pair.Item2)
            .ToList();
}