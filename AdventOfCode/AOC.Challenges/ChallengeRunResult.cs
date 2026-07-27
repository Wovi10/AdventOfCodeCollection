namespace AOC.Challenges;

public record ChallengeRunResult(bool Success, string Message)
{
    public static ChallengeRunResult Ok(string message) => new(true, message);

    public static ChallengeRunResult Failure(string message) => new(false, message);
}
