using UtilsCSharp;

namespace _2024.Models.Day13;

public class ArcadeMachine
{
    public ArcadeMachine(string[] input)
    {
        ButtonA = new ArcadeButton(input[0]);
        ButtonB = new ArcadeButton(input[1]);
        Prize = new ArcadePrize(input[2]);

        SolveLinearEquation();
    }

    private ArcadeButton ButtonA { get; }
    private ArcadeButton ButtonB { get; }
    private ArcadePrize Prize { get; }

    private decimal[] ButtonPresses { get; set; } = new decimal[2];

    private long[,] Coefficients => new [,]
    {
        {ButtonA.X, ButtonB.X},
        {ButtonA.Y, ButtonB.Y}
    };
    private long[] Constants => new[]{Prize.X, Prize.Y};
    public long Solution => (long)(3 * Math.Floor(ButtonPresses[0]) + Math.Floor(ButtonPresses[1]));
    public bool IsPossible => ButtonPresses.All(IsValid);

    private void SolveLinearEquation()
    {
        const int size = 2;
        var result = new decimal[size];

        var augmentedMatrix = GetAugmentedMatrix();
        var rowEchelonForm = GetRowEchelonForm(augmentedMatrix);
        PrintMatrix(rowEchelonForm);

        for (var i = 0; i < size; i++)
            result[i] = rowEchelonForm[i, size];

        ButtonPresses = result;
    }

    private void PrintMatrix(decimal[,] matrix)
    {
        var firstRowFirstColumn = matrix[0, 0]/1==0 ? 0 : matrix[0, 0];
        var firstRowSecondColumn = matrix[0, 1]/1==0 ? 0 : matrix[0, 1];
        var firstRowThirdColumn = matrix[0, 2]/1==0 ? 0 : matrix[0, 2];
        var secondRowFirstColumn = matrix[1, 0]/1==0 ? 0 : matrix[1, 0];
        var secondRowSecondColumn = matrix[1, 1]/1==0 ? 0 : matrix[1, 1];
        var secondRowThirdColumn = matrix[1, 2]/1==0 ? 0 : matrix[1, 2];
        Console.WriteLine($"{firstRowFirstColumn}\t{firstRowSecondColumn}\t{firstRowThirdColumn}");
        Console.WriteLine($"{secondRowFirstColumn}\t{secondRowSecondColumn}\t{secondRowThirdColumn}");
        Console.WriteLine();
    }

    private decimal[,] GetAugmentedMatrix(int size = 2)
    {
        var augmentedMatrix = new decimal[size, size + 1];
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
                augmentedMatrix[i, j] = Coefficients[i, j];
            augmentedMatrix[i, size] = Constants[i];
        }

        return augmentedMatrix;
    }

    private decimal[,] GetRowEchelonForm(decimal[,] augmentedMatrix, int size = 2)
    {
        for (var i = 0; i < size; i++)
        {
            var diagonal = augmentedMatrix[i, i];
            for (var j = 0; j < size+1; j++)
                augmentedMatrix[i, j] /= diagonal;

            for (var k = 0; k < size; k++)
            {
                if (k == i)
                    continue;

                var factor = augmentedMatrix[k, i];
                for (var j = 0; j < size + 1; j++)
                    augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
            }
        }

        return augmentedMatrix;
    }

    private bool IsValid(decimal value)
    {
        if (value < 0)
            return false;

        const decimal tolerance = (decimal)1e-5;
        var roundedValue = Math.Round(value);
        return Math.Abs(value - roundedValue) <= tolerance;
    }

    public override string ToString()
        => $"{ButtonPresses[0]} {ButtonPresses[1]}";
        // => $"{Id} - A:{ButtonA} - B:{ButtonB} - Prize:{Prize}";

}