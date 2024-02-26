using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day12;

public class SpringRow
{
    public SpringRow(string input)
    {
        var splitInput = input.Split(Constants.Space);

        var springsFromInput = splitInput.First();
        Springs = SetSprings(springsFromInput);

        var arrangementsFromInput = splitInput.Last();
        ContinuousDamagedSprings = SetContinuousArrangements(arrangementsFromInput);
    }

    public List<Spring> Springs { get; set; }
    private List<int> DamagedSpringsIndices => Springs.Select((spring, index) => (spring, index)).Where(spring => spring.spring.IsDamaged()).Select(spring => spring.index).ToList();
    public List<int> ContinuousDamagedSprings { get; set; }
    public int PossibleArrangements { get; set; }
    private List<List<int>> PossibleArrangementsPerLength = new();
    private List<List<int>> AllPossibleArrangements = new();

    private static List<Spring> SetSprings(string springsFromInput)
    {
        return springsFromInput.Select(spring => new Spring(spring)).ToList();
    }

    private static List<int> SetContinuousArrangements(string arrangementsFromInput)
    {
        return arrangementsFromInput.Split(Constants.Comma).Select(int.Parse).ToList();
    }

    public void SetPossibleArrangements()
    {
        var possibleArrangements = 0;
        var continuousDamagedWithSpaces = (ContinuousDamagedSprings.Count - 1) + ContinuousDamagedSprings.Sum();
        if (continuousDamagedWithSpaces == Springs.Count)
        {
            PossibleArrangements = 1;
            return;
        }

        GetPossibleIndicesForLength();
        GenerateAllCombinations(PossibleArrangementsPerLength);
        PossibleArrangements = AllPossibleArrangements.Count;
    }

    private void GenerateAllCombinations(List<List<int>> lists)
    {
        var combinations = new List<List<int>>();
        GenerateCombinationsHelper(lists, 0, new List<int>(), combinations);
        FilterImpossibleCombinations(combinations);
        AllPossibleArrangements = combinations;
    }

    private void FilterImpossibleCombinations(List<List<int>> combinations)
    {
        for (var i = 0; i < combinations.Count; i++)
        {
            var combination = combinations[i];
            
            if (!ContainsAllContinuousDamagedSprings(combination))
            {
                combinations.RemoveAt(i);
                i--;
                continue;
            }
            
            if (!IsSorted(combination))
            {
                combinations.RemoveAt(i);
                i--;
                continue;
            }

            for (var j = 0; j < combination.Count - 1; j++)
            {
                var currentIndex = combination[j];
                var nextIndex = combination[j + 1];

                if (currentIndex + 1 >= nextIndex)
                {
                    combinations.RemoveAt(i);
                    i--;
                    break;
                }

                if (ContainsOverlappingDamagedSprings(currentIndex, nextIndex, ContinuousDamagedSprings[j]))
                {
                    combinations.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }

    private bool ContainsAllContinuousDamagedSprings(List<int> combination)
    {
        return DamagedSpringsIndices.All(damagedSpringIndex => DamagedSpringIndexIsUsed(combination, damagedSpringIndex));
    }

    private bool DamagedSpringIndexIsUsed(List<int> combination, int damagedSpringIndex)
    {
        for (var i = 0; i < combination.Count; i++)
        {
            var currentIndex = combination[i];
            var currentRequiredLength = ContinuousDamagedSprings[i];
            for (var checkingIndex = currentIndex; checkingIndex < currentIndex + currentRequiredLength; checkingIndex++)
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

        var previousSpringIsDamaged = currentIndex > 0 && Springs[currentIndex - 1].IsDamaged();
        var nextSpringIsDamaged = indexAfterLength < Springs.Count && Springs[indexAfterLength].IsDamaged();
        var nextIndexOverlaps = currentIndex + requiredLength >= nextIndex;

        return previousSpringIsDamaged || nextSpringIsDamaged || nextIndexOverlaps || currentIndex + requiredLength >= nextIndex;
    }

    private static bool IsSorted(List<int> combination)
    {
        var previousIndex = -1;
        foreach (var currentIndex in combination)
        {
            if (currentIndex <= previousIndex)
                return false;

            previousIndex = currentIndex;
        }

        return true;
    }

    private static void GenerateCombinationsHelper(List<List<int>> lists, int index, List<int> currentCombination, List<List<int>> combinations)
    {
        if (index == lists.Count)
        {
            combinations.Add(new List<int>(currentCombination));
            return;
        }

        foreach (var number in lists[index])
        {
            if (currentCombination.Count != 0 && number <= currentCombination[^1] + 1)
                continue;

            currentCombination.Add(number);
            GenerateCombinationsHelper(lists, index + 1, currentCombination, combinations);
            currentCombination.RemoveAt(currentCombination.Count - 1);
        }
    }

    private void GetPossibleIndicesForLength()
    {
        foreach (var lengthToCheck in ContinuousDamagedSprings)
        {
            var possibility = new List<int>();

            for (var i = 0; i < Springs.Count; i++)
            {
                if (Springs[i].IsPossible(lengthToCheck, Springs, i))
                    possibility.Add(i);
            }

            PossibleArrangementsPerLength.Add(possibility);
        }
    }
}