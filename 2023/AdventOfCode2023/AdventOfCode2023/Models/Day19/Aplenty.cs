using AdventOfCode2023_1.Shared;
using Constants = UtilsCSharp.Utils.Constants;

namespace AdventOfCode2023_1.Models.Day19;

public class Aplenty
{
    private List<Workflow> Workflows { get; }
    private List<Part> Parts { get; }
    private Dictionary<string, List<Rule>> Rules { get; }

    public Aplenty(List<string> input)
    {
        var workflowInput = input.Where(line => !line.StartsWith('{') && line != Constants.EmptyString).ToList();
        if (Variables.RunningPartOne)
        {
            Workflows = new List<Workflow>();
            Parts = new List<Part>();

            foreach (var workflowLine in workflowInput)
                Workflows.Add(new Workflow(workflowLine));

            var partInput = input.Where(line => line.StartsWith('{')).ToList();

            foreach (var partLine in partInput)
                Parts.Add(new Part(partLine));
            
            return;
        }

        Rules = new Dictionary<string, List<Rule>>();
        foreach (var workflow in workflowInput)
        {
            var label = workflow[..workflow.IndexOf('{')];

            Rules.Add(label, []);
            foreach (var rule in workflow[(workflow.IndexOf('{') + 1)..^1].Split(','))
            {
                var tempRule = new Rule(rule);

                Rules[label].Add(tempRule);
            }
        }
    }

    public void Process()
    {
        foreach (var part in Parts)
        {
            do
            {
                var currentWorkflow = Workflows.First(w => w.Name == part.CurrentWorkflow);
                part.Process(currentWorkflow);
            } while (part.Accepted == null);
        }
    }

    public long GetRatings()
    {
        var partsToUse = Parts.Where(part => part.Accepted == true).ToList();
        var xRating = partsToUse.Sum(part => part.XRating);
        var mRating = partsToUse.Sum(part => part.MRating);
        var sRating = partsToUse.Sum(part => part.SRating);
        var aRating = partsToUse.Sum(part => part.ARating);

        return xRating + mRating + sRating + aRating;
    }

    public long CheckCombos(int[] startMin, int[] startMax, string ruleName)
    {
        var minXmas = new int[startMin.Length];
        Array.Copy(startMin, minXmas, startMin.Length);

        var maxXmas = new int[startMax.Length];
        Array.Copy(startMax, maxXmas, startMax.Length);

        var returnValue = 0L;

        foreach (var rule in Rules[ruleName])
        {
            var typeIndex = (int)rule.Type;
            switch (rule.Comparer)
            {
                case '=':
                    returnValue += DoResult(rule.NextState);
                    break;
                case '<':
                    var tempMax = maxXmas[typeIndex];
                    if (minXmas[typeIndex] < rule.CompareValue)
                    {
                        maxXmas[typeIndex] = int.Min(maxXmas[typeIndex], rule.CompareValue - 1);
                        returnValue += DoResult(rule.NextState);
                    }

                    maxXmas[typeIndex] = tempMax;
                    minXmas[typeIndex] = int.Max(minXmas[typeIndex], rule.CompareValue);
                    break;
                case '>':
                    var tempMin = minXmas[typeIndex];
                    if (maxXmas[typeIndex] > rule.CompareValue)
                    {
                        minXmas[typeIndex] = int.Max(minXmas[typeIndex], rule.CompareValue + 1);
                        returnValue += DoResult(rule.NextState);
                    }

                    minXmas[typeIndex] = tempMin;
                    maxXmas[typeIndex] = int.Min(maxXmas[typeIndex], rule.CompareValue);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return returnValue;

        long DoResult(string result)
        {
            if (result == "R")
                return 0;
            if (result != "A")
                return CheckCombos(minXmas, maxXmas, result);

            var newResult = 1L;
            for (var i = maxXmas.GetLowerBound(0); i <= maxXmas.GetUpperBound(0); i++) 
                newResult *= maxXmas[i] - minXmas[i] + 1;

            return newResult;

        }
    }
}