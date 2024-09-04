using System.Security.Cryptography;

namespace AdventOfCode2023_1.Models.Day25;

public class Walker(Node node, Edge? edge = default, Walker? walker = default, int score = 0)
{
    public Node Node { get; set; } = node;
    public Edge? Edge { get; set; } = edge;
    public Walker? Previous { get; set; } = walker;
    public int Score { get; set; } = score;

    public Walker Fork(Node node, Edge edge)
        => new(node, edge, this, Score + 1);

    public List<Edge>? ExtractPath()
    {
        var path = new List<Edge>();

        if (Edge is null)
            return path;
        
        path.Add(Edge);
        var prev = Previous;
        while (prev is not null)
        {
            if (prev.Edge is null)
                break;

            path.Add(prev.Edge!);
            prev = prev.Previous;
        }

        path.Reverse();

        return path;
    }
}