using System;
using System.IO;
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
        for(int i=0;i<1500;i++)
        {
            var ack=Path.Combine(directory,"viewport-ack.txt");
            if(File.Exists(ack)&&File.ReadAllText(ack)==payload)return;
            await Task.Delay(20);
        }
        throw new TimeoutException("Screenshot acknowledgement missing for "+stage);
    }
    private async Task Run()
    {
        string resultPath=Path.Combine(directory,"viewport-result.txt");
        try
        {
            File.WriteAllText(resultPath+".family",UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Pad?"ipad":UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone?"iphone":"unsupported");
            File.WriteAllText(resultPath+".started",token);
            var gate=CandidateGeometryContractChecks.CadFrameGateAssertions.Run();
            var camera=CandidateGeometryContractChecks.LegacyCameraPresetAssertions.Run();
            var offscreen=NativeGeometryCandidate.MetalViewportChecks.Run(directory);
            var parent=Window?.RootViewController?.View??throw new Exception("Visible root view missing");
            var report=await ViewportPresentationChecks.Run(parent,Capture);
            File.WriteAllText(Path.Combine(directory,"viewport-report.json"),JsonSerializer.Serialize(report,ViewportReportContext.Default.ViewportPresentationReport));
            string result="PASS visible Metal viewport:6 captured stages; source edit/orbit/empty/resize, bounded upload ownership and disposal; full product UI/PC/physical acceptance separate."+Environment.NewLine+gate+Environment.NewLine+camera+Environment.NewLine+offscreen;
            File.WriteAllText(resultPath+".pending",result);File.Move(resultPath+".pending",resultPath,overwrite:true);Console.WriteLine(result);
        }
        catch(Exception error){File.WriteAllText(resultPath,"FAIL "+error);Console.WriteLine(error);}
    }
}
