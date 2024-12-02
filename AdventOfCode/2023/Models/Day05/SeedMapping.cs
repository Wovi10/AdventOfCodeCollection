namespace _2023.Models.Day05;

public class SeedMapping(long sourceStart, long destinationStart, long range)
{
    public readonly long SourceStart = sourceStart;
    public readonly long SourceEnd = sourceStart + range - 1;

    public long MapValue(long sourceValue)
    {
        if (sourceValue < SourceStart || sourceValue > SourceEnd)
            return sourceValue;
        return sourceValue - SourceStart + destinationStart;
    }
}