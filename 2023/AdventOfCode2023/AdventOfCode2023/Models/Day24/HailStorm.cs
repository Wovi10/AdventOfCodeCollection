using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day24;

public class HailStorm
{
    public HailStorm(List<string> input)
    {
        Hails = new List<Hail>();
        foreach (var line in input)
        {
            var hail = new Hail(line);
            if (hail.WillIntersectZone) 
                Hails.Add(new(line));
        }
    }

    public List<Hail> Hails { get; set; }
    
    public int GetNumberOfCrossingPaths()
    {
        var intersectingHails = new List<(Hail, Hail)>();
        foreach (var outerHail in Hails)
        {
            foreach (var innerHail in Hails)
            {
                if (Hails.IndexOf(outerHail) == Hails.IndexOf(innerHail))
                    continue;

                var intersectionPoint = outerHail.GetIntersectionPoint(innerHail);

                if (!intersectionPoint.HasValue)
                    continue;

                var (x, y) = intersectionPoint.Value;
                if (!x.IsBetween(Boundaries.LowerX, Boundaries.UpperX) ||
                    !y.IsBetween(Boundaries.LowerY, Boundaries.UpperY)) 
                    continue;

                var smallerHail = outerHail.X < innerHail.X ? outerHail : innerHail;
                var biggerHail = outerHail.X > innerHail.X ? outerHail : innerHail;
                intersectingHails.Add((smallerHail, biggerHail));
            }
        }

        return intersectingHails.Distinct().ToList().Count;
    }
}