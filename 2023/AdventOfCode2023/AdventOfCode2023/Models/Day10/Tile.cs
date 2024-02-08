using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day10;

public class Tile
{
    public Tile(char tileChar, int mazeLineCounter, int tileCounter, int mazeWidth, int mazeLength)
    {
        TileType = tileChar.ToTileType();
        Coordinates = new Coordinates(tileCounter, mazeLineCounter);

        NorthTile = mazeLineCounter > 0 ? new Coordinates(tileCounter, mazeLineCounter - 1).ToString() : Constants.EmptyString;
        EastTile = tileCounter < mazeWidth - 1 ? new Coordinates(tileCounter + 1, mazeLineCounter).ToString() : Constants.EmptyString;
        SouthTile = mazeLineCounter < mazeLength - 1 ? new Coordinates(tileCounter, mazeLineCounter + 1).ToString() : Constants.EmptyString;
        WestTile = tileCounter > 0 ? new Coordinates(tileCounter - 1, mazeLineCounter).ToString() : Constants.EmptyString;
    }

    public TileType TileType;
    public readonly string NorthTile;
    public readonly string EastTile;
    public readonly string SouthTile;
    public readonly string WestTile;
    public readonly List<Coordinates> AdjacentTiles = new();
    public int DistanceFromStart { get; set; }

    public readonly Coordinates Coordinates;
    public bool TriedEverything { get; set; }

    public void AddAdjacentTile(Coordinates? coordinates)
    {
        if (coordinates == null) 
            return;
        if(!AdjacentTiles.Contains(coordinates))
            AdjacentTiles.Add(coordinates);
    }

    public void SetDistanceFromStart(int currentDistance)
    {
        if (DistanceFromStart == 0) 
            DistanceFromStart = currentDistance;
        DistanceFromStart = MathUtil.GetLowest(currentDistance, DistanceFromStart);
    }
}