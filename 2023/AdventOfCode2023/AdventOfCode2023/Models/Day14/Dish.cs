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

    public async Task RunCycles(int numCycles)
    {
        for (var i = 0; i < numCycles; i++) 
            await Cycle();
    }

    private async Task Cycle()
    {
        await TiltNorth();
        await TiltWest();
        await TiltSouth();
        await TiltEast();
    }

    public async Task TiltNorth()
    {
        await Tilt(Direction.North);
    }

    public void CalculateTotalLoad()
    {
        var rowCounter = Rows.Count;
        foreach (var numRoundRocks in Rows.Select(dishRow => dishRow.GetRoundRocksIndices().Count))
        {
            TotalLoad += numRoundRocks * rowCounter;
            rowCounter--;
        }
    }

    private async Task TiltSouth()
    {
        await Tilt(Direction.South);
    }

    private async Task TiltWest()
    {
        await Tilt(Direction.West);
    }
    
    private async Task TiltEast()
    {
        await Tilt(Direction.East);
    }

    private async Task Tilt(Direction direction)
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

    private void AlignRows()
    {
        for (var columnCounter = 0; columnCounter < Columns.Count; columnCounter++)
        {
            var column = Columns[columnCounter];
            for (var rowCounter = 0; rowCounter < Rows.Count; rowCounter++)
            {
                Rows[rowCounter].Rocks[columnCounter] = column.Rocks[rowCounter];
            }
        }
    }

    private void AlignColumns()
    {
        for (var rowCounter = 0; rowCounter < Rows.Count; rowCounter++)
        {
            var row = Rows[rowCounter];
            for (var columnCounter = 0; columnCounter < Columns.Count; columnCounter++)
            {
                Columns[columnCounter].Rocks[rowCounter] = row.Rocks[columnCounter];
            }
        }
    }
}