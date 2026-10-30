using System;
using System.IO;
using Foundation;
using UIKit;

namespace ReuseSmokeIOS;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Managed Main reached");
        UIApplication.Main(args,null,typeof(AppDelegate));
    }
}

[Register("AppDelegate")]
public class AppDelegate:UIApplicationDelegate
{
    public override bool FinishedLaunching(UIApplication app,NSDictionary options)
    {
        var path=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"reuse-result.txt");
        Console.WriteLine("Result path="+path);
        try
        {
            File.WriteAllText(path+".family",UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Pad?"ipad":
                UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone?"iphone":"unsupported");
            File.WriteAllText(path+".started","Managed FinishedLaunching reached");
            Console.WriteLine("Starting recovered checks");
            var result=ReuseSmoke.Checks.Run();
            Publish(path,result);
            Console.WriteLine(result);
        }
        catch(Exception error)
        {
            Publish(path,"FAIL "+error);
            Console.WriteLine(error);
        }
        return true;
    }

    private static void Publish(string path,string result)
    {
        File.WriteAllText(path+".pending",result);
        File.Move(path+".pending",path,overwrite:true);
    }
}
