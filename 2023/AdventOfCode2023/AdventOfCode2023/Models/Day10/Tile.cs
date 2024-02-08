using AdventOfCode2023_1.Shared;

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
        DistanceFromStart = MathUtil.GetLowest(currentDistance, DistanceFromStart);
    }
}