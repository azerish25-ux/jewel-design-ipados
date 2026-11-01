namespace CandidateGeometryContractChecks;
public enum ScreenshotAcknowledgementStatus {Waiting,Accepted,TimedOut}
/// <summary>Test-app screenshot handshake only; not a CAD/render or product timing tolerance.</summary>
public static class ViewportCaptureAcknowledgement
{
    public static readonly TimeSpan Deadline=TimeSpan.FromSeconds(180);
    public static ScreenshotAcknowledgementStatus Check(string expected,string? received,TimeSpan elapsed)
    {
        if(string.IsNullOrEmpty(expected))throw new ArgumentException("Expected capture token required.",nameof(expected));
        if(elapsed<TimeSpan.Zero)throw new ArgumentOutOfRangeException(nameof(elapsed));
        if(elapsed>=Deadline)return ScreenshotAcknowledgementStatus.TimedOut;
        return string.Equals(expected,received,StringComparison.Ordinal)?ScreenshotAcknowledgementStatus.Accepted:ScreenshotAcknowledgementStatus.Waiting;
    }
}
