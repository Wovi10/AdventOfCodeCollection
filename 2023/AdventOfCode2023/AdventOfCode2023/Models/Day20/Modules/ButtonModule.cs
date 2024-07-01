using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1.Models.Day20.Modules;

public class ButtonModule() : Module("aptly")
{
    public List<Module> AllModules { get; set; } = new();
    private int TotalPresses { get; set; }

    public override ButtonModule HandlePulse(Pulse pulse)
    {
        SendPulse(pulse);
        return this;
    }

    public override void Reset()
    {
        TotalPresses = 0;
        base.Reset();
    }

    private void PressButton()
    {
        if (AllModules.Any(module => module.IsProcessingPulse))
            return;

        TotalPresses++;

        var lowPulse = new Pulse {IsHighPulse = false};
        HandlePulse(lowPulse);
    }

    public ButtonModule PressButton(long[] helperCounter, int times)
    {
        if (Variables.RunningPartOne)
        {
            do
            {
                PressButton();
            } while (TotalPresses < times);

            return this;
        }

        var rxModule = (RxModule)AllModules.First(module => module is RxModule);
        var rxInitiator = (ConjunctionModule)rxModule.Initiators.First();
        var parents = rxInitiator.Initiators.Cast<ConjunctionModule>();
        var counter = 0;

        foreach (var parent in parents)
        {
            if (parent == null)
                continue;

            do
            {
                PressButton();
            } while (!parent.WroteHighPulse);

            helperCounter[counter] = TotalPresses;
            counter++;

            AllModules.ForEach(module => module.Reset());
        }

        return this;
    }
}