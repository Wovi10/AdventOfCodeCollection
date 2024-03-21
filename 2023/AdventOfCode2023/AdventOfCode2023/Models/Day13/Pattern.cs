namespace AdventOfCode2023_1.Models.Day13;

public class Pattern
{
    public Pattern(List<string> patternString)
    {
        foreach (var lineString in patternString)
        {
            Lines.Add(new Line(lineString));
        }
        NumColumns = Lines[0].Rocks.Count;
    }

    public List<Line> Lines = new();
    public int NumColumns = 0;
    public int LinesBeforeMirror = 0;
    public bool MirrorIsVertical = false;

    public async Task GetPatternNotesAsync()
    {
        var firstLine = Lines.First();
        var firstLineSymmetric = await firstLine.IsSymmetric();
        if (firstLineSymmetric)
        {
            foreach (var line in Lines.Skip(1))
            {
                if (!await line.IsSymmetric())
                {
                    MirrorIsVertical = false;
                    break;
                }
            }
            MirrorIsVertical = true;
            
            // Find the position that is present in all line.MirroredPositions
            
            LinesBeforeMirror = firstLine.LinesBeforeMirror;
            return;
        }
    }
}