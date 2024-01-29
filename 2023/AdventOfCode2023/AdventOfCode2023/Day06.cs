using AdventOfCode2023_1.Models.Day06;
using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day06:DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("06", false);
    private readonly List<Race> _races = new();

    protected override void PartOne()
    {
        var result = GetTotalWaysToWin();
        SharedMethods.AnswerPart(1, result);
    }

    protected override void PartTwo()
    {
        throw new NotImplementedException();
    }

    #region Part 1
    
    private int GetTotalWaysToWin()
    {
        ProcessFile();

        return _races.Aggregate(1, (index, race) => index * GetWaysToWin(race));
    }

    private static int GetWaysToWin(Race race)
    {
        var waysToWin = 0;
        for (var i = 0; i <= race.Duration; i++)
        {
            race.DistanceTravelled = i * (race.Duration - i);
            if (race.DistanceTravelled > race.Record) 
                waysToWin += 1;
        }

        return waysToWin;
    }

    #endregion
    

    private void ProcessFile()
    {
        var times = Input.First().Split(Constants.Space).ToList().Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
        var distances = Input.Last().Split(Constants.Space).ToList().Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();

        for (var i = 0; i < times.Count; i++)
        {
            var race = new Race(times[i], distances[i]);
            _races.Add(race);
        }

        Console.WriteLine(_races);
    }
}