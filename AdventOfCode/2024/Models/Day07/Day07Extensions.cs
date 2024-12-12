namespace _2024.Models.Day07;

public static class Day07Extensions
{
    public static IEnumerable<Equation> ToEquations(this IEnumerable<string> input)
        => input.Select(line => new Equation(line));

    public static IEnumerable<long> SolveEquations(this IEnumerable<Equation> equations)
        => from equation in equations where equation.Evaluate() select equation.Result;
}