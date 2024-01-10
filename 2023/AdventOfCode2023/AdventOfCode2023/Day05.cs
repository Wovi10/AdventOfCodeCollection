using AdventOfCode2023_1.Shared;

namespace AdventOfCode2023_1;

public class Day05 : DayBase
{
    private static readonly List<string> Input = SharedMethods.GetInput("05", true);

    private static List<List<List<int>>> InputParts = GenerateInputParts();

    private static List<List<List<int>>> GenerateInputParts()
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
                inputPart = [];
            }
        }

        for (var i = 1; i < inputParts.Count; i++)
        {
            inputParts[i].RemoveAt(0);
        }

        var result = GenerateMappings(inputParts); 
        return result;
    }

    private static List<List<List<int>>> GenerateMappings(List<List<string>> inputPartsAsStrings)
    {
        var result = new List<List<List<int>>>();
        for (var i = 1; i < inputPartsAsStrings.Count; i++)
        {
            var inputPart = inputPartsAsStrings[i];
            var convertedInputPart = inputPart
                    .Select(line =>
                    {
                        return line
                            .Split(Constants.Space)
                            .Where(location => int.TryParse(location, out _))
                            .Select(int.Parse)
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

    private static int GetLowestLocationNumber()
    {
        var seeds = GetSeeds();
        var soilsOptions = ConvertToSoilOptions(seeds);
        return seeds.Min();
    }

    public override void PartTwo()
    {
        throw new NotImplementedException();
    }

    #region Part1
    private static List<int> GetSeeds()
    {
        var seeds = Input
            .First() // First line
            .Split(Constants.Colon)
            .Last() // Everything after colon
            .Trim()
            .Split(Constants.Space)
            .ToList();

        return seeds.Where(seed => int.TryParse(seed, out _)).Select(int.Parse).ToList();
    }

    private static List<int> ConvertToSoilOptions(List<int> seeds)
    {
        var soils = new List<int>();
        foreach (var seed in seeds)
        {
            soils.AddRange(GetSoils(seed));
        }

        return soils;
    }

    private static List<int> GetSoils(int initialSeed)
    {
        var soils = new List<int>();
        for (var i = 0; i < InputParts[0].Count; i++)
        {
            var seed = initialSeed;
            var source = GetSource(InputParts[0][i]);
            var destination = GetDestination(InputParts[0][i]);
            var range = GetRange(InputParts[0][i]);
            if (seed >= int.Min(int.Min(source, source + range), int.Min(destination, destination + range)) 
                && 
                seed <= int.Max(int.Max(source, source + range), int.Max(destination, destination + range)))
            {
                seed += range;
            }

            soils.Add(seed);
        }

        return soils;
    }

    private static int GetDestination(List<int> inputPart)
    {
        return inputPart[0];
    }

    private static int GetRange(List<int> inputPart)
    {
        return inputPart[2];
    }

    private static int GetSource(List<int> inputPart)
    {
        return inputPart[1];
    }

    #endregion
}