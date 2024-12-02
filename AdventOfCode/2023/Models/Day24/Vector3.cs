namespace _2023.Models.Day24;

public record Vector3(double X, double Y, double Z)
{
    public static implicit operator (double, double, double)(Vector3 vector3) => (vector3.X, vector3.Y, vector3.Z);
    public static implicit operator Vector3((double, double, double) vector) => new(vector.Item1, vector.Item2, vector.Item3);
    public double Sum => X + Y + Z;
}