namespace AdventOfCode2023_1.Models.Day22;

public class Cube
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public override string ToString() 
        => $"{X},{Y},{Z}";

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var cube = (Cube) obj;
        return X == cube.X && Y == cube.Y && Z == cube.Z;
    }
}