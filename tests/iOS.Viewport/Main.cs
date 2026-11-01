using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Foundation;
using UIKit;
namespace ViewportPresentationCandidate;

[JsonSourceGenerationOptions(WriteIndented=true)]
[JsonSerializable(typeof(ViewportPresentationReport))]
internal partial class ViewportReportContext:JsonSerializerContext{}
public static class Program {public static void Main(string[] args)=>UIApplication.Main(args,null,typeof(AppDelegate));}
[Register("AppDelegate")]
public sealed class AppDelegate:UIApplicationDelegate
{
    public override UIWindow? Window{get;set;}
    private bool started;
    private readonly string directory=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    private readonly string token=Guid.NewGuid().ToString("N");
    public override bool FinishedLaunching(UIApplication application,NSDictionary options)
    {
        var controller=new UIViewController();controller.View!.BackgroundColor=UIColor.Black;
        Window=new UIWindow(UIScreen.MainScreen.Bounds){RootViewController=controller};Window.MakeKeyAndVisible();return true;
    }
    public override void OnActivated(UIApplication application)
    {
        if(started)return;started=true;
        BeginInvokeOnMainThread(async()=>await Run());
    }
    private async Task Capture(string stage)
    {
        var payload=token+"|"+stage;
        File.WriteAllText(Path.Combine(directory,"viewport-stage.pending"),payload);
        File.Move(Path.Combine(directory,"viewport-stage.pending"),Path.Combine(directory,"viewport-stage.txt"),overwrite:true);
        long started=Stopwatch.GetTimestamp();
        CaptureTiming("requested",payload,TimeSpan.Zero);
        while(true)
        {
            var ack=Path.Combine(directory,"viewport-ack.txt");var received=File.Exists(ack)?File.ReadAllText(ack):null;
            var elapsed=Stopwatch.GetElapsedTime(started);
            var status=CandidateGeometryContractChecks.ViewportCaptureAcknowledgement.Check(payload,received,elapsed);
            if(status==CandidateGeometryContractChecks.ScreenshotAcknowledgementStatus.Accepted){CaptureTiming("accepted",payload,elapsed);return;}
            if(status==CandidateGeometryContractChecks.ScreenshotAcknowledgementStatus.TimedOut)
            {CaptureTiming("timed-out",payload,elapsed);throw new TimeoutException("Screenshot acknowledgement missing for "+stage+" after "+elapsed.TotalSeconds.ToString("F1",CultureInfo.InvariantCulture)+"s");}
            await Task.Delay(20);
        }
    }
    private void CaptureTiming(string state,string payload,TimeSpan elapsed)=>File.AppendAllText(Path.Combine(directory,"viewport-capture-timing.txt"),state+"|"+payload+"|"+DateTime.UtcNow.ToString("O",CultureInfo.InvariantCulture)+"|"+elapsed.TotalMilliseconds.ToString("F0",CultureInfo.InvariantCulture)+Environment.NewLine);

    private async Task Run()
    {
        string resultPath=Path.Combine(directory,"viewport-result.txt");
        try
        {
            File.WriteAllText(resultPath+".family",UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Pad?"ipad":UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone?"iphone":"unsupported");
            File.WriteAllText(resultPath+".started",token);
            var gate=CandidateGeometryContractChecks.CadFrameGateAssertions.Run();
            var camera=CandidateGeometryContractChecks.LegacyCameraPresetAssertions.Run();
            var frustum=CandidateGeometryContractChecks.FrustumAssertions.Run();
            var matrices=CandidateGeometryContractChecks.CameraMatrixAssertions.Run();
            var frames=CandidateGeometryContractChecks.CameraFrameAssertions.Run();
            var transitions=CandidateGeometryContractChecks.ViewportProjectionAssertions.Run();
            var legacyScalar=CandidateGeometryContractChecks.Rhino5ScalarAssertions.Run();
            var legacyCamera=CandidateGeometryContractChecks.Rhino5CameraAssertions.Run();
            var capturePolicy=CandidateGeometryContractChecks.ViewportCaptureAcknowledgementAssertions.Run();
            var stringProvenance=CandidateGeometryContractChecks.UserStringProvenanceAssertions.Run();
            var stringCodec=CandidateGeometryContractChecks.LegacyUserStringListAssertions.Run();
            var stringBinding=CandidateGeometryContractChecks.UserStringArchiveBindingAssertions.Run();
            var userDataRuntime=CandidateGeometryContractChecks.Rhino5UserDataRuntimeAssertions.Run();
            var ownedViewData=CandidateGeometryContractChecks.Rhino5ViewDataAssertions.Run();
            var legacyCommit=CandidateGeometryContractChecks.Rhino5ViewportCommitAssertions.Run();
            var legacyCache=CandidateGeometryContractChecks.Rhino5ViewportCacheAssertions.Run();
            var legacyLocations=CandidateGeometryContractChecks.Rhino5CameraLocationAssertions.Run();
            var legacyRotation=CandidateGeometryContractChecks.Rhino5RotationAssertions.Run();
            var legacyViewport=CandidateGeometryContractChecks.Rhino5ViewportAssertions.Run();
            var offscreen=NativeGeometryCandidate.MetalViewportChecks.Run(directory);
            var parent=Window?.RootViewController?.View??throw new Exception("Visible root view missing");
            var report=await ViewportPresentationChecks.Run(parent,Capture);
            File.WriteAllText(Path.Combine(directory,"viewport-report.json"),JsonSerializer.Serialize(report,ViewportReportContext.Default.ViewportPresentationReport));
            string result="PASS visible Metal viewport:6 captured stages; source edit/orbit/empty/resize, bounded upload ownership and disposal; full product UI/PC/physical acceptance separate."+Environment.NewLine+gate+Environment.NewLine+camera+Environment.NewLine+frustum+Environment.NewLine+matrices+Environment.NewLine+frames+Environment.NewLine+transitions+Environment.NewLine+legacyScalar+Environment.NewLine+legacyCamera+Environment.NewLine+legacyViewport+Environment.NewLine+legacyRotation+Environment.NewLine+legacyLocations+Environment.NewLine+legacyCache+Environment.NewLine+legacyCommit+Environment.NewLine+ownedViewData+Environment.NewLine+userDataRuntime+Environment.NewLine+stringCodec+Environment.NewLine+stringBinding+Environment.NewLine+stringProvenance+Environment.NewLine+capturePolicy+Environment.NewLine+offscreen;
            File.WriteAllText(resultPath+".pending",result);File.Move(resultPath+".pending",resultPath,overwrite:true);Console.WriteLine(result);
        }
        catch(Exception error){File.WriteAllText(resultPath,"FAIL "+error);Console.WriteLine(error);}
    }
}
