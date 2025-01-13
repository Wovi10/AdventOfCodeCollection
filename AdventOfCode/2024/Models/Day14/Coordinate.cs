using UtilsCSharp.Enums;
using UtilsCSharp.Objects;
using UtilsCSharp.Utils;

namespace _2024.Models.Day14;

public class Coordinate:NodeBase<int>
{
    internal Coordinate(int internalX, int internalY) : base(internalX, internalY)
    {
    }

    public Coordinate(string input) : this(int.Parse(input.Split(Constants.Comma).First()), int.Parse(input.Split(Constants.Comma).Last()))
    {
    }

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}