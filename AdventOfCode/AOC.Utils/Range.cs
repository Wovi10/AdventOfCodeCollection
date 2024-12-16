using System.Collections;
using System.Numerics;

namespace AOC.Utils;

public class Range<T> : IEnumerable<T> where T : INumber<T>
{
    public T Min { get; set; }
    public T Max { get; set; }
    public T Length { get; set; }

    public Range(T min, T max)
    {
        if (min > max)
            throw new ArgumentException("Min must be less than or equal to max");

        Min = min;
        Max = max;
        Length = max - min + T.One;
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}