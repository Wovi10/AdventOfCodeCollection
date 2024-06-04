namespace AdventOfCode2023_1.Models.Day18;

public class NodeComparer : IEqualityComparer<Node>
{
    public bool Equals(Node? node1, Node? node2)
    {
        if (ReferenceEquals(node1, node2)) return true;
        if (ReferenceEquals(node1, null) || ReferenceEquals(node2, null)) return false;
        return node1.Coordinates.X == node2.Coordinates.X && node1.Coordinates.Y == node2.Coordinates.Y;
    }

    public int GetHashCode(Node obj)
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + obj.Coordinates.X.GetHashCode();
            hash = hash * 31 + obj.Coordinates.Y.GetHashCode();
            return hash;
        }
    }
}