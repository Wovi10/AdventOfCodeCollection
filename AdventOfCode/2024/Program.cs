using _2024;
using Constants = AOC.Utils.Constants;
using AOC.Utils.Enums;

// await new Day01().Run();
// await new Day02().Run();
// await new Day03().Run();
// await new Day04().Run();
// await new Day05().Run();
// await new Day06().Run();
// await new Day07().Run();
// await new Day08().Run();
// await new Day09().Run();
// await new Day10().Run();
// await new Day11().Run();
// await new Day12().Run();
// await new Day13().Run();
// await new Day14().Run();
await new Day15().Run();


// AddNewDay(16);
return;

void AddNewDay(int day)
{
    var dayString = day.ToString().PadLeft(2, '0');
    var dayFolder = Path.Combine(Constants.RootInputPath, $"Day{dayString}");
    Directory.CreateDirectory(dayFolder);

    var dayFile = Path.Combine(dayFolder, $"Day{dayString}.in");
    File.WriteAllText(dayFile, "");

    var mockDayFile = Path.Combine(dayFolder, $"MockDay{dayString}.in");
    File.WriteAllText(mockDayFile, "");
}