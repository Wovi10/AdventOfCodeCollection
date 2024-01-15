namespace AdventOfCode2023_1.Models;

public class SeedMapping(long sourceStart, long destinationStart, long range)
{
    private long SourceStart { get; } = sourceStart;
    private long DestinationStart { get; } = destinationStart;

    private long SourceEnd { get; } = sourceStart + range - 1;

    public bool IsInRange(long sourceValue) => sourceValue >= SourceStart && sourceValue <= SourceEnd;
        
    public long? MapValue(long sourceValue)
    {
        if (sourceValue < SourceStart || sourceValue > SourceEnd)
            return null;
        return sourceValue - SourceStart + DestinationStart;
    }
}