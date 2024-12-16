namespace _2024.Models.Day10;

public static class Day10Extensions
{
    public static TopoMap ToMap(this List<string> input)
        => new(input);

    public static List<Trail> FindAllTrails(this TopoMap map)
    {
        return map.Trails;
    }
}