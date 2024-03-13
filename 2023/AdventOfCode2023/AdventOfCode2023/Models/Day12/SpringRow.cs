using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day12;

public class SpringRow
{
    public SpringRow(string input)
    {
        var splitInput = input.Split(Constants.Space);

        var springsFromInput = splitInput.First();
        SetSprings(springsFromInput);
        SetDamagedSpringsIndices();

        var arrangementsFromInput = splitInput.Last();
        SetContinuousArrangements(arrangementsFromInput);
        _continuousDamagedWithSpaces = _continuousDamagedSprings.Count - 1 + _continuousDamagedSprings.Sum();
    }

    private readonly List<SpringType> _springs = new();
    private readonly List<int> _damagedSpringsIndices = new();
    private readonly List<int> _continuousDamagedSprings = new();
    private long _possibleArrangements;
    private readonly List<List<int>> _possibleArrangementsPerLength = new();
    private readonly int _continuousDamagedWithSpaces;

    private void SetSprings(string springsFromInput)
    {
        if (Variables.RunningPartOne)
        {
            foreach (var springChar in springsFromInput) 
                _springs.Add(springChar.ToSpringState());

            return;
        }

        for (var i = 0; i < 5; i++)
        {
            _springs.AddRange(springsFromInput.Select(springChar => springChar.ToSpringState()));
            _springs.Add(SpringType.Unknown);
        }

        _springs.RemoveAt(_springs.Count - 1);
    }

    private void SetDamagedSpringsIndices()
    {
        for (var i = 0; i < _springs.Count; i++)
        {
            if (_springs[i].IsDamaged()) 
                _damagedSpringsIndices.Add(i);
        }
    }

    private void SetContinuousArrangements(string arrangementsFromInput)
    {
        if (Variables.RunningPartOne)
        {
            _continuousDamagedSprings.AddRange(arrangementsFromInput.Split(Constants.Comma).Select(int.Parse));
            return;
        }

        for (var i = 0; i < 5; i++)
            _continuousDamagedSprings.AddRange(arrangementsFromInput.Split(Constants.Comma).Select(int.Parse));
    }

    public long GetPossibleArrangements()
    {
        if (_continuousDamagedWithSpaces == _springs.Count)
        {
            _possibleArrangements = 1;
            return _possibleArrangements;
        }

        GetPossibleIndicesForLength();
        CountCombinationsHelper(_possibleArrangementsPerLength, 0, new List<int>());
        return _possibleArrangements;
    }

    private void GetPossibleIndicesForLength()
    {
        var continuousCounter = 0;
        var usedContinuousDamagedWithSpaces = 0;
        foreach (var lengthToCheck in _continuousDamagedSprings)
        {
            if (continuousCounter == _continuousDamagedSprings.Count)
                usedContinuousDamagedWithSpaces--;

            var minLengthNeeded = _continuousDamagedWithSpaces - usedContinuousDamagedWithSpaces;

            var possibility = new List<int>();

            for (var i = usedContinuousDamagedWithSpaces; i < _springs.Count; i++)
            {
                if (_springs.Count < i + minLengthNeeded)
                    break;

                var previousSpring = _springs.ElementAtOrDefault(i - 1);
                var currentSpring = _springs[i];
                var nextSpring = _springs.ElementAtOrDefault(i + 1);
                var lastInLength = _springs.ElementAtOrDefault(i + minLengthNeeded - 1);
                var followingSprings = _springs[i..(lengthToCheck + i)];
                var firstAfterLength = _springs.ElementAtOrDefault(i + lengthToCheck);

                if (currentSpring.IsPossible(previousSpring, nextSpring, lastInLength, followingSprings, firstAfterLength))
                    possibility.Add(i);
            }

            _possibleArrangementsPerLength.Add(possibility);
            continuousCounter++;
            usedContinuousDamagedWithSpaces += lengthToCheck + 1;
        }
    }

    private void CountCombinationsHelper(List<List<int>> lists, int index, List<int> currentCombination)
    {
        if (index == lists.Count)
        {
            if (CombinationIsPossible(currentCombination))
                _possibleArrangements++;

            return;
        }

        foreach (var number in lists[index])
        {
            if (currentCombination.Count == 0 && _springs.Count - index < _continuousDamagedWithSpaces)
                break;

            if (currentCombination.Count != 0 && (number <= currentCombination[^1] + 1 ||
                                                  number <= currentCombination[^1] +
                                                  _continuousDamagedSprings[index - 1]))
                continue;

            var currentDamagedSpring = _continuousDamagedSprings[index];

            var nextSpringIsDamaged = number + currentDamagedSpring < _springs.Count &&
                                      _springs[number + currentDamagedSpring].IsDamaged();
            if (nextSpringIsDamaged)
                continue;

            currentCombination.Add(number);
            CountCombinationsHelper(lists, index + 1, currentCombination);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }

    private bool CombinationIsPossible(List<int> combination)
    {
        if (!ContainsAllContinuousDamagedSprings(combination))
            return false;

        for (var i = 0; i < combination.Count - 1; i++)
        {
            if (combination[i] + _continuousDamagedSprings[i] >= combination[i + 1]) 
                return false;
        }

        return true;
    }

    private bool ContainsAllContinuousDamagedSprings(List<int> combination)
        => _damagedSpringsIndices.All(damagedSpringIndex => DamagedSpringIndexIsUsed(combination, damagedSpringIndex));

    private bool DamagedSpringIndexIsUsed(List<int> combination, int damagedSpringIndex)
    {
        if (combination.Contains(damagedSpringIndex))
            return true;

        for (var i = 0; i < combination.Count; i++)
        {
            var requiredLength = _continuousDamagedSprings[i];
            for (var j = 0; j < requiredLength; j++)
            {
                if (combination[i] + j == damagedSpringIndex)
                    return true;
            }
        }

        return false;
    }

    public Task<long> GetPossibleArrangementsAsync()
    {
        if (_continuousDamagedWithSpaces == _springs.Count)
        {
            _possibleArrangements = 1;
            return Task.FromResult(_possibleArrangements);
        }

        GetPossibleIndicesForLength();
        CountCombinationsHelper(_possibleArrangementsPerLength, 0, new List<int>());
        return Task.FromResult(_possibleArrangements);
    }
}