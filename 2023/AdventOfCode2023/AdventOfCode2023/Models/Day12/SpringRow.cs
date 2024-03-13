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

    private readonly List<SpringType> _springs = [];
    private readonly List<int> _damagedSpringsIndices = [];
    private readonly List<int> _continuousDamagedSprings = [];
    private long _possibleArrangements = 0;
    private readonly List<List<int>> _possibleArrangementsPerLength = [];
    private readonly int _continuousDamagedWithSpaces = 0;

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
        CountCombinationsHelper(_possibleArrangementsPerLength, 0, []);
        return _possibleArrangements;
    }

    private void GetPossibleIndicesForLength()
    {
        var continuousCounter = 0;
        foreach (var lengthToCheck in _continuousDamagedSprings)
        {
            var usedContinuousDamagedSprings = _continuousDamagedSprings.Take(continuousCounter).ToList();
            var usedContinuousDamagedWithSpaces = usedContinuousDamagedSprings.Count != 0 
                            ? usedContinuousDamagedSprings.Sum() + usedContinuousDamagedSprings.Count
                            : 0;
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

            var nextSpringIsDamaged = number + _continuousDamagedSprings[index] < _springs.Count &&
                                      _springs[number + _continuousDamagedSprings[index]].IsDamaged();
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

        for (var j = 0; j < combination.Count - 1; j++)
        {
            var currentIndex = combination[j];
            var nextIndex = combination[j + 1];

            if (!ContainsOverlappingDamagedSprings(currentIndex, nextIndex, _continuousDamagedSprings[j]))
                continue;

            return false;
        }

        return true;
    }

    private bool ContainsAllContinuousDamagedSprings(List<int> combination)
        => _damagedSpringsIndices.All(damagedSpringIndex => DamagedSpringIndexIsUsed(combination, damagedSpringIndex));

    private bool DamagedSpringIndexIsUsed(List<int> combination, int damagedSpringIndex)
    {
        for (var i = 0; i < combination.Count; i++)
        {
            var currentIndex = combination[i];
            var currentRequiredLength = _continuousDamagedSprings[i];
            for (var checkingIndex = currentIndex;
                 checkingIndex < currentIndex + currentRequiredLength;
                 checkingIndex++)
            {
                if (damagedSpringIndex == checkingIndex)
                    return true;
            }
        }

        return false;
    }

    private bool ContainsOverlappingDamagedSprings(int currentIndex, int nextIndex, int requiredLength)
    {
        var indexAfterLength = currentIndex + requiredLength;

        var hasSpace = currentIndex + 1 >= nextIndex;
        var previousSpringIsDamaged = currentIndex > 0 && _springs[currentIndex - 1].IsDamaged();
        var nextSpringIsDamaged = indexAfterLength < _springs.Count && _springs[indexAfterLength].IsDamaged();
        var nextIndexOverlaps = currentIndex + requiredLength >= nextIndex;

        return hasSpace || previousSpringIsDamaged || nextSpringIsDamaged || nextIndexOverlaps ||
               currentIndex + requiredLength >= nextIndex;
    }

    public Task<long> GetPossibleArrangementsAsync()
    {
        if (_continuousDamagedWithSpaces == _springs.Count)
        {
            _possibleArrangements = 1;
            return Task.FromResult(_possibleArrangements);
        }

        GetPossibleIndicesForLength();
        CountCombinationsHelper(_possibleArrangementsPerLength, 0, []);
        return Task.FromResult(_possibleArrangements);
    }
}