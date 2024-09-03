using UtilsCSharp;

namespace AdventOfCode2023_1.Models.Day24;

public class Hail
{
    public Hail(string inputLine)
    {
        var parts = inputLine.Split(" @ ");
        var positionParts = parts[0].Split(", ");
        var velocityParts = parts[1].Split(", ");

        X = long.Parse(positionParts[0]);
        Y = long.Parse(positionParts[1]);
        Z = long.Parse(positionParts[2]);
        VelocityX = long.Parse(velocityParts[0]);
        VelocityY = long.Parse(velocityParts[1]);
        VelocityZ = long.Parse(velocityParts[2]);
    }

    public long X { get; set; }
    public long Y { get; set; }
    public long Z { get; set; }
    public double VelocityX { get; set; }
    public double VelocityY { get; set; }
    public double VelocityZ { get; set; }

    public double Slope => VelocityY / VelocityX;
    public double YAxisIntercept => Y - Slope * X;

    private bool XIsGoingDown => VelocityX < 0;
    private bool YIsGoingDown => VelocityY < 0;

    public bool WillIntersectZone => CheckZoneIntersection();

    private bool CheckZoneIntersection()
    {
        var xWillIntersect =
            X.IsBetween(Boundaries.LowerX, Boundaries.UpperX) ||
            (X < Boundaries.LowerX && !XIsGoingDown) ||
            (X > Boundaries.UpperX && XIsGoingDown);

        var yWillIntersect =
            Y.IsBetween(Boundaries.LowerY, Boundaries.UpperY) ||
            (Y < Boundaries.LowerY && !YIsGoingDown) ||
            (Y > Boundaries.UpperY && YIsGoingDown);

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
        => (XIsGoingDown && x < X) ||
           (!XIsGoingDown && x > X) ||
           (YIsGoingDown && y < Y) ||
           (!YIsGoingDown && y > Y);

    public (long, long, long) GetPositionAtTime(long time)
        => (X + (long)(VelocityX * time), Y + (long)(VelocityY * time), Z + (long)(VelocityZ * time));
}