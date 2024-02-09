using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day10;

public class Tile
{
    public Tile(char tileChar, int mazeLineCounter, int tileCounter, int mazeWidth, int mazeLength)
    {
        TileType = tileChar.ToTileType();
        var isTopLineAndNorth = mazeLineCounter == 0 &&
                                TileType is TileType.NorthEast or TileType.NorthSouth or TileType.NorthWest;
        var isLeftLineAndWest = tileCounter == 0 && 
                                TileType is TileType.EastWest or TileType.NorthWest or TileType.SouthWest;
        var isBottomLineAndSouth = mazeLineCounter == mazeLength - 1 &&
                                   TileType is TileType.SouthEast or TileType.SouthWest or TileType.NorthSouth;
        var isRightLineAndEast = tileCounter == mazeWidth - 1 &&
                                 TileType is TileType.EastWest or TileType.SouthEast or TileType.NorthEast;
        
        var pointsOut = isTopLineAndNorth || isLeftLineAndWest || isBottomLineAndSouth || isRightLineAndEast;

        if (TileType == TileType.Ground || pointsOut)
        {
            TileType = TileType.Ground;
            return;
        }

        Coordinates = new Coordinates(tileCounter, mazeLineCounter);

        NorthTile = mazeLineCounter > 0 ? new Coordinates(tileCounter, mazeLineCounter - 1) : null;
        EastTile = tileCounter < mazeWidth - 1 ? new Coordinates(tileCounter + 1, mazeLineCounter) : null;
        SouthTile = mazeLineCounter < mazeLength - 1 ? new Coordinates(tileCounter, mazeLineCounter + 1) : null;
        WestTile = tileCounter > 0 ? new Coordinates(tileCounter - 1, mazeLineCounter) : null;
    }

    public TileType TileType;
    public Coordinates? NorthTile;
    public Coordinates? EastTile;
    public Coordinates? SouthTile;
    public Coordinates? WestTile;
    public readonly List<Coordinates> AdjacentTiles = new();
    public int DistanceFromStart { get; set; }

    public readonly Coordinates Coordinates;
    public bool TriedEverything { get; set; }

    public void AddAdjacentTile(Coordinates? coordinates)
    {
        if (coordinates == null)
            return;
        if (!AdjacentTiles.Contains(coordinates))
            AdjacentTiles.Add(coordinates);
    }

    public void SetDistanceFromStart(int currentDistance)
    {
        if (DistanceFromStart == 0)
            DistanceFromStart = currentDistance;
        DistanceFromStart = MathUtils.GetLowest(currentDistance, DistanceFromStart);
    }
    
    public bool TileIsCoupled(Tile? adjacentTile)
    {
        return adjacentTile != null 
               && (NorthIsCoupled(adjacentTile) ||
                   EastIsCoupled(adjacentTile) ||
                   SouthIsCoupled(adjacentTile)||
                   WestIsCoupled(adjacentTile));
    }

    private bool NorthIsCoupled(Tile adjacentTile)
    {
        return IsCoupled(adjacentTile, Direction.North);
    }

    private bool EastIsCoupled(Tile adjacentTile)
    {
        return IsCoupled(adjacentTile, Direction.East);
    }

    private bool SouthIsCoupled(Tile adjacentTile)
    {
        return IsCoupled(adjacentTile, Direction.South);
    }
    private bool WestIsCoupled(Tile adjacentTile)
    {
        return IsCoupled(adjacentTile, Direction.West);
    }
    
    private bool IsCoupled(Tile adjacentTile, Direction direction)
    {
        var coordinates = Coordinates;

        return direction switch
        {
            Direction.North => NorthTile != null && Equals(adjacentTile.SouthTile, coordinates),
            Direction.East => EastTile != null && Equals(adjacentTile.WestTile, coordinates),
            Direction.South => SouthTile != null && Equals(adjacentTile.NorthTile, coordinates),
            Direction.West => WestTile != null && Equals(adjacentTile.EastTile, coordinates),
            _ => throw new ArgumentException("Invalid direction.")
        };
    }
}