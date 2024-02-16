namespace AdventOfCode2023_1.Models.Day11;

public class Galaxy
{
    public Galaxy(int xCoordinate, int yCoordinate)
    {
        XCoordinate = xCoordinate;
        XAfterEnlargement = xCoordinate;
        YCoordinate = yCoordinate;
        YAfterEnlargement = yCoordinate;
    }
    public int XCoordinate { get; set; }
    public int XAfterEnlargement { get; set; }
    public int YCoordinate { get; set; }
    public int YAfterEnlargement { get; set; }
}