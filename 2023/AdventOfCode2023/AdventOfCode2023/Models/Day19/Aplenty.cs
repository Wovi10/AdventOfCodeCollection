using Constants = UtilsCSharp.Utils.Constants;

namespace AdventOfCode2023_1.Models.Day19;

public class Aplenty
{
    private List<Workflow> Workflows { get; }
    private List<Part> Parts { get; }

    public Aplenty(List<string> input)
    {
        Workflows = new List<Workflow>();
        Parts = new List<Part>();

        var workflowInput = input.Where(line => !line.StartsWith('{') && line != Constants.EmptyString).ToList();

        foreach (var workflowLine in workflowInput)
            Workflows.Add(new Workflow(workflowLine));

        var partInput = input.Where(line => line.StartsWith('{')).ToList();

        foreach (var partLine in partInput)
            Parts.Add(new Part(partLine));
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
}