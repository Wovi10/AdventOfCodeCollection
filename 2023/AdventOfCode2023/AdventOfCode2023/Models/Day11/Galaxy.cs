using System.Numerics;
using UtilsCSharp;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day11;

public class Galaxy<T>(T x, T y): NodeBase<T>(x, y) where T : struct, INumber<T>
{
    public new T X { get; private set; } = x;
    public new T Y { get; private set; } = y;

    public override (T, T) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }

    public void Enlarge(int enlargementFactor, List<T> emptyColumns, List<T> emptyRows)
    {
        var enlargementSizeX = emptyColumns.Count(emptyColumn => X > emptyColumn) * enlargementFactor;
        X = X.Add(T.CreateChecked(enlargementSizeX));
        
        var enlargementSizeY = emptyRows.Count(emptyRow => Y > emptyRow) * enlargementFactor;
        Y = Y.Add(T.CreateChecked(enlargementSizeY));
    }
}