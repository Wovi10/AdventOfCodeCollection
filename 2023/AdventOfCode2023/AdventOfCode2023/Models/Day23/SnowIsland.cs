namespace AdventOfCode2023_1.Models.Day23;

public class SnowIsland
{
    public SnowIsland(List<string> input)
    {
        Tiles = new List<Tile>();
        
        var y = 0;
        foreach (var inputLine in input)
        {
            var x = 0;
            foreach (var tile in inputLine)
            {
                Tiles.Add(new Tile(x, y, tile));
                x++;
            }

            y++;
        }
        
        StartTile = Tiles.First(t => t.Type == TileType.Path);
        EndTile = Tiles.Last(t => t.Type == TileType.Path);
    }

    public List<Tile> Tiles { get; set; }
    public Tile StartTile { get; set; }
    public Tile EndTile { get; set; }
    public List<Hike> Hikes { get; set; } = new();

    public int FindLongestHike()
    {
        var initialHike = new Hike();
        initialHike.AddTile(StartTile);
        Hikes.Add(initialHike);
        RunOverHikes(initialHike);
        
        return Hikes.Max(hike => hike.Length);
    }

    public void RunOverHikes(Hike currentHike)
    {
        if (!currentHike.IsPossible)
        {
            Hikes.Remove(currentHike);
            return;
        }

        var lastTile = currentHike.Tiles.Last();
        var neighbourTiles = GetPossibleNeighbourTiles(lastTile);
        
        if (neighbourTiles.Count == 0)
        {
            currentHike.IsPossible = false;
            Hikes.Remove(currentHike);
            return;
        }
        
        foreach (var neighbourTile in neighbourTiles)
        {
            var newHike = new Hike();
            newHike.Tiles.AddRange(currentHike.Tiles);
            newHike.AddTile(neighbourTile);

            Hikes.Add(newHike);
            RunOverHikes(newHike);
        }
    }

    private List<Tile> GetPossibleNeighbourTiles(Tile currentTile) 
        => currentTile.Type switch
            {
                TileType.SlopeToEast 
                    => Tiles
                        .Where(tile => tile.X == currentTile.X + 1 && tile.Y == currentTile.Y && tile.Type != TileType.Forest)
                        .ToList(),
                TileType.SlopeToSouth 
                    => Tiles
                        .Where(tile => tile.X == currentTile.X && tile.Y == currentTile.Y + 1 && tile.Type != TileType.Forest)
                        .ToList(),
                TileType.Path =>
                    Tiles
                        .Where(tile => tile.Type != TileType.Forest)
                        .Where(tile => (tile.X == currentTile.X - 1 && tile.Y == currentTile.Y) ||
                                                     (tile.X == currentTile.X + 1 && tile.Y == currentTile.Y) ||
                                                     (tile.X == currentTile.X && tile.Y == currentTile.Y - 1) ||
                                                     (tile.X == currentTile.X && tile.Y == currentTile.Y + 1))
                        .ToList(),
                _ => throw new ArgumentOutOfRangeException()
            };
}