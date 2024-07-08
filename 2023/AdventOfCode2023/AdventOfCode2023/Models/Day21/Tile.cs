﻿using AdventOfCode2023_1.Models.Day21.Enums;
using UtilsCSharp.Enums;
using UtilsCSharp.Objects;

namespace AdventOfCode2023_1.Models.Day21;

public class Tile(int internalX, int internalY, char tileTypeInput) : NodeBase<int>(internalX, internalY)
{
    private readonly int _internalX = internalX;
    private readonly int _internalY = internalY;

    public TileType Type { get; set; } = tileTypeInput.ToTileType();
    public bool IsWalkable => Type != TileType.Rock;

    public override (int, int) Move(Direction direction, int distance = 1)
    {
        return direction switch
        {
            Direction.Up => (_internalX, _internalY - distance),
            Direction.Right => (_internalX + distance, _internalY),
            Direction.Down => (_internalX, _internalY + distance),
            Direction.Left => (_internalX - distance, _internalY),
            _ => (_internalX, _internalY)
        };
    }
}