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


    public Direction Facing { get; set; } = Direction.Right;
    public Coordinate Position { get; set; }

    public void Move(int distance = 1)
        => Position = new Coordinate(Position.Move(Facing, distance));

    public void TurnLeft()
    {
        Facing = Facing switch
        {
            Direction.Up => Direction.Left,
            Direction.Left => Direction.Down,
            Direction.Down => Direction.Right,
            Direction.Right => Direction.Up,
            _ => Facing
        };
    }

    public void TurnRight()
    {
        Facing = Facing switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => Facing
        };
    }

    public Coordinate GetPositionInFront()
        => new(Position.Move(Facing));

    public ReindeerPositioning[] GetNeighbouringCoordinatesWithDirection()
        => Position.GetNeighbouringCoordinatesWithDirection();

    public Direction OppositeDirection()
    {
        return Facing switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => Facing
        };
    }
}