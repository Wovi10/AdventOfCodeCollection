using _2023.Models.Day21.Enums;

namespace _2023.Models.Day21;

public class Garden
{
    public Garden(List<string> input)
    {
        Width = input[0].Length;
        Height = input.Count;

        var tiles = new List<Tile>();

        for (var y = 0; y < input.Count; y++)
        {
            var row = input[y];
            tiles.AddRange(row.Select((tileChar, x) => new Tile(x, y, tileChar)));
        }

        Tiles = tiles;
        StartingTile = Tiles.First(t => t.Type == TileType.StartingPosition);
    }

    private List<Tile> Tiles { get; set; }
    public static Tile StartingTile { get; set; } = new(0,0, 'S');
    public static int Width { get; set; }
    public static int Height { get; set; }

    public void Print()
    {
        var maxX = Tiles.Max(t => t.X);
        var maxY = Tiles.Max(t => t.Y);

        for (var y = 0; y <= maxY; y++)
        {
            var row = string.Empty;

            for (var x = 0; x <= maxX; x++)
            {
                var tile = Tiles.First(t => t.X == x && t.Y == y);
                row += tile.Type.ToTileChar();
            }

            Console.WriteLine(row);
        }
    }

    public long CalculateReachableGardenPlots(int numberOfSteps)
    {
        var startTile = Tiles.First(t => t.Type == TileType.StartingPosition);
        var reachableTiles = new List<(int, int)>();
        
        var tilesDictionary = Tiles.ToDictionary(t => (t.X, t.Y), t => t);

        startTile.Step(numberOfSteps, reachableTiles, tilesDictionary);

        var distinctTiles = reachableTiles.Distinct().ToList();
        // reachableTiles = reachableTiles.MirrorAndAddTiles();

        PrintReachableTilesInGarden(distinctTiles);
        
        return distinctTiles.Count;
    }

    public void PrintReachableTilesInGarden(List<(int, int)> distinctTiles)
    {
        var maxX = distinctTiles.Max(t => t.Item1);
        var maxY = distinctTiles.Max(t => t.Item2);

        for (var y = 0; y <= maxY; y++)
        {
            var row = string.Empty;

            for (var x = 0; x <= maxX; x++)
            {
                var tile = distinctTiles.FirstOrDefault(t => t.Item1 == x && t.Item2 == y);
                row += tile != (0,0) ? 'O' : '.';
            }

            Console.WriteLine(row);
        }
    }
}