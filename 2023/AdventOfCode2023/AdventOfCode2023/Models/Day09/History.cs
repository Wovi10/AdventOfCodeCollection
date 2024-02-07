using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day09;

public class History(List<long> sequence)
{
    private readonly List<long> _sequence = sequence;
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
        _nextStep?.Extrapolate();

        if (Variables.RunningPartOne)
        {
            if (_nextStep == null)
            {
                _sequence.Add(0);
                AddedValue = _sequence.Last();
                return;
            }

            _sequence.Add(_nextStep._sequence.Last() + _sequence.Last());
            AddedValue = _sequence.Last();
            return;
        }

        RightShiftSequence();
        if (_nextStep == null)
        {
            _sequence[0] = 0;
            AddedValue = _sequence.First();
            return;
        }

        _sequence[0] = _sequence[1] - _nextStep._sequence.First();
        AddedValue = _sequence.First();
    }

    private void RightShiftSequence()
    {
        _sequence.Add(_sequence.Last());
        for (var i = _sequence.Count - 1; i > 0; i--)
        {
            _sequence[i] = _sequence[i - 1];
        }

        if (_nextStep != null)
        {
            _nextStep.RightShiftSequence();
        }
    }
}