namespace AdventOfCode2023_1.Models.Day15;

public class Lens
{
    public Lens(string lensString)
    {
        var operationIndex = lensString.IndexOfAny(new[] { '=', '-' });
        Label = lensString[..operationIndex];
        Operation = lensString[operationIndex].ToOperation();
        FocalLength = int.TryParse(lensString[(operationIndex+1)..], out var focalLength) ? focalLength : null;
    }

    public int? FocalLength { get; set; }
    public string Label { get; set; }
    public Operation Operation { get; set; }
    public int Hash { get; set; }

    public async void SetHash()
    {
        Hash = await Label.Hash();
    }

    public int GetFocusingPower(int boxNumber, int numberInBox) 
        => boxNumber * numberInBox * (FocalLength ?? 0);
}