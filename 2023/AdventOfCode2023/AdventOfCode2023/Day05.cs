using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("05");

    private static readonly List<List<List<long>>> InputParts = GenerateInputParts();

    private static List<List<List<long>>> GenerateInputParts()
    {
        var inputParts = new List<List<string>>();
        var inputPart = new List<string>();
        foreach (var line in Input)
        {
            if (!string.IsNullOrWhiteSpace(line))
                inputPart.Add(line.Trim());
            else
            {
                inputParts.Add(inputPart);
                inputPart = new List<string>();
            }
        }
        inputParts.Add(inputPart);

        for (var i = 1; i < inputParts.Count; i++) 
            inputParts[i].RemoveAt(0);

        var result = GenerateMappings(inputParts); 
        return result;
    }

    private static List<List<List<long>>> GenerateMappings(List<List<string>> inputPartsAsStrings)
    {
        var result = new List<List<List<long>>>();
        for (var i = 1; i < inputPartsAsStrings.Count; i++)
        {
            var inputPart = inputPartsAsStrings[i];
            var convertedInputPart = inputPart
                    .Select(line =>
                    {
                        return line
                            .Split(Constants.Space)
                            .Where(location => long.TryParse(location, out _))
                            .Select(long.Parse)
                            .ToList();
                    })
                    .ToList();
            result.Add(convertedInputPart);
        }

        return result;
    }
    
    public override void PartOne()
    {
        var result = GetLowestLocationNumber();
        SharedMethods.AnswerPart(1, result);
    }

    private static long GetLowestLocationNumber()
    {
        long? possibility = null;
        var seeds = GetSeeds();
        var counter = 0;
        foreach (var seed in seeds.Distinct())
        {
            counter++;
            var soils = GetConvertedValue(seed, 0);
            foreach (var soil in soils)
            {
                var fertilizers = GetConvertedValue(soil, 1);
                foreach (var fertilizer in fertilizers)
                {
                    var waters = GetConvertedValue(fertilizer, 2);
                    foreach (var water in waters)
                    {
                        var lights = GetConvertedValue(water, 3);
                        foreach (var light in lights)
                        {
                            var temperatures = GetConvertedValue(light, 4);
                            foreach (var temperature in temperatures)
                            {
                                var humidities = GetConvertedValue(temperature, 5);
                                foreach (var humidity in humidities)
                                {
                                    var locations = GetConvertedValue(humidity, 6);
                                    var newPossibility = locations.Min();
                                    possibility = possibility == null 
                                        ? newPossibility 
                                        : Math.Min(possibility.Value, newPossibility);
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Seed {counter} of {seeds.Count} done");
        }

        return possibility ?? 0;
        // var soilsOptions = ConvertSourceToDestination(seeds, 0);
        // var fertilizerOptions = ConvertSourceToDestination(soilsOptions, 1);
        // var waterOptions = ConvertSourceToDestination(fertilizerOptions, 2);
        // var lightOptions = ConvertSourceToDestination(waterOptions, 3);
        // var temperatureOptions = ConvertSourceToDestination(lightOptions, 4);
        // var humidityOptions = ConvertSourceToDestination(temperatureOptions, 5);
        // var locationOptions = ConvertSourceToDestination(humidityOptions, 6);
        // return locationOptions.Min();
    }

    public override void PartTwo()
    {
        throw new NotImplementedException();
    }

    #region Part1
    private static List<long> GetSeeds()
    {
        var seeds = Input
            .First() // First line
            .Split(Constants.Colon)
            .Last() // Everything after colon
            .Trim()
            .Split(Constants.Space)
            .ToList();

        return seeds.Where(seed => long.TryParse(seed, out _)).Select(long.Parse).ToList();
    }

    private static List<long> ConvertSourceToDestination(List<long> sourceList, int inputPartsIndex)
    {
        var destinationList = new List<long>();
        foreach (var source in sourceList)
        {
            destinationList.AddRange(GetConvertedValue(source, inputPartsIndex));
        }

        return destinationList;
    }

    private static IEnumerable<long> GetConvertedValue(long initialLocation, int inputPartsIndex)
    {
        var destinationList = new List<long>();
        var inputPart = InputParts[inputPartsIndex];
        foreach (var inputPartLine in inputPart)
        {
            var newLocation = initialLocation;

            var source = GetSource(inputPartLine);
            var destination = GetDestination(inputPartLine);
            var range = GetRange(inputPartLine);

            var biggest = long.Max(source, destination);
            var smallest = long.Min(source, destination);

            if (newLocation >= smallest && newLocation <= biggest + range)
            {
                if (biggest == source)
                    newLocation += range;
                else
                    newLocation -= range;
            }

            if (newLocation > 0) 
                destinationList.Add(newLocation);
        }

        return destinationList;
    }

    private static long GetDestination(List<long> inputPart)
    {
        return inputPart[0];
    }

    private static long GetRange(List<long> inputPart)
    {
        return inputPart[2];
    }

    private static long GetSource(List<long> inputPart)
    {
        return inputPart[1];
    }

    #endregion
}