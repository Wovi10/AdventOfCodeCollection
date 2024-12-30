using UtilsCSharp;

namespace _2024.Models.Day13;

public class ArcadeMachine(string[] input, int arcadeCounter)
{
    public int Id { get; set; } = arcadeCounter;
    public ArcadeButton ButtonA { get; set; } = new(input[0]);
    public ArcadeButton ButtonB { get; set; } = new(input[1]);
    public ArcadePrize Prize { get; set; } = new(input[2]);

    public int LcmAX => Prize.X.Lcm(ButtonA.X);
    public int LcmBX => Prize.X.Lcm(ButtonB.X);
    public int LcmAY => Prize.Y.Lcm(ButtonA.Y);
    public int LcmBY => Prize.Y.Lcm(ButtonB.Y);
    public int GcdX => LcmAX.GcdWith(LcmBX);
    public int GcdY => LcmAY.GcdWith(LcmBY);
    public bool XIsPossible => Prize.X % GcdX == 0;
    public bool YIsPossible => Prize.Y % GcdY == 0;
    public bool CanWin => XIsPossible && YIsPossible;

    public int[,] Coefficients => new [,]
    {
        {ButtonA.X, ButtonA.Y},
        {ButtonB.X, ButtonB.Y}
    };

    public long[] Constants => new long[]{Prize.X, Prize.Y};
    public long[] Solution => SolveLinearEquation();

    public long[,] AugmentedMatrix => new long[,]
    {
        {ButtonA.X, ButtonB.X, Prize.X},
        {ButtonA.Y, ButtonB.Y, Prize.Y}
    };


    private long[] SolveLinearEquation()
    {
        var size = 2;
        var result = new long[size];

        var rowEchelonForm = GetRowEchelonForm();

        for (var i = 0; i < size; i++)
        {
            result[i] = rowEchelonForm[i, size];
        }

        return result;
    }

    private long[,] GetRowEchelonForm()
    {
        const int size = 2;
        var result = new long[3, 3];
        for (var i = 0; i < size; i++)
        {
            var diagonal = AugmentedMatrix[i, i];
            for (var j = 0; j < size+1; j++)
            {
                result[i, j] = AugmentedMatrix[i, j] / diagonal;
            }

            for (var j = 0; j < size; j++)
            {
                if (i == j)
                    continue;

                var factor = AugmentedMatrix[j, i];
                for (var k = 0; k < size + 1; k++)
                {
                    result[j, k] = AugmentedMatrix[j, k] - factor * AugmentedMatrix[i, k];
                }
            }
        }

        return result;
    }

    public override string ToString()
        => $"{Id} - A:{ButtonA} - B:{ButtonB} - Prize:{Prize}";

}