﻿namespace AOC.Utils;

public record Answer(int Day, int Part, bool IsReal, object Result);

public static class Answers
{
    private const bool Mock = false;
    private const bool Real = true;
    public const string NotYetFound = "NOT YET FOUND";
    private const string NotApplicable = "N/A";

    private static readonly List<Answer> AnswersList2023 = new()
    {
        new(1, 1, Mock, 142),
        new(1, 1, Real, 55971),
        new(1, 2, Mock, 281),
        new(1, 2, Real, 54719),
        new(2, 1, Mock, 8),
        new(2, 1, Real, 2447),
        new(2, 2, Mock, 2286),
        new(2, 2, Real, 56322),
        new(3, 1, Mock, 4361),
        new(3, 1, Real, 553079),
        new(3, 2, Mock, 467835),
        new(3, 2, Real, 84363105),
        new(4, 1, Mock, 13),
        new(4, 1, Real, 24848),
        new(4, 2, Mock, 30),
        new(4, 2, Real, 7258152),
        new(5, 1, Mock, 35),
        new(5, 1, Real, 322500873),
        new(5, 2, Mock, 46),
        new(5, 2, Real, 108956227),
        new(6, 1, Mock, 288),
        new(6, 1, Real, 840336),
        new(6, 2, Mock, 71503),
        new(6, 2, Real, 41382569),
        new(7, 1, Mock, 6440),
        new(7, 1, Real, 253866470),
        new(7, 2, Mock, 5905),
        new(7, 2, Real, 254494947),
        new(8, 1, Mock, 2),
        new(8, 1, Real, 11309),
        new(8, 2, Mock, 6),
        new(8, 2, Real, 13740108158591),
        new(9, 1, Mock, 114),
        new(9, 1, Real, 1972648895),
        new(9, 2, Mock, 2),
        new(9, 2, Real, 919),
        new(10, 1, Mock, 4),
        new(10, 1, Real, 6599),
        new(10, 2, Mock, 10),
        new(10, 2, Real, 477),
        new(11, 1, Mock, 374),
        new(11, 1, Real, 9647174),
        new(11, 2, Mock, 8410),
        new(11, 2, Real, 377318892554),
        new(12, 1, Mock, 21),
        new(12, 1, Real, 7350),
        new(12, 2, Mock, 525152),
        new(12, 2, Real, 200097286528151),
        new(13, 1, Mock, 405),
        new(13, 1, Real, 28651),
        new(13, 2, Mock, 400),
        new(13, 2, Real, 25450),
        new(14, 1, Mock, 136),
        new(14, 1, Real, 113078),
        new(14, 2, Mock, 64),
        new(14, 2, Real, 94255),
        new(15, 1, Mock, 1320),
        new(15, 1, Real, 510013),
        new(15, 2, Mock, 145),
        new(15, 2, Real, 268497),
        new(16, 1, Mock, 46),
        new(16, 1, Real, 7517),
        new(16, 2, Mock, 51),
        new(16, 2, Real, 7741),
        new(17, 1, Mock, 102),
        new(17, 1, Real, 1039),
        new(17, 2, Mock, 71),
        new(17, 2, Real, 1201),
        new(18, 1, Mock, 62),
        new(18, 1, Real, 40131),
        new(18, 2, Mock, 952408144115),
        new(18, 2, Real, 104454050898331),
        new(19, 1, Mock, 19114),
        new(19, 1, Real, 575412),
        new(19, 2, Mock, 167409079868000),
        new(19, 2, Real, 126107942006821),
        new(20, 1, Mock, 32000000),
        new(20, 1, Real, 788848550),
        new(20, 2, Mock, NotApplicable),
        new(20, 2, Real, 228300182686739),
        new(21, 1, Mock, 16),
        new(21, 1, Real, 3764),
        new(21, 2, Mock, 16733044),
        new(21, 2, Real, 622926941971282),
        new(22, 1, Mock, 5),
        new(22, 1, Real, 475),
        new(22, 2, Mock, 7),
        new(22, 2, Real, 79144),
        new(23, 1, Mock, 94),
        new(23, 1, Real, 1998),
        new(23, 2, Mock, 154),
        new(23, 2, Real, 6434),
        new(24, 1, Mock, 2),
        new(24, 1, Real, 12740),
        new(24, 2, Mock, 47),
        new(24, 2, Real, 741991571910536),
        new(25, 1, Mock, 54),
        new(25, 1, Real, 568214),
        new(25, 2, Mock, NotApplicable),
        new(25, 2, Real, NotApplicable)
    };

    private static readonly List<Answer> AnswersList2024 = new()
    {
        new(1, 1, Mock, 11),
        new(1, 1, Real, 2344935),
        new(1, 2, Mock, 31),
    };

    public static object GetExpectedAnswer(string day)
    {
        var listToUse = Constants.RunningYear == 2023 ? AnswersList2023 : AnswersList2024;

        var partInt = Variables.RunningPartOne ? 1 : 2;
        var answer = listToUse.FirstOrDefault(a =>
            a.Day == int.Parse(day) && a.Part == partInt && a.IsReal == Constants.IsRealExercise);

        return answer?.Result ?? NotYetFound;
    }
}