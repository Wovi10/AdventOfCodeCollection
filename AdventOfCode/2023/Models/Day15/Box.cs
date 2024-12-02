namespace _2023.Models.Day15;

public class Box
{
    private readonly List<Lens> _lenses = new();

    public void DoOperation(Lens lens)
    {
        if (lens.Operation)
        {
            TryAdd(lens);
            return;
        }

        TryRemove(lens);
    }

    private void TryAdd(Lens newLens)
    {
        if (_lenses.Any(lens => lens.Label == newLens.Label))
        {
            _lenses.First(lens => lens.Label == newLens.Label).FocalLength = newLens.FocalLength;
            return;
        }

        _lenses.Add(newLens);
    }

    private void TryRemove(Lens lens)
    {
        if (_lenses.Any(l => l.Label == lens.Label))
            _lenses.Remove(_lenses.First(l => l.Label == lens.Label));
    }

    public int GetFocusingPower(int boxNumber)
        => _lenses.Select((t, i) => t.GetFocusingPower(boxNumber, i + 1)).Sum();
}