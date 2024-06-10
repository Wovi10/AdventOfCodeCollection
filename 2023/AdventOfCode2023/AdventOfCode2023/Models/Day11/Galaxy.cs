using System.Numerics;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day11;

public class Galaxy<T>(T x, T y): NodeBase<T>(x, y) where T : struct, INumber<T>
{
    public new T X { get; set; } = x;
    public new T Y { get; set; } = y;
    public T XAfterEnlargement { get; set; } = x;
    public T YAfterEnlargement { get; set; } = y;

    public override (T, T) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}