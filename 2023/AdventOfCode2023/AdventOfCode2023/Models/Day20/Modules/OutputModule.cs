namespace AdventOfCode2023_1.Models.Day20.Modules;

public class OutputModule(string name = "output") :Module(name)
{
    public override OutputModule HandlePulse(Pulse pulse) 
        => this;
}