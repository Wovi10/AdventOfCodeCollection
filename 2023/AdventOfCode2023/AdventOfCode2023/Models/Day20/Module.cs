namespace AdventOfCode2023_1.Models.Day20;

public abstract class Module(string name)
{
    public string Name { get; set; } = name;
    public bool IsTurnedOn { get; set; }
    public List<Module> Initiators { get; set; } = new();
    private List<Module> Destinations { get; set; } = new();
    public bool IsProcessingPulse { get; set; }

    public abstract void HandlePulse(Pulse pulse);

    public virtual void AddInitiator(Module module)
    {
        Initiators ??= new List<Module>();
        Initiators.Add(module);
    }

    public virtual void AddDestination(Module module)
    {
        Destinations ??= new List<Module>();
        Destinations.Add(module);
    }

    protected void SendPulse(Pulse pulse)
    {
        pulse.ChangeInitiator(Name);
        foreach (var destination in Destinations)
            destination.Enqueue(pulse);

        foreach (var _ in Destinations)
            PulseModuleQueue.Dequeue();
    }

    public virtual void Reset()
    {
        IsTurnedOn = false;
        IsProcessingPulse = false;
    }
}