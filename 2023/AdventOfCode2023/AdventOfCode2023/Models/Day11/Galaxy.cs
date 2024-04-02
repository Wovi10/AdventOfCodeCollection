namespace AdventOfCode2023_1.Models.Day11;

public class Galaxy(int xCoordinate, int yCoordinate)
{
    public int XCoordinate { get; set; } = xCoordinate;
    public int XAfterEnlargement { get; set; } = xCoordinate;
    public int YCoordinate { get; set; } = yCoordinate;
    public int YAfterEnlargement { get; set; } = yCoordinate;
}