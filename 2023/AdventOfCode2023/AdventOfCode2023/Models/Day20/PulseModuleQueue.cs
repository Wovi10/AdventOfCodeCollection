namespace AdventOfCode2023_1.Models.Day20;

public static class PulseModuleQueue
{
    private static long TotalHighPulses { get; set; }
    private static long TotalLowPulses { get; set; }
    public static long TotalPulses => TotalHighPulses * TotalLowPulses;

    private static readonly Queue<Pulse> Pulses = new();
    private static readonly Queue<Module> Modules = new();

    public static void Enqueue(this Module module, Pulse pulse)
    {
        if (pulse.IsHighPulse)
            TotalHighPulses++;
        else
            TotalLowPulses++;

        Pulses.Enqueue(pulse);
        Modules.Enqueue(module);
    }

    public static void Dequeue()
    {
        if (Pulses.Count == 0)
            return;

        var pulse = Pulses.Dequeue();
        var module = Modules.Dequeue();

        module.IsProcessingPulse = true;
        module.HandlePulse(pulse);
        module.IsProcessingPulse = false;
    }
}