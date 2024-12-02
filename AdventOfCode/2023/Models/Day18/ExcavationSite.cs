using UtilsCSharp.Objects;

namespace _2023.Models.Day18;

public class ExcavationSite
{
    public ExcavationSite(List<string> input)
    {
        DigPlan = input.Select(x => new DigInstruction<long>(x)).ToList();
    }

    private List<DigInstruction<long>> DigPlan { get; }

    public long CalculateArea()
    {
        List<Node<long>> points = new();
        var perimeter = InitialisePositions(points);

        var area = 0L;

        for (var i = 0; i < points.Count; i++)
        {
            var nextIndex = (i + 1) % points.Count;
            var prevIndex = i - 1 < 0 ? points.Count - 1 : i - 1;

            area += points[i].X * (points[nextIndex].Y - points[prevIndex].Y);
        }

        area = Math.Abs(area) / 2;
        area += perimeter / 2 + 1;

        return area;
    }

    private long InitialisePositions(List<Node<long>> points)
    {
        var perimeter = 0L;
        var node = new Node<long>(0, 0);
        foreach (var instruction in DigPlan)
        {
            points.Add(node);
            node = node.MoveToNode(instruction.Offset.ToDirection(), instruction.Distance);

            perimeter += instruction.Distance;
        }

        return perimeter;
    }
}