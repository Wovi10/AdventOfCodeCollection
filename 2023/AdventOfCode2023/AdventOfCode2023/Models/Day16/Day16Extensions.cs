namespace AdventOfCode2023_1.Models.Day16;

public static class Day16Extensions
{
    public static TileType ToTileType(this char cell)
    {
        return cell switch
        {
            '.' => TileType.EmptySpace,
            '\\' => TileType.BottomLeftToBottomRightMirror,
            '/' => TileType.BottomRightToBottomLeftMirror,
            '|' => TileType.VerticalSplitter,
            '-' => TileType.HorizontalSplitter,
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };
    }

    public static List<Beam> GetNewBeams(this Beam beam, Tile tile)
    {
        return beam.Direction switch
        {
            Direction.Upwards => beam.GetNewDirectionForUpwards(tile),
            Direction.Right => beam.GetNewDirectionForRight(tile),
            Direction.Downwards => beam.GetNewDirectionForDownwards(tile),
            Direction.Left => beam.GetNewDirectionForLeft(tile),
            _ => throw new ArgumentOutOfRangeException(nameof(beam.Direction), beam.Direction, null)
        };
    }

    private static List<Beam> GetNewDirectionForUpwards(this Beam beam, Tile tile)
    {
        switch (tile.TileType)
        {
            case TileType.EmptySpace or TileType.VerticalSplitter:
                return new List<Beam>{beam};
            case TileType.BottomLeftToBottomRightMirror:
                beam.Direction = Direction.Right;
                return new List<Beam>{beam};
            case TileType.BottomRightToBottomLeftMirror:
                beam.Direction = Direction.Left;
                return new List<Beam>{beam};
        }

        var beam1 = new Beam {Direction = Direction.Left};
        var beam2 = new Beam {Direction = Direction.Right};
        return new List<Beam>{beam1, beam2};
    }
    
    private static List<Beam> GetNewDirectionForRight(this Beam beam, Tile tile)
    {
        switch (tile.TileType)
        {
            case TileType.EmptySpace or TileType.HorizontalSplitter:
                return new List<Beam>{beam};
            case TileType.BottomLeftToBottomRightMirror:
                beam.Direction = Direction.Downwards;
                return new List<Beam>{beam};
            case TileType.BottomRightToBottomLeftMirror:
                beam.Direction = Direction.Upwards;
                return new List<Beam>{beam};
        }

        var beam1 = new Beam {Direction = Direction.Upwards};
        var beam2 = new Beam {Direction = Direction.Downwards};
        return new List<Beam>{beam1, beam2};
    }
    
    private static List<Beam> GetNewDirectionForDownwards(this Beam beam, Tile tile)
    {
        switch (tile.TileType)
        {
            case TileType.EmptySpace or TileType.VerticalSplitter:
                return new List<Beam>{beam};
            case TileType.BottomLeftToBottomRightMirror:
                beam.Direction = Direction.Left;
                return new List<Beam>{beam};
            case TileType.BottomRightToBottomLeftMirror:
                beam.Direction = Direction.Right;
                return new List<Beam>{beam};
        }

        var beam1 = new Beam {Direction = Direction.Right};
        var beam2 = new Beam {Direction = Direction.Left};
        return new List<Beam>{beam1, beam2};
    }
    
    private static List<Beam> GetNewDirectionForLeft(this Beam beam, Tile tile)
    {
        switch (tile.TileType)
        {
            case TileType.EmptySpace or TileType.HorizontalSplitter:
                return new List<Beam>{beam};
            case TileType.BottomLeftToBottomRightMirror:
                beam.Direction = Direction.Upwards;
                return new List<Beam>{beam};
            case TileType.BottomRightToBottomLeftMirror:
                beam.Direction = Direction.Downwards;
                return new List<Beam>{beam};
        }

        var beam1 = new Beam {Direction = Direction.Downwards};
        var beam2 = new Beam {Direction = Direction.Upwards};
        return new List<Beam>{beam1, beam2};
    }
}