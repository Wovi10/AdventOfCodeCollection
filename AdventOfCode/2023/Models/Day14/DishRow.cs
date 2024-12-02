namespace _2023.Models.Day14;

public class DishRow
{
    public List<bool?> Rocks { get; } = new();

    public DishRow(string line)
    {
        foreach (var rockChar in line)
        {
            Rocks.Add(rockChar.ToRockType());
        }
    }

    public DishRow(List<bool?> rocks)
    {
        Rocks = rocks;
    }

    public List<int> GetIndicesOfRockType(bool? rockType)
        => Rocks.Select((r, i) => r == rockType ? i : -1).Where(i => i != -1).ToList();

    public Task TiltToEndAsync()
    {
        var hasChanged = true;
        while (hasChanged)
        {
            hasChanged = false;
            for (var i = Rocks.Count - 2; i >= 0; i--)
            {
                if (Rocks[i] != true || Rocks[i + 1] != null)
                    continue;

                Rocks[i] = null;
                Rocks[i + 1] = true;
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
                if (Rocks[i - 1] != null || Rocks[i] != true)
                    continue;

                Rocks[i - 1] = true;
                Rocks[i] = null;
                hasChanged = true;
            }
        }

        return Task.CompletedTask;
    }
}