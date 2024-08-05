using AdventOfCode2023_1.Models.Day03;
using UtilsCSharp.Utils;

namespace AdventOfCode2023_1;

public class Day03() : DayBase("03", "Gear Ratios")
{
    protected override Task<object> PartOne()
    {
        var result = GetSumPartNumbers();

        return Task.FromResult<object>(result);
    }

    protected override Task<object> PartTwo()
    {
        var result = GetSumGearRatios();

        return Task.FromResult<object>(result);
    }

    # region Part 1

    private static int GetSumPartNumbers()
        => GetPartNumbers().Sum();

    private static List<int> GetPartNumbers()
    {
        var symbolIndices = DecideSymbolIndices();
        var engineNumbers = DecideNumbers();
        var partNumbers = DecidePartNumbers(symbolIndices, engineNumbers);
        return partNumbers.Select(pn => pn.Number).ToList();
    }

    private static List<EngineSymbol> DecideSymbolIndices()
    {
        var symbolIndices = new List<EngineSymbol>();
        for (var i = 0; i < Input.Count; i++)
        {
            var line = Input[i].Trim();
            for (var j = 0; j < line.Length; j++)
            {
                var symbol = line[j];

                if (int.TryParse(line[j].ToString(), out _) || line[j] == Constants.Dot[0]) continue;

                var engineSymbol = new EngineSymbol(i, j, symbol);
                engineSymbol.IsGear = engineSymbol.Symbol == Constants.Asterisk;
                symbolIndices.Add(engineSymbol);
            }
        }

        return symbolIndices;
    }

    private static List<EngineNumber> DecideNumbers()
    {
        var output = new List<EngineNumber>();
        for (var i = 0; i < Input.Count; i++)
        {
            var line = Input[i].Trim();
            for (var j = 0; j < line.Length; j++)
            {
                var symbol = line[j];
                if (!int.TryParse(symbol.ToString(), out var number)) continue;
                var engineNumber = new EngineNumber(i, j, 1, number);

                var nextSymbol = line[j + 1];

                while (j + 1 < line.Length && int.TryParse(nextSymbol.ToString(), out var nextNumber))
                {
                    engineNumber.NumberLength += 1;
                    engineNumber.Number = Convert.ToInt32(engineNumber.Number + Constants.EmptyString + nextNumber);
                    j++;

                    if (j + 1 >= line.Length) continue;
                    nextSymbol = line[j + 1];
                }

                output.Add(engineNumber);
            }
        }

        return output;
    }

    private static List<EngineNumber> DecidePartNumbers(List<EngineSymbol> symbolIndices,
        List<EngineNumber> engineNumbers)
    {
        var partNumbers = new List<EngineNumber>();
        foreach (var engineNumber in engineNumbers)
        {
            var previousLineSymbols = symbolIndices.Where(es => es.RowIndex == engineNumber.RowIndex - 1).ToList();
            var ownLineSymbols = symbolIndices.Where(es => es.RowIndex == engineNumber.RowIndex).ToList();
            var nextLineSymbols = symbolIndices.Where(es => es.RowIndex == engineNumber.RowIndex + 1).ToList();

            var counter = -1;
            do
            {
                engineNumber.IsPartNumber =
                    previousLineSymbols.Any(es => es.ColumnIndex == engineNumber.ColumnIndex + counter) ||
                    ownLineSymbols.Any(es => es.ColumnIndex == engineNumber.ColumnIndex + counter) ||
                    nextLineSymbols.Any(es => es.ColumnIndex == engineNumber.ColumnIndex + counter);

                if (engineNumber.IsPartNumber)
                {
                    partNumbers.Add(engineNumber);
                    break;
                }

                counter++;
            } while (counter <= engineNumber.NumberLength);
        }

        return partNumbers;
    }

    # endregion

    # region Part 2

    private static int GetSumGearRatios()
        => GetGearRatios().Sum();

    private static List<int> GetGearRatios()
    {
        var symbolIndices = DecideSymbolIndices();
        var engineNumbers = DecideNumbers();
        var gears = DecideGears(symbolIndices, engineNumbers);

        return gears.Select(engineSymbol => engineSymbol.GearRatio).ToList();
    }

    private static List<EngineSymbol> DecideGears(List<EngineSymbol> engineSymbols, List<EngineNumber> engineNumbers)
    {
        var gears = new List<EngineSymbol>();
        foreach (var engineSymbol in engineSymbols)
        {
            if (engineSymbol.Symbol != Constants.Asterisk)
                continue;

            var previousLineNumbers = engineNumbers
                .Where(engineNumber => engineNumber.RowIndex == engineSymbol.RowIndex - 1).ToList();
            CheckLine(previousLineNumbers, engineSymbol);
            var ownLineNumbers = engineNumbers.Where(engineNumber => engineNumber.RowIndex == engineSymbol.RowIndex)
                .ToList();
            CheckLine(ownLineNumbers, engineSymbol);
            var nextLineNumbers = engineNumbers
                .Where(engineNumber => engineNumber.RowIndex == engineSymbol.RowIndex + 1).ToList();
            CheckLine(nextLineNumbers, engineSymbol);

            engineSymbol.IsGear = engineSymbol.AdjacentPartNumbers is { Count: >= 2 };

            if (!engineSymbol.IsGear) continue;
            engineSymbol.GearRatio = engineSymbol.GetGearRatio();
            gears.Add(engineSymbol);
        }

        return gears.Where(gear => gear.IsGear).ToList();
    }

    private static void CheckLine(List<EngineNumber> line, EngineSymbol engineSymbol)
    {
        foreach (var ownLineNumbers in line)
        {
            if (engineSymbol.ColumnIndex > ownLineNumbers.ColumnIndex + ownLineNumbers.NumberLength)
                continue;

            if (engineSymbol.ColumnIndex < ownLineNumbers.ColumnIndex - 1)
                break;

            var counter = -1;
            do
            {
                if (engineSymbol.ColumnIndex == ownLineNumbers.ColumnIndex + counter)
                {
                    if (engineSymbol.AdjacentPartNumbers != null)
                        engineSymbol.AdjacentPartNumbers.Add(ownLineNumbers);
                    else
                        engineSymbol.AdjacentPartNumbers = new List<EngineNumber> { ownLineNumbers };
                    break;
                }

                counter++;
            } while (counter <= ownLineNumbers.NumberLength);
        }
    }

    # endregion
}