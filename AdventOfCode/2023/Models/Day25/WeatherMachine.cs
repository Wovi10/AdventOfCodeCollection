using UtilsCSharp.Utils;

namespace _2023.Models.Day25;

public class WeatherMachine
{
    public WeatherMachine(List<string> lines)
    {
        foreach (var line in lines)
        {
            var parts = line.Split(Constants.Colon);
            var name = parts[0].Trim();
            var names = parts[1].Trim().Split(Constants.Space).Select(x => x.Trim()).ToList();

            var node = GetOrCreateNode(name, Lookup);
            foreach (var connectedName in names)
            {
                var connectedNode = GetOrCreateNode(connectedName, Lookup);
                var edge = new Edge(node, connectedNode);
                node.Edges.Add(edge);
                connectedNode.Edges.Add(edge);
            }
        }
    }
    private Dictionary<string, Node> Lookup { get; } = new();

    private static Node GetOrCreateNode(string name, Dictionary<string, Node> lookup)
    {
        if (!lookup.TryGetValue(name, out var node)) 
            lookup.Add(name, node = new(name));

        return node;
    }
    
    public List<Node> ReduceGraph(int edgesCount)
    {
        var nodes = Lookup.Values.ToList();
        while (nodes.Count > 2)
        {
            var wasMerged = false;
            foreach (var node in nodes.Where(node => node.Edges.Count > edgesCount))
            {
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

                    wasMerged = Merge(nodes, node, otherNode);
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

    private static bool Merge(List<Node> nodes, Node node, Node otherNode)
    {
        nodes.Remove(node);
        nodes.Remove(otherNode);
        nodes.Add(MergeNodes(node, otherNode));
     
        return true;
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