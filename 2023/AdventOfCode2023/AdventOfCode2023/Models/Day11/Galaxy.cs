namespace AdventOfCode2023_1.Models.Day11;

public class Galaxy
{
    public Galaxy(int xCoordinate, int yCoordinate)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordinate;
    }
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
}