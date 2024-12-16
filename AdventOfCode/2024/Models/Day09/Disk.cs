using AOC.Utils;


namespace _2024.Models.Day09;

public class Disk
{
    private List<File> Files { get; }
    private int?[] Blocks { get; }
    private List<Range<int>> Free { get; }

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
            var isFile = i % 2 == 0;

            if (isFile)
            {
                file++;
                files.Add(new File(Min: head, Length: count));
            }
            else if (count != 0)
                free.Add(new Range<int>(min: head, max: head + count - 1));

            for (var j = 0; j < count; j++)
            {
                blocks[head++] = isFile
                    ? file
                    : null;
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

    public void DefragmentFiles()
    {
        for (var fileId = Files.Count - 1; fileId >= 0; fileId--)
        {
            var currentFile = Files[fileId];

            for (var j = 0; j < Free.Count; j++)
            {
                var currentFree = Free[j];
                if (currentFree.Min >= currentFile.Min) // First free is after the file
                    break;

                if (currentFree.Length < currentFile.Length) // Not enough space
                    continue;

                for (var k = 0; k < currentFile.Length; k++)
                {
                    Blocks[currentFree.Min + k] = fileId; // Move file
                    Blocks[currentFile.Min + k] = null; // Clear old position
                }

                Free.RemoveAt(j);

                if (currentFree.Length > currentFile.Length)
                    Free.Insert(j, new Range<int>(currentFree.Min + currentFile.Length, currentFree.Max));

                break; // File has been moved
            }
        }
    }

    public long CheckSum()
        => Blocks.Select((file, i) => i * (file ?? 0L)).Sum();
}