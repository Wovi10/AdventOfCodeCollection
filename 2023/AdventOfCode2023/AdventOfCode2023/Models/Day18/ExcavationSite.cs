using AdventOfCode2023_1.Shared.Types;

namespace AdventOfCode2023_1.Models.Day18;

public class ExcavationSite
{
    public ExcavationSite(List<string> input)
    {
        DigPlan = input.Select(x => new DigInstruction(x)).ToList();
    }

    private List<DigInstruction> DigPlan { get; }

    public long CalculateArea()
    {
        var points = new Point2D[DigPlan.Count];
        var perimeter = 0L;

        perimeter = InitialisePositions(points, perimeter);

        var area = 0L;

        for (var i = 0; i < points.Length; i++)
        {
            var nextIndex = (i + 1) % points.Length;
            var prevIndex = i - 1 < 0 ? points.Length - 1 : i - 1;
            area += points[i].Y * (points[nextIndex].X - points[prevIndex].X);
        }

        area = Math.Abs(area) / 2;
        area += perimeter / 2 + 1;

        return area;
    }

    private long InitialisePositions(Point2D[] points, long perimeter)
    {
        var position = new Point2D(0, 0);
        for (var i = 0; i < DigPlan.Count; i++)
        {
            points[i] = position;
            var instruction = DigPlan[i];
            position = position.Move(instruction);
            perimeter += instruction.Distance;
        }

        return perimeter;
    }
}