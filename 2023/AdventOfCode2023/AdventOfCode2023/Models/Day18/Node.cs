using System.Numerics;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day18;

public class Node<T>(T internalX, T internalY): NodeBase<T>(internalX, internalY) where T : ISignedNumber<T>
{
    private Node((T, T) position) : this(position.Item1, position.Item2)
    {
    }

    public override (T, T) Move(Direction direction, int distance = 1)
    {
        var (xOffset, yOffset) = direction.ToOffset();
        xOffset *= distance;
        yOffset *= distance;

        return (Add(X, xOffset), Add(Y, yOffset));
    }

    public Node<T> MoveToNode(Direction direction, int distance = 1)
    {
        var newPosition = Move(direction, distance);

        return new Node<T>(newPosition);
    }
}