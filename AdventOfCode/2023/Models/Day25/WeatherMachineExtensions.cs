namespace _2023.Models.Day25;

public static class WeatherMachineExtensions
{
    public static List<Node> ReduceGraph(this List<Node> nodes, int edgesCount)
    {
        while (nodes.Count > 2)
        {
            var wasMerged = false;
            foreach (var node in nodes)
            {
                if (node.Edges.Count <= edgesCount)
                    continue;

                foreach (var connection in node.Edges)
                {
                    var otherNode = connection.OtherNode(node);

                    if (otherNode.Edges.Count <= edgesCount)
                        continue;

                    var without = new HashSet<Edge> {connection};
                    var found3Paths = true;

                    for (var i = 1; i < edgesCount; i++)
                    {
                        var path = FindPath(node, otherNode, without);

                        if (path is null)
                        {
                            found3Paths = false;
                            break;
                        }

                        foreach (var edge in path)
                            without.Add(edge);
                    }

                    if (!found3Paths || !PathExists(node, otherNode, without)) 
                        continue;

                    wasMerged = true;
                    nodes.Remove(node);
                    nodes.Remove(otherNode);
                    nodes.Add(MergeNodes(node, otherNode));

                    break;
                }
                
                if (wasMerged)
                    break;
            }

            if (!wasMerged && nodes.Count != 2)
                throw new Exception("Failed to reduce graph");
        }
        
        return nodes;
    }

    private static Node MergeNodes(Node node, Node otherNode)
    {
        var merged = new Node("merged")
        {
            Value = node.Value + otherNode.Value
        };

        ConvertEdges(merged, node, otherNode);
        ConvertEdges(merged, otherNode, node);

        return merged;
    }

    private static void ConvertEdges(Node merged, Node node, Node otherNode)
    {
        foreach (var edge in node.Edges)
        {
            var other = edge.OtherNode(node);

            if (other == otherNode)
                continue;
            
            var newEdge = new Edge(merged, other);
            merged.Edges.Add(newEdge);
            other.Edges.Remove(edge);
            other.Edges.Add(newEdge);
        }
    }

    private static bool PathExists(Node node, Node otherNode, HashSet<Edge> without)
    {
        var queue = new Queue<Node>();
        var visited = new HashSet<Node>();

        queue.Enqueue(node);
        visited.Add(node);

        while (queue.TryDequeue(out var next))
        {
            foreach (var edge in next.Edges)
            {
                if (without.Contains(edge))
                    continue;

                var other = edge.OtherNode(next);

                if (other == otherNode)
                    return true;

                if (!visited.Add(other))
                    continue;

                queue.Enqueue(other);
            }
        }

        return false;
    }

    private static List<Edge>? FindPath(Node node, Node otherNode, HashSet<Edge> without)
    {
        var queue = new Queue<Walker>();
        queue.Enqueue(new Walker(node));

        var bestScore = int.MaxValue;
        var best = default(Walker);

        var scores = new Dictionary<Node, int>();
        while (queue.TryDequeue(out var walker))
        {
            if (walker.Score >= bestScore)
                continue;

            foreach (var edge in walker.Node.Edges)
            {
                if (without.Contains(edge))
                    continue;

                var other = edge.OtherNode(walker.Node);

                if (scores.TryGetValue(other, out var score) && score <= walker.Score + 1)
                    continue;

                var fork = walker.Fork(other, edge);
                scores[other] = fork.Score;

                if (other != otherNode)
                {
                    queue.Enqueue(fork);
                    continue;
                }

                if (fork.Score >= bestScore)
                    continue;

                bestScore = fork.Score;
                best = fork;
            }
        }

        return best?.ExtractPath();
    }
}