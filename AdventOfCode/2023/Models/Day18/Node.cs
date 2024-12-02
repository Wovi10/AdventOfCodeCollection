using System.Numerics;
using UtilsCSharp;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2023.Models.Day18;

public class Node<T>(T internalX, T internalY): NodeBase<T>(internalX, internalY) where T : struct, INumber<T>
{
    private Node((T, T) position) : this(position.Item1, position.Item2)
    {
    }

    public override (T, T) Move(Direction direction, int distance = 1)
    {
        var (xOffset, yOffset) = direction.ToOffset();
        xOffset *= distance;
        yOffset *= distance;

        return (MathUtils.Add(X, T.CreateChecked(xOffset)), MathUtils.Add(Y, T.CreateChecked(yOffset)));
    }

    public Node<T> MoveToNode(Direction direction, int distance = 1)
    {
        var newPosition = Move(direction, distance);

        return new Node<T>(newPosition);
    }
}