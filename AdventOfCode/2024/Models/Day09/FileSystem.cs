namespace _2024.Models.Day09;

public class FileSystem
{
    public List<File> Files { get; set; } = new();
    public string FileString => GetFileString();

    public FileSystem(string input)
    {
        var isFile = true;
        var counter = 0;
        foreach (var valueAsInt in input.Select(value => int.Parse(value.ToString())))
        {
            if (valueAsInt == 0)
            {
                DoRoundUpStuff();
                continue;
            }
            var newFile = new File(counter, valueAsInt, isFile);
            Files.Add(newFile);

            DoRoundUpStuff();
        }

        return;

        void DoRoundUpStuff()
        {
            if (isFile)
                counter++;
            isFile = !isFile;
        }
    }

    private string GetFileString()
    {
        if (Files.Count == 0)
            return string.Empty;

        var result = string.Empty;
        var filesToFill = Files.Where(f => f.Id is not null).ToList();

        foreach (var file in Files)
        {
            if (file.FullyUsed)
                continue;

            if (file.Id is not null)
            {
                result += file.ToString();
                continue;
            }

            foreach (var _ in file.ToString())
            {
                var lastFile = filesToFill.Last(f => !f.FullyUsed);
                result += lastFile.Id.ToString();
                lastFile.RemoveBlock();
            }
        }

        return result;
    }
}