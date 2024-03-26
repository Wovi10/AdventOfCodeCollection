namespace AdventOfCode2023_1.Models.Day14;

public class DishRow
{
    public List<RockType> Rocks { get; set; } = new();

    public DishRow(string line)
    {
        foreach (var rockChar in line)
        {
            Rocks.Add(rockChar.ToRockType());
        }
    }

    public List<int> GetRoundRocksIndices()
        => Rocks.Select((r, i) => r == RockType.Round ? i : -1).Where(i => i != -1).ToList();

    public List<int> GetSquareRocksIndices()
        => Rocks.Select((r, i) => r == RockType.Square ? i : -1).Where(i => i != -1).ToList();

    public List<int> GetNoneRocksIndices()
        => Rocks.Select((r, i) => r == RockType.None ? i : -1).Where(i => i != -1).ToList();
    
    public List<int> GetBlockedIndices() 
        => Rocks.Select((r, i) => r != RockType.None ? i : -1).Where(i => i != -1).ToList();
}