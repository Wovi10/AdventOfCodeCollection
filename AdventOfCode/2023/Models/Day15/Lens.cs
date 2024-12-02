namespace _2023.Models.Day15;

public class Lens
{
    private static readonly System.Buffers.SearchValues<char> SMyChars = System.Buffers.SearchValues.Create("=-");

    public Lens(string lensString)
    {
        var operationIndex = lensString.AsSpan().IndexOfAny(SMyChars);
        Label = lensString[..operationIndex];
        Operation = lensString[operationIndex].ToOperation();
        FocalLength = int.TryParse(lensString[(operationIndex + 1)..], out var focalLength) ? focalLength : null;
    }

    public int? FocalLength { get; set; }
    public string Label { get; }
    public bool Operation { get; }
    public int Hash { get; private set; }

    public async void SetHash()
    {
        Hash = await Label.Hash();
    }

    public int GetFocusingPower(int boxNumber, int numberInBox)
        => boxNumber * numberInBox * (FocalLength ?? 0);
}