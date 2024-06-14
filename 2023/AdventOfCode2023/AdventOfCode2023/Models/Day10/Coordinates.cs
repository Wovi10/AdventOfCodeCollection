using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day10;

public class Coordinates(int xCoordinate, int yCoordinate): NodeBase<int>(xCoordinate, yCoordinate)
{

    public override bool Equals(object? obj)
    {
        if (obj is Coordinates coordinates)
            return Equals(coordinates);

        return false;
    }

    public bool Equals(Coordinates other)
        => X == other.X && Y == other.Y;

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        throw new NotImplementedException();
    }
}