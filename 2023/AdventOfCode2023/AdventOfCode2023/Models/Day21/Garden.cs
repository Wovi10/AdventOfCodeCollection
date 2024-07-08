using AdventOfCode2023_1.Models.Day21.Enums;

namespace AdventOfCode2023_1.Models.Day21;

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

            for (var x = 0; x < row.Length; x++)
            {
                var tileChar = row[x];

                tiles.Add(new Tile(x, y, tileChar));
            }
        }

        Tiles = tiles;
    }

    private List<Tile> Tiles { get; set; }
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
                var tile = Tiles.FirstOrDefault(t => t.X == x && t.Y == y);

                row += tile?.Type.ToTileChar() ?? '.';
            }

            Console.WriteLine(row);
        }
    }

    public long CalculateReachableGardenPlots(int numberOfSteps)
    {
        var startingPosition = Tiles.First(t => t.Type == TileType.StartingPosition);
        var reachableTiles = new List<(int, int)>();
        startingPosition.StepCounter = 0;

        startingPosition.StepNonRecursive(numberOfSteps, reachableTiles, Tiles);

        var distinctTiles = reachableTiles.Distinct().ToList();

        return distinctTiles.Count;
    }
}