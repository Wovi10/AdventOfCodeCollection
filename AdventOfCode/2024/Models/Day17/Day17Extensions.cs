namespace _2024.Models.Day17;

public static class Day17Extensions
{
    public static Computer InitializeComputer(this IEnumerable<string> input)
        => new(input.ToArray());

    public static Computer TryDifferentValuesOfA(this Computer computer)
    {
        var counter = 1299389035120L;
        do
        {
            if (counter < 0L)
            {
                Console.WriteLine("What the actual fuck?");
            }
            computer.ResetComputerPart2(counter);
            computer.Run();

            if (computer.Output.SequenceEqual(computer.Program))
            {
                computer.RegisterA = counter;
                return computer;
            }

            counter++;
        } while (true);
    }
}