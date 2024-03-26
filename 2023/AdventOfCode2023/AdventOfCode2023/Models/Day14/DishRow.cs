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
        bool hasChanged;
        do
        {
            hasChanged = false;
            var counter = Rocks.Count - 2;
            while (counter >= 0)
            {
                if (Rocks[counter] == RockType.Round && Rocks[counter + 1] == RockType.None)
                {
                    Rocks[counter] = RockType.None;
                    Rocks[counter + 1] = RockType.Round;
                    hasChanged = true;
                }

                counter--;
            }
        } while (hasChanged);
        
        return Task.CompletedTask;
    }

    public Task TiltToStartAsync()
    {
        bool hasChanged;
        do
        {
            hasChanged = false;
            var counter = 1;
            foreach (var rockType in Rocks.Skip(1))
            {
                if (Rocks[counter-1] != RockType.None || rockType != RockType.Round)
                {
                    counter++;
                    continue;
                }

                Rocks[counter - 1] = rockType;
                Rocks[counter] = RockType.None;
                counter++;
                
                hasChanged = true;
            }
        } while (hasChanged);

        return Task.CompletedTask;
    }
}