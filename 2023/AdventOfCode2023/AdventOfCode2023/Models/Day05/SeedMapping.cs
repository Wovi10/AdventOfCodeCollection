namespace AdventOfCode2023_1.Models.Day05;

public class SeedMapping
{
    public SeedMapping(long sourceStart, long destinationStart, long range)
    {
        _sourceStart = sourceStart;
        _destinationStart = destinationStart;
        _sourceEnd = sourceStart + range - 1;
    }

    private readonly long _sourceStart;
    private readonly long _destinationStart;

    private readonly long _sourceEnd;

    public bool IsInRange(long sourceValue) => sourceValue >= _sourceStart && sourceValue <= _sourceEnd;

    public long? MapValue(long sourceValue)
    {
        if (sourceValue < _sourceStart || sourceValue > _sourceEnd)
            return null;
        return sourceValue - _sourceStart + _destinationStart;
    }
}