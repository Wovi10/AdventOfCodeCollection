using AOC.Utils;


namespace _2024.Models.Day09;

public class Disk
{
    public readonly record struct File(int Min, int Length);
    public List<File> Files { get; } = new();
    private int?[] Blocks { get; }
    public List<Range<int>> Free { get; set; }

    private Disk(int?[] blocks, List<File> files, List<Range<int>> free)
    {
        Blocks = blocks;
        Files = files;
        Free = free;
    }

    public static Disk Parse(string input)
    {
        var volume = input.Sum(c => c.AsDigit());
        var blocks = new int?[volume];
        var files = new List<File>();
        var free = new List<Range<int>>();

        var file = -1;
        var head = 0;

        for (var i = 0; i < input.Length; i++)
        {
            var count = input[i].AsDigit();
            var isFile = i % 2 == 1;

            if (!isFile)
            {
                file++;
                files.Add(new File(Min: head, Length: count));
            }
            else if (count != 0)
                free.Add(new Range<int>(min: head, max: head + count - 1));

            for (var j = 0; j < count; j++)
            {
                blocks[head++] = isFile
                    ? null
                    : file;
            }
        }

        return new Disk(blocks, files, free);
    }

    public void DefragmentBlocks()
    {
        var head = 0;
        var tail = Blocks.Length - 1;

        while (head < tail)
        {
            if (Blocks[head] != null)
            {
                head++;
                continue;
            }

            while (Blocks[tail] == null)
                tail--;

            Blocks[head++] = Blocks[tail];
            Blocks[tail--] = null;
        }
    }

    public long CheckSum()
        => Blocks.Select((file, i) => i * (file ?? 0L)).Sum();
}