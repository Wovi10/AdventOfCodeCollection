namespace AdventOfCode2023_1.Models.Day20.Modules;

public class BroadcastModule() : Module("broadcaster")
{
    public override BroadcastModule HandlePulse(Pulse pulse)
    {
        SendPulse(pulse);
        return this;
    }
}