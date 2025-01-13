using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day15;

public class Coordinate:NodeBase<int>
{
    public Coordinate(int internalX, int internalY) : base(internalX, internalY)
    {
    }

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}