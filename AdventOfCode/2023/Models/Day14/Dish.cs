using UtilsCSharp.Enums;

namespace _2023.Models.Day14;

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
        TotalLoad = 0;
        var rowCounter = Rows.Count;
        foreach (var numRoundRocks in Rows.Select(dishRow => dishRow.GetIndicesOfRockType(true).Count))
        {
            TotalLoad += numRoundRocks * rowCounter;
            rowCounter--;
        }
    }

    public void TiltNorth()
        => Tilt(WindDirection.North);

    public long Cycle()
    {
        Tilt(WindDirection.North);
        Tilt(WindDirection.West);
        Tilt(WindDirection.South);
        Tilt(WindDirection.East);

        CalculateTotalLoad();
        return TotalLoad;
    }

    private async void Tilt(WindDirection direction)
    {
        var tasks = direction switch
        {
            WindDirection.North => Columns.Select(column => column.TiltToStartAsync()),
            WindDirection.South => Columns.Select(column => column.TiltToEndAsync()),
            WindDirection.East => Rows.Select(row => row.TiltToEndAsync()),
            WindDirection.West => Rows.Select(row => row.TiltToStartAsync()),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        await Task.WhenAll(tasks);

        switch (direction)
        {
            case WindDirection.North:
            case WindDirection.South:
                AlignRows();
                break;
            case WindDirection.East:
            case WindDirection.West:
                AlignColumns();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private async void AlignRows()
    {
        await Task.WhenAll(
            AlignColumnsRockTypeAsync(true, row => row.GetIndicesOfRockType(true)),
            AlignColumnsRockTypeAsync(false, row => row.GetIndicesOfRockType(false)),
            AlignColumnsRockTypeAsync(null, row => row.GetIndicesOfRockType(null))
        );
    }

    private async Task AlignColumnsRockTypeAsync(bool? rockType, Func<DishRow, IEnumerable<int>> getIndicesFunc)
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
            AlignRowsRockTypeAsync(true, row => row.GetIndicesOfRockType(true)),
            AlignRowsRockTypeAsync(false, row => row.GetIndicesOfRockType(false)),
            AlignRowsRockTypeAsync(null, row => row.GetIndicesOfRockType(null))
        );
    }

    private async Task AlignRowsRockTypeAsync(bool? rockType, Func<DishRow, IEnumerable<int>> getIndicesFunc)
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