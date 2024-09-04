using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day24;

public class Hail
{
    public Hail(string inputLine)
    {
        var parts = inputLine.Split(" @ ");
        var positionParts = parts[0].Split(", ");
        var velocityParts = parts[1].Split(", ");

        Coordinates = new(double.Parse(positionParts[0]), double.Parse(positionParts[1]), double.Parse(positionParts[2]));
        Velocity = new(double.Parse(velocityParts[0]), double.Parse(velocityParts[1]), double.Parse(velocityParts[2]));
    }

    public Hail(Vector3 coordinates, Vector3 velocity)
    {
        Coordinates = coordinates;
        Velocity = velocity;
    }

    public Vector3 Coordinates { get; set; }
    public Vector3 Velocity { get; set; }

    public double Slope => Velocity.Y / Velocity.X;
    public double YAxisIntercept => Coordinates.Y - Slope * Coordinates.X;

    private bool XIsGoingDown => Velocity.X < 0;
    private bool YIsGoingDown => Velocity.Y < 0;

    public bool WillIntersectZone => CheckZoneIntersection();

    private bool CheckZoneIntersection()
    {
        var xWillIntersect =
            Coordinates.X.IsBetween(Boundaries.LowerX, Boundaries.UpperX) ||
            (Coordinates.X < Boundaries.LowerX && !XIsGoingDown) ||
            (Coordinates.X > Boundaries.UpperX && XIsGoingDown);

        var yWillIntersect =
            Coordinates.Y.IsBetween(Boundaries.LowerY, Boundaries.UpperY) ||
            (Coordinates.Y < Boundaries.LowerY && !YIsGoingDown) ||
            (Coordinates.Y > Boundaries.UpperY && YIsGoingDown);

        return xWillIntersect && yWillIntersect;
    }

    public (double, double)? GetIntersectionPoint(Hail otherHail)
    {
        if (Slope == otherHail.Slope) // Parallel lines or same line
            return null;

        var originalX = (otherHail.YAxisIntercept - YAxisIntercept) / (Slope - otherHail.Slope);
        var originalY = Slope * originalX + YAxisIntercept;

        var roundedX = Math.Round(originalX, 3);
        var roundedY = Math.Round(originalY, 3);

        // Check if the intersection point is in the future for both hails
        if (!WillCrossInTheFuture(roundedX, roundedY) || !otherHail.WillCrossInTheFuture(roundedX, roundedY))
            return null;

        return (roundedX, roundedY);
    }

    private bool WillCrossInTheFuture(double x, double y)
        => (XIsGoingDown && x < Coordinates.X) ||
           (!XIsGoingDown && x > Coordinates.X) ||
           (YIsGoingDown && y < Coordinates.Y) ||
           (!YIsGoingDown && y > Coordinates.Y);

    public Vector3 GetPositionAtTime(long time)
        => (Coordinates.X + (long)(Velocity.X * time), Coordinates.Y + (long)(Velocity.Y * time), Coordinates.Z + (long)(Velocity.Z * time));
}