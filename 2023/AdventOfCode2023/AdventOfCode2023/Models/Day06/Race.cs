namespace AdventOfCode2023_1.Models.Day06;

public class Race
{
    public readonly int DurationInt;
    public readonly int RecordInt;
    public int DistanceTravelledInt { get; set; }

    public readonly long DurationLong;
    public readonly long RecordLong;
    
    public long DistanceTravelledLong { get; set; }
    
    public Race(int durationInt, int recordInt)
    {
        DurationInt = durationInt;
        RecordInt = recordInt;
    }

    public Race(long durationLong, long recordLong)
    {
        DurationLong = durationLong;
        RecordLong = recordLong;
    }
}