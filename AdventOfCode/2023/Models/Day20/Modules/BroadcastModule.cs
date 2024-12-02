namespace _2023.Models.Day20.Modules;

public class BroadcastModule() : Module("broadcaster")
{
    public override BroadcastModule HandlePulse(Pulse pulse)
    {
        SendPulse(pulse);
        return this;
    }
}