using AOC.Utils;
using Constants = UtilsCSharp.Utils.Constants;

namespace _2024.Models.Day17;

public class Computer(string[] input)
{
    internal List<int> Program { get; } = input[4].Split(':').Last().Trim().Split(Constants.Comma).Select(int.Parse).ToList();
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
        while (InstructionPointer < Program.Count - 1)
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
        => Output.Add((int)(Combo(operand) % 8));

    private void Bdv(int operand)
        => RegisterB = (int)(RegisterA / Math.Pow(2, Combo(operand)));

    private void Cdv(int operand)
        => RegisterC = (int)(RegisterA / Math.Pow(2, Combo(operand)));

    private long Combo(int operand)
        => operand switch
        {
            0 or 1 or 2 or 3 => operand,
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            _ => throw new Exception($"Invalid input: {operand}")
        };
}