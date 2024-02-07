namespace AdventOfCode2023_1.Models.Day09;

public class History
{
    public History(List<long> sequence)
    {
        _sequence = sequence;
    }

    private readonly List<long> _sequence;
    private History? _nextStep;
    public long AddedValue;

    public void CalculateNextSequence()
    {
        var nextStepSequence = new List<long>();
        for (var i = 0; i < _sequence.Count - 1; i++)
        {
            nextStepSequence.Add(_sequence[i + 1] - _sequence[i]);
        }
        _nextStep = new History(nextStepSequence);
        if (_nextStep._sequence.Any(x => x != 0))
        {
            _nextStep.CalculateNextSequence();
        }
    }

    public void Extrapolate()
    {
        if (_nextStep != null) 
            _nextStep.Extrapolate();
        else
        {
            _sequence.Add(0);
            AddedValue = _sequence.Last();
            return;
        }

        _sequence.Add(_nextStep._sequence.Last() + _sequence.Last());
        AddedValue = _sequence.Last();
    }
}