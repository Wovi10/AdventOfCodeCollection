namespace AdventOfCode2023_1.Models.Day14;

public class Dish
{
    public Dish(List<string> input)
    {
        Rows = input.Select(l => l.Trim()).Select(line => new DishRow(line)).ToList();
    }

    private List<DishRow> Rows { get; set; }
    public long TotalLoad { get; set; }

    public void TiltNorth()
    {
        var hasChanged = false;

        do
        {
            hasChanged = false;
            var counter = 1;
            foreach (var dishRow in Rows.Skip(1))
            {
                var previousRow = Rows[counter - 1];
                var blockedIndices = previousRow.GetBlockedIndices();
                var roundRocks = dishRow.GetRoundRocksIndices().Where(index => !blockedIndices.Contains(index)).ToList();
                var emptySpaces = previousRow.GetNoneRocksIndices();
            
                foreach (var roundRockIndex in roundRocks.Where(roundRockIndex => emptySpaces.Contains(roundRockIndex)))
                {
                    previousRow.Rocks[roundRockIndex] = RockType.Round;
                    dishRow.Rocks[roundRockIndex] = RockType.None;
                    hasChanged = true;
                }

                counter++;
            }
        } while (hasChanged);
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
}