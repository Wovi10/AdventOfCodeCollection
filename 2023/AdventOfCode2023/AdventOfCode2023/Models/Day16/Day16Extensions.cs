using AdventOfCode2023_1.Models.Day16.Enums;
using UtilsCSharp.Enums;

namespace AdventOfCode2023_1.Models.Day16;

public static class Day16Extensions
{
    public static TileType ToTileType(this char cell)
    {
        return cell switch
        {
            '.' => TileType.EmptySpace,
            '\\' => TileType.TopLeftToBottomRightMirror,
            '/' => TileType.BottomLeftToTopRightMirror,
            '|' => TileType.VerticalSplitter,
            '-' => TileType.HorizontalSplitter,
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };
    }

    public static List<Direction> GetNewDirections(this Direction direction, TileType tileType)
    {
        if (tileType == TileType.EmptySpace)
            return new List<Direction> {direction};

        return direction switch
        {
            Direction.Up => direction.GetNewDirectionForVertical(tileType),
            Direction.Right => direction.GetNewDirectionForHorizontal(tileType),
            Direction.Down => direction.GetNewDirectionForVertical(tileType),
            Direction.Left => direction.GetNewDirectionForHorizontal(tileType),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static List<Direction> GetNewDirectionForVertical(this Direction direction, TileType tileType)
    {
        var newDirections = new List<Direction>();

        switch (tileType)
        {
            case TileType.VerticalSplitter:
                newDirections.Add(direction);
                return newDirections;
            case TileType.HorizontalSplitter:
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Left);
                return newDirections;
            case TileType.EmptySpace:
            case TileType.BottomLeftToTopRightMirror:
            case TileType.TopLeftToBottomRightMirror:
            default:
                return direction switch
                {
                    Direction.Up => direction.GetNewDirectionForUpwards(tileType),
                    Direction.Down => direction.GetNewDirectionForDownwards(tileType),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
        }
    }
    
    private static List<Direction> GetNewDirectionForHorizontal(this Direction direction, TileType tileType)
    {
        var newDirections = new List<Direction>();

        switch (tileType)
        {
            case TileType.HorizontalSplitter:
                newDirections.Add(direction);
                return newDirections;
            case TileType.VerticalSplitter:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Down);
                return newDirections;
            case TileType.EmptySpace:
            case TileType.BottomLeftToTopRightMirror:
            case TileType.TopLeftToBottomRightMirror:
            default:
                return direction switch
                {
                    Direction.Right => direction.GetNewDirectionForRight(tileType),
                    Direction.Left => direction.GetNewDirectionForLeft(tileType),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                };
        }
    }

    private static List<Direction> GetNewDirectionForUpwards(this Direction direction, TileType tileType)
    {
        var newDirections = new List<Direction>();
        switch (tileType)
        {
            case TileType.BottomLeftToTopRightMirror:
                newDirections.Add(Direction.Right);
                break;
            case TileType.TopLeftToBottomRightMirror:
                newDirections.Add(Direction.Left);
                break;
            case TileType.HorizontalSplitter:
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Left);
                break;
            case TileType.EmptySpace:
            case TileType.VerticalSplitter:
            default:
                newDirections.Add(direction);
                break;
        }

        return newDirections;
    }

    private static List<Direction> GetNewDirectionForRight(this Direction direction, TileType tileType)
    {
        var newDirections = new List<Direction>();
        switch (tileType)
        {
            case TileType.BottomLeftToTopRightMirror:
                newDirections.Add(Direction.Up);
                break;
            case TileType.TopLeftToBottomRightMirror:
                newDirections.Add(Direction.Down);
                break;
            case TileType.VerticalSplitter:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Down);
                break;
            case TileType.EmptySpace:
            case TileType.HorizontalSplitter:
            default:
                newDirections.Add(direction);
                break;
        }

        return newDirections;
    }
    
    private static List<Direction> GetNewDirectionForDownwards(this Direction direction, TileType tileType)
    {
        var newDirections = new List<Direction>();
        switch (tileType)
        {
            case TileType.BottomLeftToTopRightMirror:
                newDirections.Add(Direction.Left);
                break;
            case TileType.TopLeftToBottomRightMirror:
                newDirections.Add(Direction.Right);
                break;
            case TileType.HorizontalSplitter:
                newDirections.Add(Direction.Right);
                newDirections.Add(Direction.Left);
                break;
            case TileType.EmptySpace:
            case TileType.VerticalSplitter:
            default:
                newDirections.Add(direction);
                break;
        }

        return newDirections;
    }

    private static List<Direction> GetNewDirectionForLeft(this Direction direction, TileType tileType)
    {
        var newDirections = new List<Direction>();
        switch (tileType)
        {
            case TileType.BottomLeftToTopRightMirror:
                newDirections.Add(Direction.Down);
                break;
            case TileType.TopLeftToBottomRightMirror:
                newDirections.Add(Direction.Up);
                break;
            case TileType.VerticalSplitter:
                newDirections.Add(Direction.Up);
                newDirections.Add(Direction.Down);
                break;
            case TileType.EmptySpace:
            case TileType.HorizontalSplitter:
            default:
                newDirections.Add(direction);
                break;
        }

        return newDirections;
    }
}