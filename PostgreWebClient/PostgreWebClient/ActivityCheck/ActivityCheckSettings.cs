namespace PostgreWebClient.ActivityCheck;

public class ActivityCheckSettings
{
    public TimeSpan TimeBeforeClose { get; init; }
    public TimeSpan TimeBeforeChecks { get; init; }
}