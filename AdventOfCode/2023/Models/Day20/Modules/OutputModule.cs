namespace _2023.Models.Day20.Modules;

public class OutputModule(string name = "output") :Module(name)
{
    public override OutputModule HandlePulse(Pulse pulse) 
        => this;
}