namespace AdventOfCode2023_1.Shared;

public static class MathUtil
{
    /// <summary>
    /// Greatest Common Factor
    /// The biggest number that can divide both a and b
    /// </summary>
    public static long Gcf(long a, long b)
    {
        if (a == 0 && b == 0)
            return 1;
        return b == 0
            ? a 
            : Gcf(b, a % b);
    }

    /// <summary>
    /// Least Common Multiple
    /// The smallest number that both a and b can divide
    /// </summary>
    public static long Lcm(long a, long b) {
        return a*b / Gcf(a, b);
    }
    
    public static int GetHighest(int a, int b) => a > b ? a : b;
    public static int GetLowest(int a, int b) => a < b ? a : b;
    
    public static long GetHighest(long a, long b) => a > b ? a : b;
    public static long GetLowest(long a, long b) => a < b ? a : b;
}