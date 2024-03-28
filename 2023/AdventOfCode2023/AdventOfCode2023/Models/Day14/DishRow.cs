namespace AdventOfCode2023_1.Models.Day14;

public class DishRow
{
    public List<RockType> Rocks { get; } = new();

    public DishRow(string line)
    {
        foreach (var rockChar in line)
        {
            Rocks.Add(rockChar.ToRockType());
        }
    }

    public DishRow(List<RockType> rocks)
    {
        Rocks = rocks;
    }

    public List<int> GetRoundRocksIndices()
        => Rocks.Select((r, i) => r == RockType.Round ? i : -1).Where(i => i != -1).ToList();

    public Task TiltToEndAsync()
    {
        var hasChanged = true;
        while (hasChanged)
        {
            hasChanged = false;
            for (var i = Rocks.Count - 2; i >= 0; i--)
            {
                if (Rocks[i] != RockType.Round || Rocks[i + 1] != RockType.None) 
                    continue;

                Rocks[i] = RockType.None;
                Rocks[i + 1] = RockType.Round;
                hasChanged = true;
            }
        }

        return Task.CompletedTask;
    }

    public Task TiltToStartAsync()
    {
        var hasChanged = true;
        while (hasChanged)
        {
            hasChanged = false;
            for (var i = 1; i < Rocks.Count; i++)
            {
                if (Rocks[i - 1] != RockType.None || Rocks[i] != RockType.Round)
                    continue;

                Rocks[i - 1] = RockType.Round;
                Rocks[i] = RockType.None;
                hasChanged = true;
            }
        }

        return Task.CompletedTask;
    }
}