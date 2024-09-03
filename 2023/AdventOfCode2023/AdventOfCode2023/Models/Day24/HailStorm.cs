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

    public Hail GetIntersection()
    {
        // In this case only 3 hails are given. We need to find a line that intersects all 3 hails.
        // Find three planes
        // First plane is between the first two hails

        var (vectorCrossProductFirstSecond, vectorDotProductFirstSecond) = FindPlane(Hails[0], Hails[1]);
        var (vectorCrossProductSecondThird, vectorDotProductSecondThird) = FindPlane(Hails[1], Hails[2]);
        var (vectorCrossProductFirstThird, vectorDotProductFirstThird) = FindPlane(Hails[0], Hails[2]);

        var w = Line(vectorDotProductFirstSecond, VectorCrossProduct(vectorCrossProductSecondThird, vectorCrossProductFirstThird), vectorDotProductSecondThird, VectorCrossProduct(vectorCrossProductFirstSecond, vectorCrossProductFirstThird), vectorDotProductFirstThird, VectorCrossProduct(vectorCrossProductFirstSecond, vectorCrossProductSecondThird));
        
    }

    private object Line(long similarityFirstSecond, (long, long, long) cross, long l1, (long, long, long) valueTuple, long l2,
        (long, long, long) cross1)
    {
        throw new NotImplementedException();
    }

    private ((long, long, long) vectorCrossProduct, long vectorDotProduct) FindPlane(Hail first, Hail second)
    {
        var positionFirst = (first.X, first.Y, first.Z);
        var positionSecond = (second.X, second.Y, second.Z);
        var positionFirstSecond = Sub(positionFirst, positionSecond);
        var velocityFirstSecond = Sub(positionFirst, positionSecond);
        var vv = VectorCrossProduct(positionFirst, positionSecond);

        return (VectorCrossProduct(positionFirstSecond, velocityFirstSecond), VectorDotProduct(positionFirstSecond, vv));
    }

    // This will give a number showing how similar two vectors are. The closer to 1 the more similar they are. -1 is opposite.
    private long VectorDotProduct((long, long, long) positionFirstSecond, (long, long, long) vv)
    {
        return positionFirstSecond.Item1 * vv.Item1 + positionFirstSecond.Item2 * vv.Item2 +
               positionFirstSecond.Item3 * vv.Item3;
    }

    private (long, long, long) Sub((long, long, long) first, (long, long, long) second)
    {
        return (first.Item1 - second.Item1, first.Item2 - second.Item2, first.Item3 - second.Item3);
    }

    private (long, long, long) VectorCrossProduct((long, long, long) first, (long, long, long) second)
    {
        return (first.Item2 * second.Item3 - first.Item3 * second.Item2,
            first.Item3 * second.Item1 - first.Item1 * second.Item3,
            first.Item1 * second.Item2 - first.Item2 * second.Item1);
    }
}