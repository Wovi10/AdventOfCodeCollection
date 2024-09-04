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

                var smallerHail = outerHail.Coordinates.X < innerHail.Coordinates.X ? outerHail : innerHail;
                var biggerHail = outerHail.Coordinates.X > innerHail.Coordinates.X ? outerHail : innerHail;
                intersectingHails.Add((smallerHail, biggerHail));
            }
        }

        return intersectingHails.Distinct().ToList().Count;
    }

    public double GetIntersection()
    {
        var firstCoordinates = Hails[0].Coordinates;
        var firstVelocities = Hails[0].Velocity;
        var secondCoordinates = Hails[1].Coordinates;
        var secondVelocities = Hails[1].Velocity;
        var thirdCoordinates = Hails[2].Coordinates;
        var thirdVelocities = Hails[2].Velocity;

        var (rock, s) = FindRock(firstCoordinates, firstVelocities, secondCoordinates, secondVelocities,
            thirdCoordinates, thirdVelocities);
        return rock.Sum / s;
    }

    private static (Vector3, double) FindRock(Vector3 position1, Vector3 velocity1, Vector3 position2, Vector3 velocity2, Vector3
        position3, Vector3 velocity3)
    {
        var (crossProductFirstSecond, vectorDotProductFirstSecond) =
            FindPlane(position1, velocity1, position2, velocity2);
        var (crossProductFirstThird, vectorDotProductFirstThird) =
            FindPlane(position1, velocity1, position3, velocity3);
        var (crossProductSecondThird, vectorDotProductSecondThird) =
            FindPlane(position2, velocity2, position3, velocity3);

        var linearCombination =
            LinearCombination(
                vectorDotProductFirstSecond,
                VectorCrossProduct(crossProductFirstThird, crossProductSecondThird),
                vectorDotProductFirstThird,
                VectorCrossProduct(crossProductSecondThird, crossProductFirstSecond),
                vectorDotProductSecondThird,
                VectorCrossProduct(crossProductFirstSecond, crossProductFirstThird));
        var dotProcuctCrossProducts = 
            VectorDotProduct(
                crossProductFirstSecond,
                VectorCrossProduct(crossProductFirstThird, crossProductSecondThird));

        linearCombination = (Math.Round(linearCombination.X / dotProcuctCrossProducts),
            Math.Round(linearCombination.Y / dotProcuctCrossProducts),
            Math.Round(linearCombination.Z / dotProcuctCrossProducts));

        Console.WriteLine(linearCombination);

        var intersection23 = SubtractVectors(velocity1, linearCombination);
        var intersection13 = SubtractVectors(velocity2, linearCombination);
        var crossProductIntersections = VectorCrossProduct(intersection23, intersection13);
        var E = VectorDotProduct(crossProductIntersections, VectorCrossProduct(position2, intersection13));
        var F = VectorDotProduct(crossProductIntersections, VectorCrossProduct(position1, intersection23));
        var G = VectorDotProduct(position1, crossProductIntersections);
        var S = VectorDotProduct(crossProductIntersections, crossProductIntersections);
        var rock = LinearCombination(E, intersection23, -F, intersection13, G, crossProductIntersections);

        return (rock, S);
    }

    private static Vector3 LinearCombination(double dotProduct12, Vector3 crossProduct12,
        double dotProduct23, Vector3 crossProduct23,
        double dotProduct13, Vector3 crossProduct13)
    {
        var x = dotProduct12 * crossProduct12.X + dotProduct23 * crossProduct23.X + dotProduct13 * crossProduct13.X;
        var y = dotProduct12 * crossProduct12.Y + dotProduct23 * crossProduct23.Y + dotProduct13 * crossProduct13.Y;
        var z = dotProduct12 * crossProduct12.Z + dotProduct23 * crossProduct23.Z + dotProduct13 * crossProduct13.Z;

        return (x, y, z);
    }

    /// <summary>
    /// Finds the plane between two lines defined by (x,y,z) coordinates and their velocities
    /// </summary>
    private static (Vector3, double) FindPlane(Vector3 position1, Vector3
        velocity1, Vector3 position2, Vector3 velocity2)
    {
        var positionFirstSecond = SubtractVectors(position1, position2);
        var velocityFirstSecond = SubtractVectors(velocity1, velocity2);
        var crossProductVelocities = VectorCrossProduct(velocity1, velocity2);

        return (VectorCrossProduct(positionFirstSecond, velocityFirstSecond),
            VectorDotProduct(positionFirstSecond, crossProductVelocities));
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// The dot product is the perpendicular projection of one vector onto the other.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    private static double VectorDotProduct(Vector3 first, Vector3 second)
        => first.X * second.X + first.Y * second.Y + first.Z * second.Z;

    private static Vector3 SubtractVectors(Vector3 first, Vector3 second) 
        => (first.X - second.X, first.Y - second.Y, first.Z - second.Z);

    private static Vector3 VectorCrossProduct(Vector3 first, Vector3 second)
    {
        return (first.Y * second.Z - first.Z * second.Y,
            first.Z * second.X - first.X * second.Z,
            first.X * second.Y - first.Y * second.X);
    }
}