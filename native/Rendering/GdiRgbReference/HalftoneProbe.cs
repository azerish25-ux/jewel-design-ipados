using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

static class HalftoneProbe
{
 public static void Run(string output)
 {
  if(Marshal.SizeOf<Native.ColorAdjustment>()!=24)throw new Exception("COLORADJUSTMENT ABI");
  var cases=new List<Response>();int controls=0;
  foreach(int depth in new[]{24,32})foreach(bool vertical in new[]{false,true})foreach(int source in new[]{2,3,4,5,7,8})foreach(int destination in new[]{1,2,3,4,5,7}){
   foreach(int constant in new[]{0,17,91,203,255})Add("constant",-1,constant,3);
   Add("ramp",-1,255,3);
   for(int i=0;i<source;i++){Add("impulse",i,255,3);Add("impulse",i,128,3);}
   if(source==4&&destination==2)for(int channel=0;channel<3;channel++)for(int i=0;i<source;i++)Add("impulse",i,255,channel);
   void Add(string pattern,int index,int level,int channel){var a=Measure(depth,vertical,source,destination,pattern,index,level,channel);var b=Measure(depth,vertical,source,destination,pattern,index,level,channel);if(a.StoredBase64!=b.StoredBase64||a.After!=b.After)throw new Exception("Non-repeatable HALFTONE pixels/state");controls++;if(pattern=="constant"){byte[] rgb=Convert.FromBase64String(a.RgbaBase64);for(int i=0;i<rgb.Length;i+=4)if(rgb[i]!=level||rgb[i+1]!=level||rgb[i+2]!=level)throw new Exception("HALFTONE constant control");controls++;}cases.Add(a);}
  }
  if(Dib.LiveHandles!=0)throw new Exception("Native GDI handles retained");
  Directory.CreateDirectory(Path.GetDirectoryName(output)!);File.WriteAllText(output,JsonSerializer.Serialize(new{SchemaVersion=1,Kind="public_windows_gdi_halftone_response_not_original_vendor_runtime",OS=RuntimeInformation.OSDescription,Architecture=RuntimeInformation.ProcessArchitecture.ToString(),Runtime=RuntimeInformation.FrameworkDescription,OriginalTargetExecuted=false,Cases=cases,Controls=controls,LiveOwnedHandles=Dib.LiveHandles,Scope="Actual current-host Win32 HALFTONE state and pixel response. Complete portable filter/original vendor/MFC/OS/driver/profile/PC/physical parity separate."}));
  Console.WriteLine("PASS public GDI HALFTONE response:"+cases.Count+" cases/"+controls+" repeatability and independent constant controls; original vendor/portable filter parity separate.");
 }
 static Response Measure(int depth,bool vertical,int size,int target,string pattern,int index,int level,int channel)
 {
  int sw=vertical?1:size,sh=vertical?size:1,dw=vertical?1:target,dh=vertical?target:1;byte[] rgba=new byte[sw*sh*4];for(int i=0;i<size;i++){int value=pattern=="constant"?level:pattern=="ramp"?i*255/(size-1):i==index?level:0;for(int c=0;c<3;c++)rgba[i*4+c]=(byte)(channel==3||channel==c?value:0);rgba[i*4+3]=255;}
  using var source=new Dib(sw,sh,depth);using var destination=new Dib(dw,dh,depth);source.WriteRgba(rgba);destination.Fill(90);
  var before=new Native.ColorAdjustment{Size=24};if(!Native.GetColorAdjustment(destination.Dc,ref before)||!Native.GetBrushOrgEx(destination.Dc,out var brushBefore))throw new Win32Exception(Marshal.GetLastWin32Error());
  if(Native.SetStretchBltMode(destination.Dc,4)==0)throw new Win32Exception(Marshal.GetLastWin32Error());
  var after=new Native.ColorAdjustment{Size=24};if(!Native.GetColorAdjustment(destination.Dc,ref after)||!Native.GetBrushOrgEx(destination.Dc,out var brushAfter)||Native.GetStretchBltMode(destination.Dc)!=4)throw new Win32Exception(Marshal.GetLastWin32Error());
  if(!Native.StretchBlt(destination.Dc,0,0,dw,dh,source.Dc,0,0,sw,sh,0xcc0020))throw new Win32Exception(Marshal.GetLastWin32Error());
  var bytes=destination.ReadStored();return new(depth,vertical,size,target,pattern,index,level,channel,State(before),State(after),brushBefore.X,brushBefore.Y,brushAfter.X,brushAfter.Y,destination.Stride,Convert.ToBase64String(rgba),Convert.ToBase64String(bytes),Convert.ToBase64String(destination.ReadRgba()),Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant());
 }
 static Adjustment State(Native.ColorAdjustment x)=>new(x.Size,x.Flags,x.IlluminantIndex,x.RedGamma,x.GreenGamma,x.BlueGamma,x.ReferenceBlack,x.ReferenceWhite,x.Contrast,x.Brightness,x.Colorfulness,x.RedGreenTint);
}
sealed record Adjustment(ushort Size,ushort Flags,ushort IlluminantIndex,ushort RedGamma,ushort GreenGamma,ushort BlueGamma,ushort ReferenceBlack,ushort ReferenceWhite,short Contrast,short Brightness,short Colorfulness,short RedGreenTint);
sealed record Response(int Depth,bool Vertical,int Source,int Target,string Pattern,int Index,int Level,int Channel,Adjustment Before,Adjustment After,int BrushBeforeX,int BrushBeforeY,int BrushAfterX,int BrushAfterY,int Stride,string SourceRgbaBase64,string StoredBase64,string RgbaBase64,string StoredSha256);
static partial class Native
{
 [StructLayout(LayoutKind.Sequential)]public struct ColorAdjustment{public ushort Size,Flags,IlluminantIndex,RedGamma,GreenGamma,BlueGamma,ReferenceBlack,ReferenceWhite;public short Contrast,Brightness,Colorfulness,RedGreenTint;}
 [DllImport("gdi32.dll",SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]public static extern bool GetColorAdjustment(nint hdc,ref ColorAdjustment adjustment);
}
