using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2023.Models.Day10;

public class Coordinates(int xCoordinate, int yCoordinate) : NodeBase<int>(xCoordinate, yCoordinate)
{
    public static bool operator ==(Coordinates? left, Coordinates? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Coordinates left, Coordinates right)
        => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is Coordinates coordinates)
            return Equals(coordinates);

        return false;
    }

    public bool Equals(Coordinates? other)
    {
        if (other is null)
            return false;

        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}