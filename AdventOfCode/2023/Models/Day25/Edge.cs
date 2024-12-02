namespace _2023.Models.Day25;

public class Edge(Node node1, Node node2)
{
    public Node Node1 { get; set; } = node1;
    public Node Node2 { get; set; } = node2;

    public Node OtherNode(Node node) 
        => node == Node1 ? Node2 : Node1;
    
    public override string ToString() 
        => $"{Node1} <-> {Node2}";
}