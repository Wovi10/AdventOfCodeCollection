using AdventOfCode2023_1.Shared;
using AdventOfCode2023_1.Shared.Types;
using NUnit.Framework;

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
        var digplanArray = DigPlan.Select(x => (x.Offset, x.Distance)).ToArray();
        // return Area(digplanArray);
        List<Point2D> points = new();

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

    private long InitialisePositions(List<Point2D> points)
    {
        var perimeter = 0L;
        var position = new Point2D(0, 0);
        foreach (var instruction in DigPlan)
        {
            points.Add(position);
            position = position.Move(instruction);
            perimeter += instruction.Distance;
        }

        return perimeter;
    }

    static long Area(((int, int) dir, int len)[] instructions)
    {
        var pos = (x: 0L, y: 0L);
        var points = new (long x, long y)[instructions.Length];
        var perimeter = 0L;

        for (var i = 0; i < instructions.Length; i++)
        {
            points[i] = pos;
            var instruction = instructions[i];
            var (_, len) = instruction;
            pos = PosFromInstruction(pos, instruction);
            perimeter += len;
        }

        // shoelace formula
        var area = 0L;
            
        for (var i = 0; i < points.Length; i++)
        {
            var nextI = (i + 1) % points.Length;
            var prevI = i - 1 < 0 ? points.Length - 1 : i - 1;
            area += points[i].x * (points[nextI].y - points[prevI].y);
        }

        area = Math.Abs(area) / 2;
        // adjust for the perimeter being the outer edge of the area
        area += perimeter / 2 + 1;

        return area;

        static (long x, long y) PosFromInstruction((long x, long y) pos, ((int, int) dir, int len) instruction)
        {
            var (dir, len) = instruction;
            var (dx, dy) = dir switch
            {
                (0, -1) => (-len, 0),
                (0, 1) => (+len, 0),
                (-1, 0) => (0, -len),
                (1, 0) => (0, +len)
            };

            return (pos.x + dx, pos.y + dy);
        }
    }
}