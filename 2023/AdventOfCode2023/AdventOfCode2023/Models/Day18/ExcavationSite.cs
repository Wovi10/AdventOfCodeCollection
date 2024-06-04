using AdventOfCode2023_1.Shared;
using AdventOfCode2023_1.Shared.Types;
using NUnit.Framework;

namespace AdventOfCode2023_1.Models.Day18;

public class ExcavationSite
{
    public ExcavationSite(List<string> input)
    {
        DigPlan = input.Select(x => new DigInstruction(x)).ToList();
        var fakeDigplan = new List<DigInstruction>{new(){Distance = 461937, Offset = (1, 0)},new(){Distance = 56407, Offset = (0, 1)},new(){Distance = 356671, Offset = (1, 0)},new(){Distance = 863240, Offset = (0, 1)},new(){Distance = 367720,Offset = (1, 0)},new(){Distance = 266681,Offset = (0, 1)},new(){Distance = 577262,Offset = (-1, 0)},new(){Distance = 829975,Offset = (0, -1)},new(){Distance = 112010,Offset = (-1, 0)},new(){Distance = 829975,Offset = (0, 1)},new(){Distance = 491645,Offset = (-1, 0)},new(){Distance = 686074,Offset = (0, -1)},new(){Distance = 5411,Offset = (-1, 0)},new(){Distance = 500254,Offset = (0, -1)}};

        if (!Variables.RunningPartOne)
        {
            Assert.AreEqual(DigPlan, fakeDigplan);
        }
    }

    private List<DigInstruction> DigPlan { get; }

    public long CalculateArea()
    {
        List<Point2D> points = new();
        var perimeter = 0L;

        perimeter = InitialisePositions(points, perimeter);

        var area = 0L;

        for (var i = 0; i < points.Count; i++)
        {
            var nextIndex = (i + 1) % points.Count;
            var prevIndex = i - 1 < 0 ? points.Count - 1 : i - 1;

            var currentX = points[i].X;
            var nextY = points[nextIndex].Y;
            var prevY = points[prevIndex].Y;

            area += currentX * (nextY - prevY);
        }

        area = Math.Abs(area) / 2;
        area += perimeter / 2 + 1;

        return area;
    }

    private long InitialisePositions(List<Point2D> points, long perimeter)
    {
        var position = new Point2D(0, 0);
        foreach (var instruction in DigPlan)
        {
            points.Add(position);
            position = position.Move(instruction);
            perimeter += instruction.Distance;
        }

        return perimeter;
    }
}