using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day17;

public class Computer(string[] input)
{
    internal int[] Program { get; } = input[4].Split(':').Last().Trim().Split(Constants.Comma).Select(int.Parse).ToArray();
    public long RegisterA { get; internal set; } = long.Parse(input[0].Split(':').Last().Trim());
    private long RegisterB { get; set; } = long.Parse(input[1].Split(':').Last().Trim());
    private long RegisterC { get; set; } = long.Parse(input[2].Split(':').Last().Trim());
    private int InstructionPointer { get; set; }
    internal List<int> Output { get; } = new();

    public string GetOutputAsString()
    {
        Run();
        return string.Join(Constants.Comma, Output);
    }

    public void ResetComputerPart2(long registerAValue)
    {
        RegisterA = registerAValue;
        InstructionPointer = 0;
        RegisterB = 0;
        RegisterC = 0;
        Output.Clear();
    }

    public void Run()
    {
        while (InstructionPointer < Program.Length - 1)
        {
            var opCode = Program[InstructionPointer];
            var operand = Program[InstructionPointer + 1];

            GetOperation(opCode).Invoke(operand);
            if (opCode != 3 || RegisterA == 0)
                InstructionPointer += 2;

            if (!Variables.RunningPartOne && opCode == 5 && Output[^1] != Program[Output.Count-1])
                return;
        }
    }

    private Action<int> GetOperation(int opcode)
        => opcode switch
        {
            0 => Adv,
            1 => Bxl,
            2 => Bst,
            3 => Jnz,
            4 => Bxc,
            5 => Out,
            6 => Bdv,
            7 => Cdv,
            _ => throw new Exception("Unknown opcode")
        };

    private void Adv(int operand)
        => RegisterA = RegisterA >> Combo(operand); // Use bit shift instead of division and casting

    private void Bxl(int operand)
        => RegisterB ^= operand;

    private void Bst(int operand)
        => RegisterB = Combo(operand) % 8;

    private void Jnz(int operand)
    {
        if (RegisterA == 0)
            return;

        InstructionPointer = operand;
    }

    private void Bxc(int operand)
        => RegisterB ^= RegisterC;

    private void Out(int operand)
        => Output.Add((int)(Combo(operand) % 8));

    private void Bdv(int operand)
        => RegisterB = RegisterA >> Combo(operand); // Use bit shift instead of division and casting

    private void Cdv(int operand)
        => RegisterC = RegisterA >> Combo(operand); // Use bit shift instead of division and casting

    private int Combo(int operand) // Change return type to int since operands are small
        => operand switch
        {
            0 or 1 or 2 or 3 => operand,
            4 => (int)(RegisterA & 0x7FFFFFFF), // Safely cast to int, keeping only lower bits
            5 => (int)(RegisterB & 0x7FFFFFFF), // Safely cast to int, keeping only lower bits
            6 => (int)(RegisterC & 0x7FFFFFFF), // Safely cast to int, keeping only lower bits
            _ => throw new Exception($"Invalid input: {operand}")
        };
}