using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day23;

public interface ITileType
{
    public List<Tile> GetPossibleNeighbourTiles(List<Tile> tiles, Tile currentTile, Hike currentHike) 
        => Variables.RunningPartOne
            ? GetPossibleNeighbourTilesPart1(tiles, currentTile, currentHike)
            : GetPossibleNeighbourTilesPart2(tiles, currentTile, currentHike);

    protected List<Tile> GetPossibleNeighbourTilesPart1(List<Tile> tiles, Tile currentTile, Hike currentHike);
    
    protected List<Tile> GetPossibleNeighbourTilesPart2(List<Tile> tiles, Tile currentTile, Hike currentHike);
    
}