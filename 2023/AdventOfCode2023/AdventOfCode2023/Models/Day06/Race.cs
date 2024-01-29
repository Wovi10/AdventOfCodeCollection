namespace AdventOfCode2023_1.Models.Day06;

public class Race
{
    public readonly int Duration;
    public readonly int Record;
    public int WaysToWin { get; set; }
    public int TimeHeld { get; set; }
    public int DistanceTravelled { get; set; }
    public int DistancePerMs { get; set; }

    public Race(int duration, int record)
    {
        Duration = duration;
        Record = record;
    }
}