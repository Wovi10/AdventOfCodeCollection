namespace AdventOfCode2023_1.Models.Day11;

public class GalaxyPair
{
    public GalaxyPair(Galaxy galaxy1, Galaxy galaxy2)
    {
        Galaxy1 = galaxy1;
        Galaxy2 = galaxy2;
    }

    public Galaxy Galaxy1 { get; set; }
    public Galaxy Galaxy2 { get; set; }
    
    public int GetManhattanDistance()
    {
        return Math.Abs(Galaxy1.XCoordinate - Galaxy2.XCoordinate) + Math.Abs(Galaxy1.YCoordinate - Galaxy2.YCoordinate);
    }
}