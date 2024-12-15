namespace _2024.Models.Day08;

public class Location
{
    public int? Id { get; init; }
    public Coordinate Coordinate { get; set; }
    public bool IsAntenna { get; set; }
    public char? Frequency { get; set; }
}