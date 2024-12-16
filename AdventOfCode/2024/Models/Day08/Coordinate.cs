using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2024.Models.Day08;

public class Coordinate(int x, int y): NodeBase<int>(x, y)
{
    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }

    public (int, int) DistanceTo(Coordinate other)
        => (other.X - X, other.Y - Y);

    public Coordinate Move((int x, int y) distance)
        => new(X + distance.x, Y + distance.y);
}