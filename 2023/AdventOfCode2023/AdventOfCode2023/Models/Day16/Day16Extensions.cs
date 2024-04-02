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
            Direction.Upwards => direction.GetNewDirectionForVertical(tileType),
            Direction.Right => direction.GetNewDirectionForHorizontal(tileType),
            Direction.Downwards => direction.GetNewDirectionForVertical(tileType),
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
            default:
                return direction switch
                {
                    Direction.Upwards => direction.GetNewDirectionForUpwards(tileType),
                    Direction.Downwards => direction.GetNewDirectionForDownwards(tileType),
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
                newDirections.Add(Direction.Upwards);
                newDirections.Add(Direction.Downwards);
                return newDirections;
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
                newDirections.Add(Direction.Upwards);
                break;
            case TileType.TopLeftToBottomRightMirror:
                newDirections.Add(Direction.Downwards);
                break;
            case TileType.VerticalSplitter:
                newDirections.Add(Direction.Upwards);
                newDirections.Add(Direction.Downwards);
                break;
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
                newDirections.Add(Direction.Downwards);
                break;
            case TileType.TopLeftToBottomRightMirror:
                newDirections.Add(Direction.Upwards);
                break;
            case TileType.VerticalSplitter:
                newDirections.Add(Direction.Upwards);
                newDirections.Add(Direction.Downwards);
                break;
            default:
                newDirections.Add(direction);
                break;
        }

        return newDirections;
    }
}