namespace AdventOfCode2023_1.Models.Day20.Modules;

public class FlipFlopModule(string name) : Module(name)
{
    public override FlipFlopModule HandlePulse(Pulse pulse)
    {
        if (pulse.IsHighPulse)
            return this;

        IsTurnedOn = !IsTurnedOn;

        var pulseToSend = new Pulse {IsHighPulse = IsTurnedOn};
        SendPulse(pulseToSend);
        return this;
    }
}