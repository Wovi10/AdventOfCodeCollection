namespace _2024.Models.Day17;

public static class Day17Extensions
{
    public static Computer InitializeComputer(this IEnumerable<string> input)
        => new(input.ToArray());

    // Brute-force approach to find the correct value of Register A (takes too long)
    public static Computer TryDifferentValuesOfA(this Computer computer)
    {
        var counter = 164516454365620;
        var lastReport = DateTime.Now;

        do
        {
            computer.ResetComputerPart2(counter);
            computer.Run();

            if (computer.Output.SequenceEqual(computer.Program))
            {
                computer.RegisterA = counter;
                return computer;
            }

            counter++;

            if ((DateTime.Now - lastReport).TotalSeconds >= 10)
            {
                Console.WriteLine($"Tested up to: {counter}");
                lastReport = DateTime.Now;
            }

        } while (counter < long.MaxValue);

        Console.WriteLine("Counter overflowed without finding a solution.");

        return computer;
    }

    // MockProgram: 0,3,5,4,3,0
    // Program: 2,4,1,1,7,5,1,5,0,3,4,4,5,5,3,0
    public static Computer TryFindMathematics(this Computer computer)
    {
        var target = computer.Program;
        var candidates = new Queue<long>();
        candidates.Enqueue(0);

        Console.WriteLine($"Target program: [{string.Join(",", target)}]");
        Console.WriteLine("Working backwards from the last digit...");

        // Work backwards through each position in the target output
        for (var pos = target.Length - 1; pos >= 0; pos--)
        {
            var nextCandidates = new Queue<long>();

            while (candidates.Count > 0)
            {
                var baseA = candidates.Dequeue();

                // Try all possible 3-bit values (0-7) for this position
                for (var bits = 0; bits < 8; bits++)
                {
                    var testA = (baseA << 3) | bits;

                    // Simulate one iteration of the program with this A value
                    var outputDigit = SimulateOneIteration(testA);

                    // Check if this produces the correct digit for this position
                    if (outputDigit == target[pos])
                    {
                        nextCandidates.Enqueue(testA);
                    }
                }
            }

            candidates = nextCandidates;

            if (candidates.Count == 0)
            {
                throw new Exception($"No solutions found at position {pos}");
            }

            Console.WriteLine($"Position {pos} (digit {target[pos]}): Found {candidates.Count} candidates");
        }

        // Find the minimum valid solution
        var solution = candidates.Min();
        Console.WriteLine($"Found minimum solution: {solution}");

        // Verify the solution by running the full program
        computer.ResetComputerPart2(solution);
        computer.Run();

        if (computer.Output.SequenceEqual(computer.Program))
        {
            Console.WriteLine($"✓ Solution verified: {solution}");
            computer.RegisterA = solution;
        }
        else
        {
            Console.WriteLine($"✗ Solution verification failed!");
        }

        Console.WriteLine($"Expected: [{string.Join(",", computer.Program)}]");
        Console.WriteLine($"Got:      [{string.Join(",", computer.Output)}]");

        return computer;
    }

    // Simulate one iteration of your specific program: 2,4,1,1,7,5,1,5,0,3,4,4,5,5,3,0
    private static int SimulateOneIteration(long a)
    {
        // Your program breakdown:
        // 2,4 -> bst 4: B = A % 8
        // 1,1 -> bxl 1: B = B XOR 1
        // 7,5 -> cdv 5: C = A / 2^B
        // 1,5 -> bxl 5: B = B XOR 5
        // 0,3 -> adv 3: A = A / 8 (this happens but we don't care for single iteration)
        // 4,4 -> bxc 4: B = B XOR C
        // 5,5 -> out 5: output B % 8
        // 3,0 -> jnz 0: jump to start if A != 0

        var b = a % 8;           // B = A % 8
        b ^= 1;                  // B = B XOR 1
        var c = a / (1L << (int)b);  // C = A / 2^B (using bit shift for division by power of 2)
        b ^= 5;                  // B = B XOR 5
        b ^= c;                  // B = B XOR C

        return (int)(b % 8);     // output B % 8
    }
}