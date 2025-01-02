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
    public long Solution => (long)(3 * Math.Round(ButtonPresses[0]) + Math.Round(ButtonPresses[1])); // Round so that .99999 will be 1 instead of 0
    public bool IsPossible => ButtonPresses.All(IsValid);

    private void SolveLinearEquation()
    {
        const int size = 2;
        var result = new decimal[size];

        var augmentedMatrix = GetAugmentedMatrix();
        var rowEchelonForm = GetRowEchelonForm(augmentedMatrix);

        for (var i = 0; i < size; i++)
            result[i] = rowEchelonForm[i, size];

        ButtonPresses = result;
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

    private static decimal[,] GetRowEchelonForm(decimal[,] augmentedMatrix, int size = 2)
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

    private static bool IsValid(decimal value)
    {
        if (value < 0)
            return false;

        const decimal tolerance = (decimal)1e-5;
        var roundedValue = Math.Round(value);

        return Math.Abs(value - roundedValue) <= tolerance;
    }
}