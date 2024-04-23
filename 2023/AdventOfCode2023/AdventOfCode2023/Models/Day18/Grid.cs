using System.Text;
using AdventOfCode2023_1.Models.Day18.Enums;
using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day18;

public class Grid(List<Node> nodes, int width, int height)
{
    public List<Node> Nodes { get; } = nodes;
    private int Height { get; } = height;
    private int Width { get; } = width;

    public async Task DigHole()
    {
        var nodeSet = new HashSet<(int,int)>(Nodes.Select(n => (n.X, n.Y)));
        var edgeCrossedCalculators = new Dictionary<Direction, Func<Node, Task<int>>>
        {
            {Direction.Up, CountEdgesCrossedUpAsync},
            {Direction.Right, CountEdgesCrossedRightAsync},
            {Direction.Down, CountEdgesCrossedDownAsync},
            {Direction.Left, CountEdgesCrossedLeftAsync}
        };

        var tasks = new List<Task<Node?>>();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (nodeSet.Contains((x, y)))
                    continue;

                var newNode = new Node {X = x, Y = y};
                
                tasks.Add(Task.Run(async () =>
                {
                    var closestEdge = GetClosestEdge(newNode.X, newNode.Y);

                    var edgesCrossed = await edgeCrossedCalculators[closestEdge](newNode);

                    return edgesCrossed.IsOdd() ? newNode : null;
                }));
            }
        }

        var nodesToAdd = (await Task.WhenAll(tasks)).ToList().Where(node => node != null);

        foreach (var node in nodesToAdd) 
            Nodes.TryAddNode(node);
    }

    private async Task<int> CountEdgesCrossedUpAsync(Node node) 
        => await CountEdgesCrossed(node.Y - 1, 0, node.X, true, true);
    
    private async Task<int> CountEdgesCrossedRightAsync(Node node) 
        => await CountEdgesCrossed(node.X, Width, node.Y, false);    
    
    private async Task<int> CountEdgesCrossedDownAsync(Node node) 
        => await CountEdgesCrossed(node.Y, Height, node.X, true);

    private async Task<int> CountEdgesCrossedLeftAsync(Node node) 
        => await CountEdgesCrossed(node.X-1, 0, node.Y, false, true);

    private Direction GetClosestEdge(int x, int y)
    {
        var distanceDown = Height - y;
        var distanceRight = Width - x;
        
        var minDistance = MathUtils.GetLowest(MathUtils.GetLowest(y, distanceDown), MathUtils.GetLowest(x, distanceRight));
        
        if (minDistance == distanceRight)
            return Direction.Right;

        if (minDistance == distanceDown)
            return Direction.Down;

        return x < y 
                ? Direction.Left
                : Direction.Up;
    }

    private async Task<int> CountEdgesCrossed(int startPoint, int endPoint, int constantPart, bool isOnYAxis, bool shouldDecrement = false)
    {
        var edgesCrossed = 0;
        var previousWasEdge = false;

        var typesToSkip = new HashSet<NodeType>{NodeType.Enclosed};
        var startEdgeTypes = new HashSet<NodeType>();

        if (isOnYAxis)
        {
            typesToSkip.Add(NodeType.NorthSouth );

            startEdgeTypes.Add(NodeType.EastWest);
            startEdgeTypes.UnionWith(shouldDecrement
                ? new[] {NodeType.NorthEast, NodeType.NorthWest}
                : new[] {NodeType.SouthEast, NodeType.SouthWest});
        }
        else
        {
            typesToSkip.Add(NodeType.EastWest);

            startEdgeTypes.Add(NodeType.NorthSouth);
            startEdgeTypes.UnionWith(shouldDecrement
                ? new[] {NodeType.NorthWest, NodeType.SouthWest}
                : new[] {NodeType.NorthEast, NodeType.SouthEast});
        }

        var nodeSet = isOnYAxis
            ? new Dictionary<(int, int), Node>(Nodes.Where(node => node.X == constantPart).Select(n => new KeyValuePair<(int, int), Node>((n.X, n.Y), n)))
            : new Dictionary<(int, int), Node>(Nodes.Where(node => node.Y == constantPart).Select(n => new KeyValuePair<(int, int), Node>((n.X, n.Y), n)));

        var startOfWall = NodeType.Enclosed;

        for (var i = startPoint; ShouldStop(i, endPoint);)
        {
            var currentNode = isOnYAxis
                ? nodeSet.GetValueOrDefault((constantPart,i))
                : nodeSet.GetValueOrDefault((i, constantPart));

            if (shouldDecrement) i--;
            else i++;

            if (currentNode == null)
            {
                previousWasEdge = false;
                continue;
            }

            var currentNodeType = currentNode.Type;

            if (!previousWasEdge)
            {
                startOfWall = currentNodeType;
            }

            if (typesToSkip.Contains(currentNodeType))
            {
                previousWasEdge = true;
                continue;
            }

            if (currentNodeType.IsMatching(startOfWall, isOnYAxis) && previousWasEdge)
            {
                edgesCrossed--;
                continue;
            }

            if (startEdgeTypes.Contains(currentNodeType) && !previousWasEdge) 
                edgesCrossed++;

            previousWasEdge = true;
        }

        return edgesCrossed;
    }

    private static bool ShouldStop(int index, int endPoint)
        => endPoint == 0 ? index >= endPoint : index < endPoint;

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var node = Nodes.FirstOrDefault(n => n.X == x && n.Y == y);
                sb.Append(node == null ? '.' : '#');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public void DecideEdgeTypes()
    {
        foreach (var node in Nodes)
        {
            var neighbours = GetNeighbours(node.X, node.Y);
            node.DecideType(neighbours);
        }
    }

    private List<Direction> GetNeighbours(int x, int y)
    {
        var neighbours = new List<Direction>();
        var upNeighbour = Nodes.FirstOrDefault(n => n.X == x && n.Y == y - 1);
        var rightNeighbour = Nodes.FirstOrDefault(n => n.X == x + 1 && n.Y == y);
        var downNeighbour = Nodes.FirstOrDefault(n => n.X == x && n.Y == y + 1);
        var leftNeighbour = Nodes.FirstOrDefault(n => n.X == x - 1 && n.Y == y);

        if (upNeighbour != null)
            neighbours.Add(Direction.Up);

        if (rightNeighbour != null)
            neighbours.Add(Direction.Right);

        if (downNeighbour != null)
            neighbours.Add(Direction.Down);

        if (leftNeighbour != null)
            neighbours.Add(Direction.Left);

        return neighbours;
    }
}