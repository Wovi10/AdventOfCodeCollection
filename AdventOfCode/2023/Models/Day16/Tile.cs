﻿using _2023.Models.Day16.Enums;
using UtilsCSharp.Enums;

namespace _2023.Models.Day16;

public class Tile(char tileType)
{
    public TileType TileType { get; } = tileType.ToTileType();
    public bool IsEnergised;
    private readonly List<Direction> _directionsUsed = new();

    public void SetEnergised() 
        => IsEnergised = true;

    public bool ShouldStop(Direction inputDirection) 
        => _directionsUsed.Contains(inputDirection);

    public void AddDirection(Direction direction) 
        => _directionsUsed.Add(direction);

    public void ResetTile()
    {
        IsEnergised = false;
        _directionsUsed.Clear();
    }
}