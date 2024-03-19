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

    public async Task<long> GetPossibleArrangementsAsync()
    {
        if (_continuousDamagedWithSpaces != _springs.Count)
        {
            GetPossibleIndicesForLength();
            await CountCombinationsHelper(_possibleArrangementsPerLength);
            return _possibleArrangements;
        }

        if(!IsPossibleArrangement())
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
            
            if(_springs.Count > currentIndex + requiredLength && _springs[currentIndex + requiredLength].IsDamaged())
                return false;

            var followingSprings = _springs.Skip(currentIndex).Take(requiredLength).ToList();
            
            if ((followingSprings.Count == 1 && _springs.Count > currentIndex + 1 && _springs[currentIndex + 1].IsDamaged()) ||
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
        int? firstDamagedSpring = _damagedSpringsIndices.Count > 0 ? _damagedSpringsIndices.First() : null;
        int? lastDamagedSpring = _damagedSpringsIndices.Count > 0 ? _damagedSpringsIndices.Last() : null;
        foreach (var lengthToCheck in _continuousDamagedSprings)
        {
            var minLengthNeeded = _continuousDamagedWithSpaces - usedContinuousDamagedWithSpaces;
            var possibility = new List<int>();

            for (var i = usedContinuousDamagedWithSpaces; i < _springs.Count - minLengthNeeded + 1; i++)
            {
                if (_springs[i].IsOperational())
                    continue;

                if (usedContinuousDamagedWithSpaces == 0 && i > firstDamagedSpring)
                    break;

                if (counter + 1 == _continuousDamagedSprings.Count && i + lengthToCheck < lastDamagedSpring)
                    continue;

                if (i > 0 && _springs[i-1].IsDamaged())
                    continue;

                if (_springs.Count > i + lengthToCheck && _springs[i + lengthToCheck].IsDamaged())
                    continue;

                var followingSprings = _springs[i..(lengthToCheck + i)];
                if ((followingSprings.Count == 1 && _springs.Count > i + 1 && _springs[i+1].IsDamaged()) ||
                    followingSprings.Any(spring => spring.IsOperational()))
                    continue;

                possibility.Add(i);
            }

            _possibleArrangementsPerLength.Add(possibility);
            usedContinuousDamagedWithSpaces += lengthToCheck + 1;
            counter++;
        }
    }

    private async Task CountCombinationsHelper(List<List<int>> lists)
    {
        var stack = new Stack<Tuple<int, List<int>>>();
        stack.Push(new Tuple<int, List<int>>(0, new List<int>()));
        var springsCount = _springs.Count;

        while (stack.Count > 0)
        {
            var (currentIndex, currentList) = stack.Pop();

            if (currentIndex == lists.Count)
            {
                if (await CombinationIsPossibleAsync(currentList))
                    _possibleArrangements++;

                continue;
            }

            foreach (var number in lists[currentIndex])
            {
                var currentListIsEmpty = currentList.Count == 0;

                if (currentListIsEmpty && springsCount - currentIndex < _continuousDamagedWithSpaces)
                    break;

                if (!currentListIsEmpty && (number <= currentList[^1] + 1 ||
                                                 number <= currentList[^1] + _continuousDamagedSprings[currentIndex - 1]))
                    continue;

                var indexAfterLength = number + _continuousDamagedSprings[currentIndex];

                if (indexAfterLength < springsCount && _springs[indexAfterLength].IsDamaged())
                    continue;

                var newList = new List<int>(currentList) {number};
                stack.Push(new Tuple<int, List<int>>(currentIndex + 1, newList));
            }
        }
    }
    
    private async Task<bool> CombinationIsPossibleAsync(List<int> combination)
    {
        return await Task.Run(() => CombinationIsPossible(combination));
    }

    private async Task<bool> CombinationIsPossible(List<int> combination)
    {
        if (!await ContainsAllContinuousDamagedSpringsAsync(combination))
            return false;

        for (var i = 0; i < combination.Count - 1; i++)
        {
            if (combination[i] + _continuousDamagedSprings[i] >= combination[i + 1]) 
                return false;
        }

        return true;
    }

    private async Task<bool> ContainsAllContinuousDamagedSpringsAsync(List<int> combination)
    {
        return await Task.Run(() => ContainsAllContinuousDamagedSprings(combination));
    }

    private async Task<bool> ContainsAllContinuousDamagedSprings(List<int> combination)
    {
        var tasks = _damagedSpringsIndices.Select(async damagedSpringsIndex => await DamagedSpringIndexIsUsedAsync(combination, damagedSpringsIndex));
        var result = await Task.WhenAll(tasks);
        return result.All(result => result);
    }

    private async Task<bool> DamagedSpringIndexIsUsedAsync(List<int> combination, int damagedSpringIndex)
    {
        return await Task.Run(() => DamagedSpringIndexIsUsed(combination, damagedSpringIndex));
    }

    private bool DamagedSpringIndexIsUsed(List<int> combination, int damagedSpringIndex)
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