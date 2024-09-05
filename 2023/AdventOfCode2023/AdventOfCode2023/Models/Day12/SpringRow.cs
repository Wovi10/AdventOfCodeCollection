using AdventOfCode2023_1.Models.Day12.Enums;
using AdventOfCode2023_1.Shared;
using UtilsCSharp;
using Constants = UtilsCSharp.Utils.Constants;

namespace AdventOfCode2023_1.Models.Day12;

public class SpringRow
{
    public SpringRow(string input)
    {
        var splitInput = input.Split(Constants.Space);

        var springsFromInput = splitInput.First();
        SetSprings(springsFromInput);
        _damagedSpringsIndices = GetDamagedSpringsIndices();

        var arrangementsFromInput = splitInput.Last();
        SetContinuousArrangements(arrangementsFromInput);
        _continuousDamagedWithSpaces = MathUtils.Add(_continuousDamagedSprings, constant: 1);
    }

    private readonly List<SpringType> _springs = new();
    private readonly int[] _damagedSpringsIndices;
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

    private int[] GetDamagedSpringsIndices()
    {
        var damagedSpringsIndices = new List<int>();
        for (var i = 0; i < _springs.Count; i++)
        {
            if (_springs[i].IsDamaged()) 
                damagedSpringsIndices.Add(i);

        }
        return damagedSpringsIndices.ToArray();
    }

    private void SetContinuousArrangements(string arrangementsFromInput)
    {
        if (Variables.RunningPartOne)
        {
            _continuousDamagedSprings
                .AddRange(arrangementsFromInput.Split(Constants.Comma).Select(int.Parse));
            return;
        }

        for (var i = 0; i < 5; i++)
            _continuousDamagedSprings.AddRange(arrangementsFromInput.Split(Constants.Comma).Select(int.Parse));
    }

    public Task<long> GetPossibleArrangementsAsync() 
        => Task.FromResult(GetPossibleArrangements());

    public long GetPossibleArrangements()
    {
        if (_continuousDamagedWithSpaces != _springs.Count)
        {
            GetPossibleIndicesForLength();
            CountCombinationsHelper(_possibleArrangementsPerLength);
            return _possibleArrangements;
        }

        if (!IsPossibleArrangement())
            return _possibleArrangements;

        _possibleArrangements = 1;
        return _possibleArrangements;
    }

    private bool IsPossibleArrangement()
    {
        var currentIndex = 0;
        foreach (var requiredLength in _continuousDamagedSprings)
        {
            if (_springs[currentIndex].IsOperational())
                return false;

            if (currentIndex > 0 && _springs[currentIndex - 1].IsDamaged())
                return false;

            if (_springs.Count > currentIndex + requiredLength && _springs[currentIndex + requiredLength].IsDamaged())
                return false;

            var followingSprings = _springs.Skip(currentIndex).Take(requiredLength).ToList();

            if ((followingSprings.Count == 1 && _springs.Count > currentIndex + 1 &&
                 _springs[currentIndex + 1].IsDamaged()) ||
                followingSprings.Any(spring => spring.IsOperational()))
                return false;

            currentIndex += requiredLength + 1;
        }

        return true;
    }

    private void GetPossibleIndicesForLength()
    {
        var usedContinuousDamagedWithSpaces = 0;
        var counter = 0;
        int? firstDamagedSpring = _damagedSpringsIndices.Length > 0 ? _damagedSpringsIndices.First() : null;
        int? lastDamagedSpring = _damagedSpringsIndices.Length > 0 ? _damagedSpringsIndices.Last() : null;
        foreach (var lengthToCheck in _continuousDamagedSprings)
        {
            var minLengthNeeded = _continuousDamagedWithSpaces - usedContinuousDamagedWithSpaces;
            var maxLength = _springs.Count-minLengthNeeded + 1 - usedContinuousDamagedWithSpaces;
            var possibilityArray = new int[maxLength];
            var arrayCounter = 0;
            for (var i = usedContinuousDamagedWithSpaces; i < _springs.Count - minLengthNeeded + 1; i++)
            {
                if (usedContinuousDamagedWithSpaces == 0 && i > firstDamagedSpring)
                    break;

                var iPlusLengthToCheck = i + lengthToCheck;

                if (_springs.Count < iPlusLengthToCheck)
                    break;

                var followingSprings = _springs[i..iPlusLengthToCheck];
                
                if ((counter + 1 == _continuousDamagedSprings.Count && iPlusLengthToCheck < lastDamagedSpring) || 
                    _springs[i].IsOperational() || 
                    (i > 0 && _springs[i - 1].IsDamaged()) ||
                    (_springs.Count > iPlusLengthToCheck && _springs[iPlusLengthToCheck].IsDamaged()) ||
                    (followingSprings.Count == 1 && _springs.Count > i + 1 && _springs[i + 1].IsDamaged()) ||
                    followingSprings.Any(spring => spring.IsOperational()))
                    continue;

                possibilityArray[arrayCounter] = i;
                arrayCounter++;
            }

            var possibilityList = possibilityArray.Take(arrayCounter).ToList();

            _possibleArrangementsPerLength.Add(possibilityList);
            usedContinuousDamagedWithSpaces += lengthToCheck + 1;
            counter++;
        }
    }

    private void CountCombinationsHelper(List<List<int>> lists)
    {
        var springsCount = _springs.Count;
        var stack = new Stack<Tuple<int, int[]>>();
        stack.Push(new Tuple<int, int[]>(0, Array.Empty<int>()));

        while (stack.Count > 0)
        {
            var (currentIndex, currentArray) = stack.Pop();

            if (currentIndex == lists.Count)
            {
                if (CombinationIsPossible(currentArray))
                    _possibleArrangements++;

                continue;
            }

            foreach (var number in lists[currentIndex])
            {
                var currentArrayIsEmpty = currentArray.Length == 0;

                if (currentArrayIsEmpty && springsCount - currentIndex < _continuousDamagedWithSpaces)
                    break;

                if (!currentArrayIsEmpty &&
                    (number <= currentArray[^1] + 1 ||
                     number <= currentArray[^1] + _continuousDamagedSprings[currentIndex - 1]))
                    continue;

                var indexAfterLength = number + _continuousDamagedSprings[currentIndex];

                if (indexAfterLength < springsCount && _springs[indexAfterLength].IsDamaged())
                    continue;

                var newArray = new int[currentArray.Length + 1];
                currentArray.CopyTo(newArray, 0);
                newArray[^1] = number;
                stack.Push(new Tuple<int, int[]>(currentIndex + 1, newArray));
            }
        }
    }

    private bool CombinationIsPossible(int[] combination)
    {
        if (!ContainsAllContinuousDamagedSprings(combination))
            return false;

        for (var i = 0; i < combination.Length - 1; i++)
        {
            if (combination[i] + _continuousDamagedSprings[i] >= combination[i + 1])
                return false;
        }

        return true;
    }

    private bool ContainsAllContinuousDamagedSprings(int[] combination)
        => _damagedSpringsIndices
            .Select(damagedSpringIndex =>
                DamagedSpringIndexIsUsed(combination, damagedSpringIndex))
            .All(result => result);

    private bool DamagedSpringIndexIsUsed(int[] combination, int damagedSpringIndex)
    {
        var index = 0;
        foreach (var item in combination)
        {
            if (item == damagedSpringIndex)
                return true;

            var requiredLength = _continuousDamagedSprings[index];
            if (damagedSpringIndex >= item && damagedSpringIndex < item + requiredLength)
                return true;

            index++;
        }

        return false;
    }
}