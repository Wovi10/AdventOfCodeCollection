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
    public readonly long SourceEnd;
    private readonly long _destinationStart;

    public long MapValue(long sourceValue)
    {
        if (sourceValue < SourceStart || sourceValue > SourceEnd)
            return sourceValue;
        return sourceValue - SourceStart + _destinationStart;
    }
}