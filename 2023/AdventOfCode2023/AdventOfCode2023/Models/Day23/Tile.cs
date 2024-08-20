using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day23;

public class Tile(int internalX, int internalY, char type) : NodeBase<int>(internalX, internalY)
{
    public ITileType Type { get; init; } = type.ToTileType();

    public List<Tile> GetPossibleNeighbourTiles(List<Tile> tiles, Hike currentHike)
        => Type.GetPossibleNeighbourTiles(tiles, this, currentHike);

    public override (int, int) Move(Direction direction, int distance = 1) 
        => direction switch 
            {
                Direction.Up => (X, Y - distance),
                Direction.Right => (X + distance, Y),
                Direction.Down => (X, Y + distance),
                Direction.Left => (X - distance, Y),
                _ => (X, Y)
            };
}