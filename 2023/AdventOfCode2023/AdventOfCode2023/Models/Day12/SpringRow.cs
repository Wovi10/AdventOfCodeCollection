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

        // Run over all indices and check for possible arrangements
        // You can stop earlier if the required length is longer than the remaining springs
        // You know this if the remaining springs are less than the remaining continuous damaged springs + a space for each one of them - 1
        // On this index, check for the possible combinations of the number of continuous damaged springs
        // Go over each of these indices and check for the next possible arrangements
        // The next possible arrangements can be checked as of the current index + the number of continuous damaged springs + 2 (You have to include a space)
        // Do this for all the continuous damaged springs until you reach the end of that list
        // Then go on to the next index and do the same
        // You can only add 1 to PossibleArrangements if you reach the end of the ContinuousDamagedSprings list
        // for (var i = 0; i < Springs.Count; i++)
        // {
        //     CheckPositionIsPossible(i);
        // }

        GetPossibleIndicesForLength();
        GenerateAllCombinations(PossibleArrangementsPerLength);
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
        // Remove all combinations that are not possible
        // A combination is not possible if any of the indices is not at least 2 indices away from the previous one
        // This is because you need at least 1 space between each continuous damaged spring
        for (var i = 0; i < combinations.Count; i++)
        {
            var combination = combinations[i];
            for (var j = 0; j < combination.Count - 1; j++)
            {
                if (combination[j + 1] - combination[j] < 2)
                {
                    combinations.RemoveAt(i);
                    i--;
                    break;
                }
            }
        }
    }

    private void GenerateCombinationsHelper(List<List<int>> lists, int index, List<int> currentCombination, List<List<int>> combinations)
    {
        if (index == lists.Count)
        {
            combinations.Add(new List<int>(currentCombination));
            return;
        }

        foreach (var number in lists[index])
        {
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

    private void CheckPositionIsPossible(int startIndex)
    {
        var possibility = new List<int>();
        var indexToCheck = startIndex;
        var counter = 0;
        foreach (var lengthToCheck in ContinuousDamagedSprings)
        {
            var nextPossibleIndex = GetNextPossibleIndex(indexToCheck, Springs, lengthToCheck);
            if (nextPossibleIndex == null)
                return;
            possibility.Add(nextPossibleIndex.Value);
            indexToCheck = nextPossibleIndex.Value + lengthToCheck + 1;
        }

        PossibleArrangementsPerLength.Add(possibility);
        PossibleArrangements++;
    }

    private int? GetNextPossibleIndex(int indexToCheck, List<Spring> springs, int originalLengthToCheck)
    {
        var lengthToCheck = originalLengthToCheck;
        var firstMatchingIndex = indexToCheck;
        for (var i = indexToCheck; i < springs.Count; i++)
        {
            if (springs[i].IsOperational())
            {
                lengthToCheck = originalLengthToCheck;
                firstMatchingIndex = i + 1;
                continue;
            }

            lengthToCheck--;
            if (lengthToCheck == 0)
                return firstMatchingIndex;
        }

        return null;
    }
}