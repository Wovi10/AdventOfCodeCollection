namespace _2024.Models.Day20;

public static class Day20Extensions
{
    public static Race ToRace(this IEnumerable<string>input)
        => new(input.ToArray());

    public static Race FillShortestPathNoCheating(this Race race)
    {
        race.SetFastestRoute();
        return race;
    }

    public static long FindWithCheats(this Race race)
    {
        var fastestRoute = race.FastestRoute;
        var allWalls = race.GetAllWalls().ToArray();

        var counter = 0L;
        foreach (var racePosition in allWalls)
        {
            var newRace = new Race(race, racePosition);
            newRace.SetFastestRoute();
            if (newRace.FastestRoute != -1 && fastestRoute - 100 >= newRace.FastestRoute)
                counter++;
        }

        return counter;
    }
}