using AOC.Utils;
using UtilsCSharp;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day17;

public class Computer(string[] input)
{
    public List<int> Program { get; set; } = input[4].Split(':').Last().Trim().Split(Constants.Comma).Select(int.Parse).ToList();
    public int RegisterA { get; set; } = int.Parse(input[0].Split(':').Last().Trim());
    public int RegisterB { get; set; } = int.Parse(input[1].Split(':').Last().Trim());
    public int RegisterC { get; set; } = int.Parse(input[2].Split(':').Last().Trim());
    public int InstructionPointer { get; set; }
    public List<int> Output { get; set; } = new();

    public string GetOutputAsString()
    {
        Run();
        return string.Join(Constants.Comma, Output);
    }

    private void Run()
    {
        while (InstructionPointer < Program.Count - 1)
        {
            var opCode = Program[InstructionPointer];

            PerformOpcode(opCode, Program[InstructionPointer + 1]);
            if (opCode != 3 || RegisterA == 0)
                InstructionPointer += 2;
        }
    }

    private void PerformOpcode(int opcode, int operand)
    {
        switch (opcode)
        {
            case 0:
                Adv(operand);
                return;
            case 1:
                Bxl(operand);
                return;
            case 2:
                Bst(operand);
                return;
            case 3:
                Jnz(operand);
                return;
            case 4:
                Bxc(operand);
                return;
            case 5:
                Out(operand);
                return;
            case 6:
                Bdv(operand);
                return;
            case 7:
                Cdv(operand);
                return;
            default:
                throw new Exception("Unknown opcode");
        }
    }

    private void Adv(int operand)
        => RegisterA = (int)(RegisterA / Math.Pow(2, Combo(operand)));

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
        => Output.Add(Combo(operand) % 8);

    private void Bdv(int operand)
        => RegisterB = (int)(RegisterA / Math.Pow(2, Combo(operand)));

    private void Cdv(int operand)
        => RegisterC = (int)(RegisterA / Math.Pow(2, Combo(operand)));

    private int Combo(int operand)
        => operand switch
        {
            0 or 1 or 2 or 3 => operand,
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            _ => throw new Exception($"Invalid input: {operand}")
        };
}