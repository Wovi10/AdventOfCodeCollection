namespace AdventOfCode2023_1.Models.Day08;

public class MathUtil
{
    public static int Gcd(int a, int b)
    {
        if (a == 0 && b == 0)
            return 1;
        return b == 0 
            ? a 
            : Gcd(b, a % b);
    }

    public static int Lcm(int a, int b) {
        return Math.Abs(a * b) / Gcd(a, b);
    }
}