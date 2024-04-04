using System.Reflection;
using AdventOfCode2023_1.Models.Day17.Enums;

namespace AdventOfCode2023_1.Models.Day17;

public class CityMap
{
    public CityMap(List<string> rows)
    {
        Rows = new List<List<int>>();
        foreach (var rowBlocks in rows.Select(row => row.Select(num => int.Parse(num.ToString())).ToList()))
        {
            Rows.Add(rowBlocks);
        }
        Height = Rows.Count;
        Width = Rows[0].Count;
        UsedBlocks = new List<Coordinates>();
        UsedBlocks.Add(_startCoordinates);
        EndCoordinates = new Coordinates(Rows[0].Count - 1, Rows.Count - 1);
    }

    public List<List<int>> Rows { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public List<Coordinates> UsedBlocks { get; set; } = new();
    private readonly Coordinates _startCoordinates = new Coordinates(0, 0);
    public Coordinates EndCoordinates { get; set; }

    public int GetBlockValue(Coordinates coordinates)
        => Rows[coordinates.GetYCoordinate()][coordinates.GetXCoordinate()];

    public async Task<int> GetMinimalHeatLoss(Direction inputDirection = Direction.Right, Coordinates coordinates = null, int heatLoss = 0, int directionUsed = 1)
    {
        coordinates ??= _startCoordinates;

        var xCoord = coordinates.GetXCoordinate();
        var yCoord = coordinates.GetYCoordinate();

        if (xCoord < 0 || xCoord >= Width || yCoord < 0 || yCoord >= Height)
            return heatLoss;

        var cityBlockHeatLoss = Rows[yCoord][xCoord];

        UsedBlocks.Add(coordinates);

        cityBlockHeatLoss.AddDirection(inputDirection);

        var newDirections = inputDirection.GetNewDirections(directionUsed);

        foreach (var direction in newDirections)
        {
            var newX = GetNewX(direction, xCoord);
            var newY = GetNewY(direction, yCoord);

            if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
                continue;

            heatLoss = await GetMinimalHeatLoss(direction, coordinates, Rows[yCoord][xCoord], directionUsed);
        }

        return heatLoss;
    }

    private static int GetNewX(Direction direction, int xCoord)
    {
        return direction switch
        {
            Direction.Up => xCoord - 1,
            Direction.Down => xCoord + 1,
            _ => xCoord
        };
    }
    
    private static int GetNewY(Direction direction, int yCoord)
    {
        return direction switch
        {
            Direction.Left => yCoord - 1,
            Direction.Right => yCoord + 1,
            _ => yCoord
        };
    }
}