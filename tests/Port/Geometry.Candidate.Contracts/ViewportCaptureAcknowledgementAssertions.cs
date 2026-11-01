namespace CandidateGeometryContractChecks;
public static class ViewportCaptureAcknowledgementAssertions
{
    public static string Run()
    {
        int checks=0;void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
        const string expected="0123456789abcdef0123456789abcdef|orbit";
        Check(ViewportCaptureAcknowledgement.Check(expected,null,TimeSpan.FromSeconds(65))==ScreenshotAcknowledgementStatus.Waiting,"Observed slow simulator capture must remain pending at65 seconds");
        Check(ViewportCaptureAcknowledgement.Check(expected,expected,TimeSpan.FromSeconds(65))==ScreenshotAcknowledgementStatus.Accepted,"Delayed matching acknowledgement must complete");
        foreach(var stale in new[]{"other|orbit","0123456789abcdef0123456789abcdef|original","0123456789ABCDEF0123456789ABCDEF|orbit",string.Empty})
            Check(ViewportCaptureAcknowledgement.Check(expected,stale,TimeSpan.FromSeconds(65))==ScreenshotAcknowledgementStatus.Waiting,"Stale/wrong phase/case token must not pass");
        Check(ViewportCaptureAcknowledgement.Check(expected,null,TimeSpan.FromSeconds(179.999))==ScreenshotAcknowledgementStatus.Waiting,"Bounded deadline just before180s");
        Check(ViewportCaptureAcknowledgement.Check(expected,null,TimeSpan.FromSeconds(180))==ScreenshotAcknowledgementStatus.TimedOut,"Exact deadline expires");
        Check(ViewportCaptureAcknowledgement.Check(expected,expected,TimeSpan.FromSeconds(181))==ScreenshotAcknowledgementStatus.TimedOut,"Late acknowledgement cannot revive expired request");
        Check(ViewportCaptureAcknowledgement.Check(expected,expected,TimeSpan.Zero)==ScreenshotAcknowledgementStatus.Accepted,"Immediate valid acknowledgement");
        bool invalid=false;try{ViewportCaptureAcknowledgement.Check(expected,null,TimeSpan.FromSeconds(-1));}catch(ArgumentOutOfRangeException){invalid=true;}Check(invalid,"Negative elapsed clock rejected");
        return "PASS viewport screenshot acknowledgement:"+checks+" timing/token boundaries; actual simulator screenshot and pixel gates separate.";
    }
}
