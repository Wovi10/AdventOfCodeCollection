namespace AdventOfCode2023_1.Models.Day05;

public class SeedMapping
{
    public SeedMapping(long sourceStart, long destinationStart, long range)
    {
        SourceStart = sourceStart;
        _destinationStart = destinationStart;
        SourceEnd = sourceStart + range - 1;
    }

    public readonly long SourceStart;
    private readonly long _destinationStart;

    public readonly long SourceEnd;

    public long? MapValue(long sourceValue)
    {
        if (sourceValue < SourceStart || sourceValue > SourceEnd)
            return null;
        return sourceValue - SourceStart + _destinationStart;
    }
}