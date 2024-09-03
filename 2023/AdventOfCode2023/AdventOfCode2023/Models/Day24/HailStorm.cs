namespace AdventOfCode2023_1.Models.Day24;

public class HailStorm
{
    public HailStorm(List<string> input)
    {
        Hails = new List<Hail>();
        foreach (var line in input)
        {
            Hails.Add(new(line));
        }
    }

    public List<Hail> Hails { get; set; }
}