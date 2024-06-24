namespace AdventOfCode2023_1.Models.Day19;

public class Workflow
{
    public string Name { get; }
    public List<Rule> Rules { get; }
    

    public Workflow(string workflowLine)
    {
        Name = workflowLine.Split('{').First();
        Rules = new List<Rule>();

        var rulesPart = workflowLine.Split('{').Last();
        var rulesList = rulesPart.Split(',').ToList();
        foreach (var rulesLine in rulesList) 
            Rules.Add(new Rule(rulesLine));
    }
}