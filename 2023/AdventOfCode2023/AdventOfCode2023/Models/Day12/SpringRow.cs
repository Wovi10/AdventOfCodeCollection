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
    }
}