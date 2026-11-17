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
[JsonSerializable(typeof(WallpaperPresentationReport))]
[JsonSerializable(typeof(TracePresentationReport))]
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
            var anisotropyPolicy=CandidateGeometryContractChecks.TextureAnisotropyAssertions.Run();
            var anisotropyGpu=await CandidateAppleImageChecks.TextureAnisotropyGpuChecks.Run(directory);
            var gdiInteger=CandidateGeometryContractChecks.GdiIntegerStretchAssertions.Run();
            var gdiHalftone=CandidateGeometryContractChecks.GdiHalftoneStretchAssertions.Run();
            var gdiEnlargement=CandidateGeometryContractChecks.GdiEnlargementAssertions.Run();
            var sourceCache=CandidateGeometryContractChecks.ContextImageCacheAssertions.Run();
            var unicodeNlsLower=CandidateGeometryContractChecks.UnicodeNlsLowerAssertions.Run();
            var nlsComparisonInputs=CandidateGeometryContractChecks.NlsComparisonInputAssertions.Run();
            var sourceEntryNameCopy=CandidateGeometryContractChecks.CacheEntryNameAssertions.Run();
            var nativeCacheOrdering=CandidateGeometryContractChecks.CacheNativeOrderingAssertions.Run();
            var nativeCrtSort=CandidateGeometryContractChecks.CrtSortAssertions.Run();
            var mutableStringRuntime=CandidateGeometryContractChecks.StringRuntimeAssertions.Run();
            var ansiNlsProfile=CandidateGeometryContractChecks.AnsiNlsProfileAssertions.Run();
            var crt100Profiles=CandidateGeometryContractChecks.Crt100ProfileAssertions.Run();
            var nlsFilenamePolicy=CandidateGeometryContractChecks.NlsFilenamePolicyAssertions.Run();
            var nlsDirectStrings=CandidateGeometryContractChecks.NlsDirectComparisonAssertions.Run();
            var nlsDirectExhaustive=CandidateGeometryContractChecks.NlsDirectExhaustiveAssertions.RunExhaustive();
            var nlsDirectJamo=CandidateGeometryContractChecks.NlsDirectExhaustiveAssertions.RunJamo();
            var nlsDirectStress=CandidateGeometryContractChecks.NlsDirectExhaustiveAssertions.RunStress();
            var liveStringRuntimeGpu=await CandidateAppleImageChecks.LiveRuntimeSourceCacheGpuChecks.Run(directory);
            var sourceCacheGpu=await CandidateAppleImageChecks.SourceCacheGpuChecks.Run(directory);
            var preparedWallpaper=CandidateGeometryContractChecks.PreparedWallpaperAssertions.Run();
            var preparedWallpaperGpu=await CandidateAppleImageChecks.PreparedWallpaperGpuChecks.Run(directory);
            var composedTexture=CandidateGeometryContractChecks.ComposedTextureAssertions.Run();
            var composedTextureGpu=await CandidateAppleImageChecks.ComposedTextureGpuChecks.Run(directory);
            var tracePlane=CandidateGeometryContractChecks.TracePlaneAssertions.Run();
            var texturePrep=CandidateGeometryContractChecks.TexturePreparationAssertions.Run();
            var texturePrepGpu=await CandidateAppleImageChecks.TexturePreparationGpuChecks.Run(directory);
            var wallpaperPlacement=CandidateGeometryContractChecks.WallpaperPlacementAssertions.Run();
            var pngSamples=CandidateGeometryContractChecks.PngStraightSampleAssertions.Run();
            var straightProvider=CandidateGeometryContractChecks.StraightImageProviderAssertions.Run();
            var straightImage=await CandidateAppleImageChecks.StraightImageDecodeChecks.Run(directory);
            var encodedImages=CandidateGeometryContractChecks.LocalImageAssetAssertions.Run();
            var appleImages=CandidateAppleImageChecks.LocalImageDecodeChecks.Run();
            var imageGpu=await CandidateAppleImageChecks.MetalImageQuadChecks.Run(directory);
            var wallpaperRaster=CandidateGeometryContractChecks.Rhino5WallpaperRasterAssertions.Run();
            var setViewPreflight=CandidateGeometryContractChecks.Rhino5SetViewPreflightAssertions.Run();
            var popMerge=CandidateGeometryContractChecks.Rhino5PopMergeAssertions.Run();
            var historyPush=CandidateGeometryContractChecks.Rhino5ViewHistoryAssertions.Run();
            var runtimeView=CandidateGeometryContractChecks.Rhino5RuntimeViewAssertions.Run();
            var viewScale=CandidateGeometryContractChecks.Rhino5ViewScaleAssertions.Run();
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
            var wallpaperReport=await WallpaperPresentationChecks.Run(parent,Capture);
            var traceReport=await TracePresentationChecks.Run(parent,Capture);
            File.WriteAllText(Path.Combine(directory,"trace-report.json"),JsonSerializer.Serialize(traceReport,ViewportReportContext.Default.TracePresentationReport));
            File.WriteAllText(Path.Combine(directory,"wallpaper-report.json"),JsonSerializer.Serialize(wallpaperReport,ViewportReportContext.Default.WallpaperPresentationReport));
            File.WriteAllText(Path.Combine(directory,"viewport-report.json"),JsonSerializer.Serialize(report,ViewportReportContext.Default.ViewportPresentationReport));
            string result="PASS visible Metal viewport:6 captured stages; source edit/orbit/empty/resize, bounded upload ownership and disposal; full product UI/PC/physical acceptance separate."+Environment.NewLine+gate+Environment.NewLine+camera+Environment.NewLine+frustum+Environment.NewLine+matrices+Environment.NewLine+frames+Environment.NewLine+transitions+Environment.NewLine+legacyScalar+Environment.NewLine+legacyCamera+Environment.NewLine+legacyViewport+Environment.NewLine+legacyRotation+Environment.NewLine+legacyLocations+Environment.NewLine+legacyCache+Environment.NewLine+legacyCommit+Environment.NewLine+ownedViewData+Environment.NewLine+userDataRuntime+Environment.NewLine+stringCodec+Environment.NewLine+stringBinding+Environment.NewLine+stringProvenance+Environment.NewLine+runtimeView+Environment.NewLine+viewScale+Environment.NewLine+historyPush+Environment.NewLine+popMerge+Environment.NewLine+setViewPreflight+Environment.NewLine+wallpaperRaster+Environment.NewLine+encodedImages+Environment.NewLine+straightProvider+Environment.NewLine+pngSamples+Environment.NewLine+wallpaperPlacement+Environment.NewLine+texturePrep+Environment.NewLine+anisotropyPolicy+Environment.NewLine+anisotropyGpu+Environment.NewLine+gdiInteger+Environment.NewLine+gdiHalftone+Environment.NewLine+gdiEnlargement+Environment.NewLine+sourceCache+Environment.NewLine+unicodeNlsLower+Environment.NewLine+nlsComparisonInputs+Environment.NewLine+sourceEntryNameCopy+Environment.NewLine+nativeCacheOrdering+Environment.NewLine+nativeCrtSort+Environment.NewLine+mutableStringRuntime+Environment.NewLine+ansiNlsProfile+Environment.NewLine+crt100Profiles+Environment.NewLine+nlsFilenamePolicy+Environment.NewLine+nlsDirectStrings+Environment.NewLine+nlsDirectExhaustive+Environment.NewLine+nlsDirectJamo+Environment.NewLine+nlsDirectStress+Environment.NewLine+liveStringRuntimeGpu+Environment.NewLine+sourceCacheGpu+Environment.NewLine+preparedWallpaper+Environment.NewLine+preparedWallpaperGpu+Environment.NewLine+composedTexture+Environment.NewLine+composedTextureGpu+Environment.NewLine+tracePlane+Environment.NewLine+texturePrepGpu+Environment.NewLine+"PASS visible world trace:6 actual captures; source corners/world orbit/hide/pose/clear and retained scene; original camera/cache/PC separate."+Environment.NewLine+"PASS visible CAD wallpaper:6 actual captures; real images/mesh composition and retained scene ownership; source filtering/document/PC parity separate."+Environment.NewLine+straightImage+Environment.NewLine+appleImages+Environment.NewLine+imageGpu+Environment.NewLine+capturePolicy+Environment.NewLine+offscreen;
            File.WriteAllText(resultPath+".pending",result);File.Move(resultPath+".pending",resultPath,overwrite:true);Console.WriteLine(result);
        }
        catch(Exception error){File.WriteAllText(resultPath,"FAIL "+error);Console.WriteLine(error);}
    }
}
