using AdventOfCode2023_1.Models.Day20.Modules;
using AdventOfCode2023_1.Models.Day20.Utils;

namespace AdventOfCode2023_1.Models.Day20;

public class DesertMachineHeadquarters
{
    private ButtonModule Button { get; }
    public long[] HelperCounter { get; } = new long[4];
    
    public DesertMachineHeadquarters(List<string> input)
    {
        var allModules = new List<Module>();
        Button = new ButtonModule();
        allModules.Add(Button);

        var destinationParts = new List<string>();

        foreach (var parts in input.Select(line => line.Split(" -> ")))
        {
            destinationParts.Add(parts.Last());

            var namePart = parts.First();
            var prefix = namePart[0];
            var name = namePart[1..];

            switch (prefix)
            {
                case Constants.BroadcastPrefix:
                    var broadcastModule = new BroadcastModule();
                    allModules.Add(broadcastModule);
                    break;
                case Constants.ConjunctionPrefix:
                    var conjunctionModule = new ConjunctionModule(name);
                    allModules.Add(conjunctionModule);
                    break;
                case Constants.FlipFlopPrefix:
                    var flipFlopModule = new FlipFlopModule(name);
                    allModules.Add(flipFlopModule);
                    break;
                case Constants.OutputPrefix:
                    var outputModule = new OutputModule();
                    allModules.Add(outputModule);
                    break;
            }
        }
        
        var counter = 1;
        foreach (var destinationPart in destinationParts)
        {
            var moduleToAdjust = allModules[counter];

            var destinations = destinationPart.Split(", ");
            foreach (var destination in destinations)
            {
                var moduleToAdd = allModules.FirstOrDefault(module => module.Name == destination);
                moduleToAdd ??= new RxModule();

                moduleToAdjust.AddDestination(moduleToAdd);
                moduleToAdd.AddInitiator(moduleToAdjust);

                if (moduleToAdd is RxModule) 
                    allModules.Add(moduleToAdd);
            }

            counter++;
        }

        Button.AllModules = allModules;
        Button.AddDestination(allModules.First(module => module.Name == "broadcaster"));
    }

    public void PressButton(int times = 0)
        => Button.PressButton(HelperCounter, times);

    public static long GetTotalPulses() 
        => PulseModuleQueue.TotalPulses;
}