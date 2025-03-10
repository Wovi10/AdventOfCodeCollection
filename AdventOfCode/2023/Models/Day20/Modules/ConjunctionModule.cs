﻿namespace _2023.Models.Day20.Modules;

public class ConjunctionModule(string name) : Module(name)
{
    private Dictionary<string, Pulse> Memory { get; } = new();
    public bool WroteHighPulse { get; set; }

    public override void AddInitiator(Module module)
    {
        base.AddInitiator(module);
        Memory.Add(module.Name, new Pulse {Initiator = module.Name, IsHighPulse = false});
    }

    public override void Reset()
    {
        Memory.Values.ToList().ForEach(pulse => pulse.IsHighPulse = false);
        WroteHighPulse = false;
        base.Reset();
    }

    public override ConjunctionModule HandlePulse(Pulse pulse)
    {
        UpdateMemory(pulse);

        var pulseToSend = new Pulse
        {
            IsHighPulse = !Memory.All(p => p.Value.IsHighPulse)
        };

        WroteHighPulse = pulseToSend.IsHighPulse || WroteHighPulse;

        SendPulse(pulseToSend);

        return this;
    }

    private void UpdateMemory(Pulse pulse)
    {
        var initiator = pulse.Initiator;

        if (initiator == null)
            return;

        Memory[initiator].IsHighPulse = pulse.IsHighPulse;
    }
}