namespace AdventOfCode2023_1.Models.Day11;

public class GalaxyPair(Galaxy galaxy, Galaxy galaxy2)
{
    public long ManhattanDistance;

    public void SetManhattanDistance()
    {
        ManhattanDistance = Math.Abs(galaxy.XCoordinate - galaxy2.XCoordinate) +
                            Math.Abs(galaxy.YCoordinate - galaxy2.YCoordinate);
    }
}