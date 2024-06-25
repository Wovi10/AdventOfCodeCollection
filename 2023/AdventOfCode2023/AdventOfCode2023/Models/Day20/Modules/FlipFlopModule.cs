namespace AdventOfCode2023_1.Models.Day20.Modules;

public class FlipFlopModule(string name) : Module(name)
{
    public override void HandlePulse(Pulse pulse)
    {
        if (pulse.IsHighPulse)
            return;

        IsTurnedOn = !IsTurnedOn;

        var pulseToSend = new Pulse {IsHighPulse = IsTurnedOn};
        SendPulse(pulseToSend);
    }
}