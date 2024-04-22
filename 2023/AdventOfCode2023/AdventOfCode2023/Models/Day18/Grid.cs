namespace AdventOfCode2023_1.Models.Day18;

public class Grid
{
    public Grid(List<Node> nodes, int width, int height)
    {
        Nodes = nodes;
        Width = width;
        Height = height;
    }

    public List<Node> Nodes { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
}