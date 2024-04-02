namespace AdventOfCode2023_1.Models.Day16;

public class Grid
{
    public Grid(List<string> input)
    {
        Width = input[0].Trim().Length;
        Height = input.Count;
        Rows = new List<List<Tile>>();
        foreach (var row in input)
        {
            var newRow = row.Trim().Select(tile => new Tile(tile)).ToList();
            Rows.Add(newRow);
        }
    }

    public int Width { get; set; }
    public int Height { get; set; }
    public List<List<Tile>> Rows { get; set; }

    public void ChangeDirection(Direction inputDirection = Direction.Right, int x = 0, int y = 0)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return;

        var cell = Rows[y][x];
        cell.SetEnergised();

        if (cell.ShouldStop(inputDirection))
            return;

        cell.AddDirection(inputDirection);

        var newDirections = inputDirection.GetNewDirections(cell.TileType);

        foreach (var direction in newDirections)
        {
            var newX = GetNewX(direction, x);
            var newY = GetNewY(direction, y);

            if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
                continue;

            ChangeDirection(direction, newX, newY);
        }
    }

    private static int GetNewY(Direction direction, int y)
    {
        return direction switch
        {
            Direction.Upwards => y - 1,
            Direction.Right => y,
            Direction.Downwards => y + 1,
            Direction.Left => y,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static int GetNewX(Direction direction, int inputX)
    {
        return direction switch
        {
            Direction.Upwards => inputX,
            Direction.Right => inputX + 1,
            Direction.Downwards => inputX,
            Direction.Left => inputX - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}