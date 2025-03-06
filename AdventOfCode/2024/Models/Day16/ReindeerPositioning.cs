using UtilsCSharp.Enums;

namespace _2024.Models.Day16;

public class ReindeerPositioning
{
    public ReindeerPositioning(Coordinate startingPosition, Direction direction)
    {
        Position = startingPosition;
        Facing = direction;
    }

    public ReindeerPositioning(Coordinate startingPosition)
    {
        Position = startingPosition;
    }

    public Direction Facing { get; } = Direction.Right;
    public Coordinate Position { get; }

    public ReindeerPositioning[] GetNeighbouringCoordinatesWithDirection()
        => Position.GetNeighbouringCoordinatesWithDirection();

    private Direction OppositeDirection()
        => Facing switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => Facing
        };

    public bool IsOppositeDirection(Direction reindeerPositioningFacing)
        => reindeerPositioningFacing == OppositeDirection();
}