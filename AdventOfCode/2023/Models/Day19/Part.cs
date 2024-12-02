using Type = _2023.Models.Day19.Type;

namespace _2023.Models.Day19;

public class Part
{
    public Part(string partLine)
    {
        CurrentWorkflow = "in";
        Accepted = null;
        var partRatings = partLine.Split('{').Last().Split(',').ToList();

        XRating = int.Parse(partRatings[0][2..]);
        MRating = int.Parse(partRatings[1][2..]);
        ARating = int.Parse(partRatings[2][2..]);
        var sRatingPart = partRatings[3].Split('}').First();
        SRating = int.Parse(sRatingPart[2..]);
    }

    public string CurrentWorkflow { get; set; }
    public bool? Accepted { get; set; }
    public int XRating { get; }
    public int MRating { get; }
    public int SRating { get; }
    public int ARating { get; }

    public void Process(Workflow currentWorkflow)
    {
        foreach (var rule in currentWorkflow.Rules)
        {
            var valueToCompare = rule.Type switch
            {
                Type.X => XRating,
                Type.M => MRating,
                Type.S => SRating,
                Type.A => ARating,
                _ => 0
            };

            if (rule.Works(valueToCompare))
            {
                SetCurrentWorkFlow(rule.NextState);
                return;
            }

            if (rule.Type == Type.Unknown)
            {
                SetCurrentWorkFlow(rule.NextState);
                return;
            }
        }
    }

    private void SetCurrentWorkFlow(string nextWorkflow)
    {
        CurrentWorkflow = nextWorkflow;

        if (CurrentWorkflow is "A" or "R")
            Accepted = CurrentWorkflow == "A";
    }
}