namespace AdventOfCode2023_1.Models.Day20.Modules;

public class ConjunctionModule(string name) : Module(name)
{
    private Dictionary<string, Pulse> Memory { get; } = new();
    public bool WroteHighPulse { get; set; }
    
    public override void AddInitiator(Module module)
    {
        base.AddInitiator(module);
        Memory.Add(module.Name, new Pulse{Initiator = module.Name, IsHighPulse = false});
    }

    public override void Reset()
    {
        Memory.Values.ToList().ForEach(pulse => pulse.IsHighPulse = false);
        WroteHighPulse = false;
        base.Reset();
    }

    public override void HandlePulse(Pulse pulse)
    {
        UpdateMemory(pulse);

        var pulseToSend = new Pulse
        {
            IsHighPulse = !Memory.All(p => p.Value.IsHighPulse)
        };

        WroteHighPulse = pulseToSend.IsHighPulse;

        SendPulse(pulseToSend);
    }

    private void UpdateMemory(Pulse pulse)
    {
        var initiator = pulse.Initiator;
        Memory[initiator].IsHighPulse = pulse.IsHighPulse;
    }
}