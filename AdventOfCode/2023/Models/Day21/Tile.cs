using _2023.Models.Day21.Enums;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace _2023.Models.Day21;

public class Tile(int internalX, int internalY, char tileTypeInput) : NodeBase<int>(internalX, internalY)
{
    public int ActualX { get; set; } = internalX;
    public int ActualY { get; set; } = internalY;
    public int StepCounter { get; set; }

    public TileType Type { get; set; } = tileTypeInput.ToTileType();
    public bool IsWalkable => Type != TileType.Rock;
    public bool Reachable { get; set; }

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

    public long GetHashCode()
    {
        unchecked
        {
            var hash = 17L;
            hash = hash * 23 + EqualityComparer<long>.Default.GetHashCode(ActualX);
            hash = hash * 23 + EqualityComparer<long>.Default.GetHashCode(ActualY);
            hash = hash * 23 + EqualityComparer<long>.Default.GetHashCode(StepCounter);
            return hash;
        }
    }
}