using System.Data.Common;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day14;

public class Dish
{
    public Dish(List<string> input)
    {
        Rows = input.Select(l => l.Trim()).Select(line => new DishRow(line)).ToList();
        Columns = new List<DishRow>();
        for (var i = 0; i < Rows[0].Rocks.Count; i++)
        {
            var columnRocks = Rows.Select(row => row.Rocks[i]).ToList();
            Columns.Add(new DishRow(columnRocks));
        }
    }

    private List<DishRow> Rows { get; }
    private List<DishRow> Columns { get; }
    public long TotalLoad { get; private set; }

    public void CalculateTotalLoad()
    {
        var rowCounter = Rows.Count;
        foreach (var numRoundRocks in Rows.Select(dishRow => dishRow.GetIndicesOfRockType(RockType.Round).Count))
        {
            TotalLoad += numRoundRocks * rowCounter;
            rowCounter--;
        }
    }
    
    public void TiltNorth() 
        => Tilt(Direction.North);

    public void Cycle()
    {
        Tilt(Direction.North);
        Tilt(Direction.West);
        Tilt(Direction.South);
        Tilt(Direction.East);
    }

    private async void Tilt(Direction direction)
    {
        var tasks = direction switch
        {
            Direction.North => Columns.Select(column => column.TiltToStartAsync()),
            Direction.South => Columns.Select(column => column.TiltToEndAsync()),
            Direction.East => Rows.Select(row => row.TiltToEndAsync()),
            Direction.West => Rows.Select(row => row.TiltToStartAsync()),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        await Task.WhenAll(tasks);

        switch (direction)
        {
            case Direction.North:
            case Direction.South:
                AlignRows();
                break;
            case Direction.East:
            case Direction.West:
                AlignColumns();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private async void AlignRows()
    {
        await Task.WhenAll(
            AlignColumnsRockTypeAsync(RockType.Round, row => row.GetIndicesOfRockType(RockType.Round)),
            AlignColumnsRockTypeAsync(RockType.Square, row => row.GetIndicesOfRockType(RockType.Square)),
            AlignColumnsRockTypeAsync(RockType.None, row => row.GetIndicesOfRockType(RockType.None))
        );
    }

    private async Task AlignColumnsRockTypeAsync(RockType rockType, Func<DishRow, IEnumerable<int>> getIndicesFunc)
    {
        var tasks = Columns.Select(async row =>
        {
            var rowIndex = Columns.IndexOf(row);
            var indices = getIndicesFunc(row);
            foreach (var index in indices)
            {
                Rows[index].Rocks[rowIndex] = rockType;
            }

            await Task.CompletedTask;
        });
        
        await Task.WhenAll(tasks);
    }

    private async void AlignColumns()
    {
        await Task.WhenAll(
            AlignRowsRockTypeAsync(RockType.Round, row => row.GetIndicesOfRockType(RockType.Round)),
            AlignRowsRockTypeAsync(RockType.Square, row => row.GetIndicesOfRockType(RockType.Square)),
            AlignRowsRockTypeAsync(RockType.None, row => row.GetIndicesOfRockType(RockType.None))
        );
    }

    private async Task AlignRowsRockTypeAsync(RockType rockType, Func<DishRow, IEnumerable<int>> getIndicesFunc)
    {
        var tasks = Rows.Select(async row =>
        {
            var rowIndex = Rows.IndexOf(row);
            var indices = getIndicesFunc(row);
            foreach (var index in indices)
            {
                Columns[index].Rocks[rowIndex] = rockType;
            }

            await Task.CompletedTask;
        });
        
        await Task.WhenAll(tasks);
    }
}