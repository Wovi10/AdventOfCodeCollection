namespace AdventOfCode2023_1.Models.Day13;

public class Line
{
    public Line(string lineString)
    {
        foreach (var character in lineString.Trim())
        {
            Rocks.Add(character == '#');
        }
    }

    public List<bool> Rocks = new();
    public int LinesBeforeMirror { get; set; }
    public List<int> MirroredPositions = new();

    public List<int> GetMirroredPositions()
    {
        MirroredPositions.Sort();
        return MirroredPositions;
    }

    public async Task<bool> IsSymmetric()
    {
        for (var i = 1; i < Rocks.Count-1; i++)
        {
            if (CanBeMirrored(i)) 
                MirroredPositions.Add(i);
        }

        return MirroredPositions.Count > 0;
    }

    private bool CanBeMirrored(int position)
    {
        var placesFromEnd = Rocks.Count - position;

        var isBeforeMiddle = position.IsBeforeMiddle(Rocks.Count / 2);

        var rangeToCheck = isBeforeMiddle switch
        {
            true => Rocks[..(position * 2)],
            false => Rocks[(position - placesFromEnd)..],
            null => Rocks
        };

        for (var i = 0; i < rangeToCheck.Count / 2; i++)
        {
            if (rangeToCheck[i] != rangeToCheck[^(i + 1)])
                return false;
        }

        return true;
    }
}