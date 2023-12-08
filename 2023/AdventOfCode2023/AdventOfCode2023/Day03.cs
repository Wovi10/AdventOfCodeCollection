namespace AdventOfCode2023_1;

public static class Day03
{
    private static readonly string FilePath = Path.Combine("../../..", "Input/Day03/Day03.in");
    private static readonly string MockFilePath = Path.Combine("../../..", "Input/Day03/MockDay03.in");
    private static readonly string FullPath = Path.Combine(Directory.GetCurrentDirectory(), MockFilePath);
    private static readonly string InputFile = File.ReadAllText(FullPath);

    public static void Run()
    {
        Console.WriteLine("Starting day 2 challenge: Cube Conundrum");
        PartOne();
        // PartTwo();
        Console.WriteLine();
    }

    private static void PartOne()
    {
        var result = GetSumPartNumbers();
        Console.WriteLine($"Answer of part 1 is: \n{result}");
    }

    private static void PartTwo()
    {
        throw new NotImplementedException();
    }
    
    # region
    private static int GetSumPartNumbers()
    {
        return GetPartNumbers().Sum();
    }

    private static List<int> GetPartNumbers()
    {
        var input = GetInput();
        var symbolIndices = DecideSymbolIndices(input);
        var engineNumbers = DecideNumbers(input);
        return new List<int>();
    }

    private static List<EngineNumber> DecideNumbers(List<string> input)
    {
        var output = new List<EngineNumber>();
        for (var i = 0; i < input.Count; i++)
        {
            var s = input[i];
            for (var j = 0; j < s.Length; j++)
            {
                if (!int.TryParse(s[j].ToString(), out _)) continue;
                var engineNumber = new EngineNumber(i, j, 1);

                if (j+1 != s.Length && int.TryParse(s[j+1].ToString(), out _))
                {
                    engineNumber.NumberLength += 1;
                    j++;
                }

                output.Add(engineNumber);
            }
        }
        

        return output;
    }

    private static List<Tuple<int, int>> DecideSymbolIndices(List<string> input)
    {
        var symbolIndices = new List<Tuple<int, int>>();
        for (var i = 0; i < input.Count; i++)
        {
            var s = input[i];
            for (var j = 0; j < s.Length; j++)
            {
                if (!int.TryParse(s[j].ToString(), out _) || s[j] != '.')
                {
                    symbolIndices.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        return symbolIndices;
    }

    private static List<string> GetInput()
    {
        return InputFile.Split("\n").ToList();
    }

    # endregion
}

internal class EngineNumber
{
    public int RowNumber { get; set; }
    public int StartIndex { get; set; }
    public int NumberLength { get; set; }

    public EngineNumber(int rowNumber, int startIndex, int numberLength)
    {
        RowNumber = rowNumber;
        StartIndex = startIndex;
        NumberLength = numberLength;
    }
}