namespace AdventOfCode2023_1.Models.Day22;

public class Cube
{
    public Cube(string input)
    {
        var parts = input.Split(',');
        Width = int.Parse(parts[0]);
        Depth = int.Parse(parts[1]);
        Height = int.Parse(parts[2]);
    }

    public Cube()
    {
    }

    public int Width { get; set; }
    public int Depth { get; set; }
    public int Height { get; set; }

    public override string ToString() 
        => $"{Width},{Depth},{Height}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var cube = (Cube) obj;
        return Width == cube.Width && Depth == cube.Depth && Height == cube.Height;
    }
}