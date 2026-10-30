using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Foundation;
using UIKit;
namespace NativeGeometryCandidate;
public static class Program { public static void Main(string[] args)=>UIApplication.Main(args,null,typeof(AppDelegate)); }
[Register("AppDelegate")]
public class AppDelegate:UIApplicationDelegate
{
 [DllImport("__Internal",EntryPoint="jd_occt_candidate_run",CallingConvention=CallingConvention.Cdecl)]
 private static extern int RunNative([Out]byte[] result,uint capacity);
 public override bool FinishedLaunching(UIApplication app,NSDictionary options)
 {
  var path=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"native-result.txt");
  try
  {
   File.WriteAllText(path+".family",UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Pad?"ipad":UIDevice.CurrentDevice.UserInterfaceIdiom==UIUserInterfaceIdiom.Phone?"iphone":"unsupported");
   File.WriteAllText(path+".started","Managed launch reached before native candidate");
   var buffer=new byte[8192];int status=RunNative(buffer,(uint)buffer.Length);int end=Array.IndexOf(buffer,(byte)0);
   if(end<0)throw new Exception("Native report missing terminator");var result=Encoding.UTF8.GetString(buffer,0,end);
   if(status!=0||!result.StartsWith("PASS OCCT ",StringComparison.Ordinal))throw new Exception("Native candidate status "+status+": "+result);
   File.WriteAllText(path,result);Console.WriteLine(result);
  }catch(Exception error){File.WriteAllText(path,"FAIL "+error);Console.WriteLine(error);}
  return true;
 }
}
