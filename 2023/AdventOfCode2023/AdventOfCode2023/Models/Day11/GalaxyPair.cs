namespace AdventOfCode2023_1.Models.Day11;

public class GalaxyPair
{
    public int ManhattanDistance;
    private readonly Galaxy _galaxy1;
    private readonly Galaxy _galaxy2;

    public GalaxyPair(Galaxy galaxy, Galaxy galaxy2)
    {
        _galaxy1 = galaxy;
        _galaxy2 = galaxy2;
    }

    public void SetManhattanDistance()
    {
        ManhattanDistance = Math.Abs(_galaxy1.XCoordinate - _galaxy2.XCoordinate) +
                             Math.Abs(_galaxy1.YCoordinate - _galaxy2.YCoordinate);
    }
}