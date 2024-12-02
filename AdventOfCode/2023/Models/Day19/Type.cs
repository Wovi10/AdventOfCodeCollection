namespace _2023.Models.Day19;

public enum Type
{
    X,
    M,
    A,
    S,
    Unknown
}

public static class TypeExtensions
{
    public static Type ToType(this char c)
    {
        return c switch
        {
            'x' => Type.X,
            'm' => Type.M,
            'a' => Type.A,
            's' => Type.S,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}