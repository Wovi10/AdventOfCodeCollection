namespace AdventOfCode2023_1.Models.Day24;

public class Hail
{
    public Hail(string inputLine)
    {
        var parts = inputLine.Split(" @ ");
        var positionParts = parts[0].Split(", ");
        var velocityParts = parts[1].Split(", ");
        
        X = int.Parse(positionParts[0]);
        Y = int.Parse(positionParts[1]);
        Z = int.Parse(positionParts[2]);
        VelocityX = int.Parse(velocityParts[0]);
        VelocityY = int.Parse(velocityParts[1]);
        VelocityZ = int.Parse(velocityParts[2]);
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
    public int VelocityX { get; set; }
    public int VelocityY { get; set; }
    public int VelocityZ { get; set; }
}