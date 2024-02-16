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
        
    }
}