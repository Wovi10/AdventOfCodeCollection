using AdventOfCode2023_1.Models.Day21.Enums;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day21;

public class Tile(int internalX, int internalY, char tileTypeInput) : NodeBase<int>(internalX, internalY)
{
    private readonly int _internalX = internalX;
    private readonly int _internalY = internalY;
    private int ActualX { get; set; } = internalX;
    private int ActualY { get; set; } = internalY;
    public int StepCounter { get; set; }

    public TileType Type { get; set; } = tileTypeInput.ToTileType();
    public bool IsWalkable => Type != TileType.Rock;

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        return direction switch
        {
            Direction.Up => (ActualX, ActualY - distance),
            Direction.Right => (ActualX + distance, ActualY),
            Direction.Down => (ActualX, ActualY + distance),
            Direction.Left => (ActualX - distance, ActualY),
            _ => (ActualX, ActualY)
        };
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(X);
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(Y);
            hash = hash * 23 + EqualityComparer<int>.Default.GetHashCode(StepCounter);
            return hash;
        }
    }
}