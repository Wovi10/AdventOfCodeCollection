using UtilsCSharp.Enums;

namespace AdventOfCode2023_1.Models.Day16;

public class Grid
{
    public Grid(List<string> input)
    {
        Width = input[0].Length;
        Height = input.Count;
        Rows = new List<List<Tile>>();
        foreach (var newRow in input.Select(row => row.Select(tile => new Tile(tile)).ToList()))
        {
            Rows.Add(newRow);
        }
    }

    private int Width { get; }
    private int Height { get; }
    private List<List<Tile>> Rows { get; }
    public int EnergisedTilesCount { get; private set; }

    public Task ChangeDirection(Direction inputDirection = Direction.Right, int x = 0, int y = 0)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return Task.CompletedTask;

        var cell = Rows[y][x];
        cell.SetEnergised();

        if (cell.ShouldStop(inputDirection))
            return Task.CompletedTask;

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
        
        return Task.CompletedTask;
    }

    private static int GetNewY(Direction direction, int y)
    {
        return direction switch
        {
            Direction.Up => y - 1,
            Direction.Down => y + 1,
            _ => y
        };
    }

    private static int GetNewX(Direction direction, int inputX)
    {
        return direction switch
        {
            Direction.Right => inputX + 1,
            Direction.Left => inputX - 1,
            _ => inputX
        };
    }

    public void ResetGrid()
    {
        foreach (var cell in Rows.SelectMany(row => row))
        {
            cell.ResetTile();
        }
    }

    public void SetEnergisedTilesCount() 
        => EnergisedTilesCount = Rows.SelectMany(row => row).Count(tile => tile.IsEnergised);
}