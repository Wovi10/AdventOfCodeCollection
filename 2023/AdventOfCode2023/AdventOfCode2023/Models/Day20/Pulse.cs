namespace AdventOfCode2023_1.Models.Day20;

public class Pulse
{
    public bool IsHighPulse { get; set; }
    public bool IsLowPulse => !IsHighPulse;
    public string? Initiator { get; set; }

    public void ChangeInitiator(string name)
    {
        Initiator = name;
    }
}