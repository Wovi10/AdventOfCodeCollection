namespace _2023.Models.Day25;

public class Node(string name)
{
    public string Name { get; set; } = name;
    public int Value { get; set; } = 1;
    public List<Edge> Edges { get; set; } = new();

    public override string ToString() 
        => Name;
}