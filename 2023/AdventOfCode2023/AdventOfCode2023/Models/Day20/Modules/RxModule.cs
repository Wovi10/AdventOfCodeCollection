namespace AdventOfCode2023_1.Models.Day20.Modules;

public class RxModule() : Module("rx")
{
    public int LowPulsesReceived { get; set; }

    public override RxModule HandlePulse(Pulse pulse)
    {
        if (pulse.IsLowPulse)
            LowPulsesReceived++;
        
        return this;
    }

    public override void Reset()
    {
        LowPulsesReceived = 0;
        base.Reset();
    }
}