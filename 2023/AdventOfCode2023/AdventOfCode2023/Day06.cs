using AdventOfCode2023_1.Models.Day06;
using AdventOfCode2023_1.Shared;
using Constants = UtilsCSharp.Utils.Constants;

namespace AdventOfCode2023_1;

public class Day06 : DayBase
{
    private readonly List<Race> _races = new();

    protected override Task<object> PartOne()
    {
        var result = GetTotalWaysToWin();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetTotalWaysToWin();

        return Task.FromResult<object>(result);
    }

    private int GetTotalWaysToWin()
    {
        ProcessFile();
        return _races.Aggregate(1, (index, race) => index * GetWaysToWin(race));
    }

    private static int GetWaysToWin(Race race)
    {
        var duration = Variables.RunningPartOne ? race.DurationInt : race.DurationLong;
        var record = Variables.RunningPartOne ? race.RecordInt : race.RecordLong;
        var waysToWin = 0;
        for (var i = 0; i <= duration; i++)
        {
            var distance = i * (duration - i);
            if (distance > record)
                waysToWin += 1;
        }

        return waysToWin;
    }

    private void ProcessFile()
    {
        _races.Clear();
        var times = Input
            .First()
            .Split(Constants.Space)
            .ToList()
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();
        var distances = Input
            .Last()
            .Split(Constants.Space)
            .ToList()
            .Where(x => int.TryParse(x, out _))
            .Select(int.Parse)
            .ToList();
        if (Variables.RunningPartOne)
        {
            for (var i = 0; i < times.Count; i++)
            {
                var race = new Race(times[i], distances[i]);
                _races.Add(race);
            }
        }
        else
        {
            var time = long.Parse(string.Join(Constants.EmptyString, times));
            var distance = long.Parse(string.Join(Constants.EmptyString, distances));
            var race = new Race(time, distance);
            _races.Add(race);
        }
    }
}