namespace _2024.Models.Day13;

public static class Day13Extensions
{
    public static ArcadeMachine[] CreateArcadeMachines(this IEnumerable<string> input)
    {
        var arcadeMachines = new List<ArcadeMachine>();
        var arcadeInput = new string[3];
        var lineCounter = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                lineCounter = 0;
                arcadeMachines.Add(new ArcadeMachine(arcadeInput));
                continue;
            }

            arcadeInput[lineCounter] = line;
            lineCounter++;
        }

        arcadeMachines.Add(new ArcadeMachine(arcadeInput));

        return arcadeMachines.ToArray();
    }
}