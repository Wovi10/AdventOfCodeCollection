namespace AdventOfCode2023_1.Models.Day16;

public class Grid
{
    public Grid(List<string> input)
    {
        Width = input[0].Length;
        Height = input.Count;
        Rows = new List<List<Tile>>();
        Columns = new List<List<Tile>>();
        for (var y = 0; y < Height; y++)
        {
            var row = new List<Tile>();
            for (var x = 0; x < Width; x++)
            {
                row.Add(new Tile(input[y][x]));
            }
            Rows.Add(row);
        }
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public List<List<Tile>> Rows { get; set; }
    public List<List<Tile>> Columns { get; set; }

    public List<Beam> MoveBeam(Beam inputBeam, int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return new List<Beam> { inputBeam };

        var hasValid = true;
        while (hasValid)
        {
            hasValid = true;
            var newBeams = inputBeam.GetNewBeams(Rows[y][x]);
            foreach (var beam in newBeams)
            {
                var newX = GetNewX(beam, x);
                var newY = GetNewY(beam, y);
                if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
                {
                    
                }
                return MoveBeam(beam,);
            }
        }
        
    }

    private int GetNewY(Beam beam, int y)
    {
        return beam.Direction switch
        {
            Direction.Upwards => y - 1,
            Direction.Right => y,
            Direction.Downwards => y + 1,
            Direction.Left => y,
            _ => throw new ArgumentOutOfRangeException(nameof(beam.Direction), beam.Direction, null)
        };
    }

    private static int GetNewX(Beam beam, int inputX)
    {
        return beam.Direction switch
        {
            Direction.Upwards => inputX,
            Direction.Right => inputX + 1,
            Direction.Downwards => inputX,
            Direction.Left => inputX - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(beam.Direction), beam.Direction, null)
        };
    }
}