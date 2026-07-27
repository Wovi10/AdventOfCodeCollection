namespace AOC.Utils;

public static class Variables
{
    public static bool RunningPartOne { get; set; } // Dynamically set in DayBase.cs

    // Lets a scoped run (e.g. DayBase.RunPartForResult) force mock/real input for every
    // GetInput call during that run, even the ones a Day class makes directly instead
    // of through DayBase. Null means "no override, fall back to Constants.IsRealExercise".
    public static bool? UseMockInput { get; set; }
}