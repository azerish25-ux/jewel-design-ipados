using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

if(!OperatingSystem.IsWindows())throw new PlatformNotSupportedException("Public Windows GDI reference only.");
bool integerProbe=args.Length==2&&args[1]=="--integer-probe";
bool halftoneProbe=args.Length==2&&args[1]=="--halftone-probe";
bool mixedProbe=args.Length==2&&args[1]=="--mixed-probe";
if(args.Length!=1&&!integerProbe&&!halftoneProbe&&!mixedProbe)throw new ArgumentException("Expected output JSON under this D: repository, optional --integer-probe/--halftone-probe/--mixed-probe.");
string root=Path.GetFullPath(Environment.CurrentDirectory),output=Path.GetFullPath(args[0]);
if(!root.StartsWith("D:"+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!output.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new InvalidOperationException("Reference output leaves D: workspace.");
if(halftoneProbe){HalftoneProbe.Run(output);return;}
var sizes=new[]{(1,1,1,1),(3,2,3,2),(3,2,5,7),(7,5,4,3),(8,6,4,3),(9,7,4,3),(4,8,8,4),(13,7,7,4)};
if(integerProbe)sizes=[(1,1,1,1),(2,3,3,2),(3,5,5,3),(4,3,7,2),(5,4,3,7),(6,5,11,3),(7,3,4,5),(9,7,5,4),(11,9,6,5),(13,7,7,4),(5,13,3,7),(7,5,5,7),(1,9,5,5),(9,1,5,5),(2,9,3,7),(9,2,7,3),(3,1,2,1),(1,3,1,2),(3,3,2,2),(3,4,2,4),(4,3,4,2),(2,3,3,3),(3,5,2,4),(3,7,2,5),(5,3,4,2),(7,1,6,1),(1,7,1,6),(7,7,6,6),(7,3,6,5),(7,3,6,4),(7,3,6,6),(4,5,3,4),(4,5,3,7),(2,1,1,1),(2,2,1,1),(9,4,8,3),(11,5,12,6),(5,9,6,8),(7,7,8,8),(13,15,12,16),(9,9,7,7),(5,5,1,1),(8,6,2,2),(9,7,1,2),(32,7,3,2)];
if(mixedProbe)sizes=[(4,8,8,4),(8,4,4,8),(3,7,5,4),(7,3,4,5),(5,9,7,5),(9,5,5,7),(4,7,9,3),(7,4,3,9),(2,5,3,3),(5,2,3,3),(6,11,11,6),(11,6,6,11)];
var sources=new List<SourceCase>();var cases=new List<ResizeCase>();int controls=0;
foreach(int depth in new[]{24,32})foreach(var size in sizes)foreach(int pattern in mixedProbe?new[]{0,1,2,3,4,5,6}:integerProbe?new[]{0,1,2,3}:new[]{0,1,2})
{
 var (sw,sh,dw,dh)=size;byte[] rgba=Pixels(sw,sh,pattern);string sourceId="b"+depth+"-"+sw+"x"+sh+"-p"+pattern;
 if(!sources.Any(s=>s.Id==sourceId)){using var src=new Dib(sw,sh,depth);src.WriteRgba(rgba);var b=src.ReadStored();sources.Add(new(sourceId,sw,sh,depth,src.Stride,Convert.ToBase64String(rgba),Hash(b),Convert.ToBase64String(b)));}
 foreach(int seed in new[]{0,90})foreach(int mode in mixedProbe?new[]{4}:integerProbe?new[]{1,3}:new[]{0,3,4})
 {
  var first=Resize(sourceId,rgba,sw,sh,dw,dh,depth,pattern,seed,mode);var second=Resize(sourceId,rgba,sw,sh,dw,dh,depth,pattern,seed,mode);
  if(first.StoredBase64!=second.StoredBase64||first.EffectiveStretchMode!=second.EffectiveStretchMode||first.InitialAllZero!=second.InitialAllZero||first.BrushX!=second.BrushX||first.BrushY!=second.BrushY)throw new Exception("Non-repeatable GDI result");controls++;
  byte[] actual=Convert.FromBase64String(first.RgbaBase64);
  if((sw==dw&&sh==dh)||pattern==0){for(int y=0;y<dh;y++)for(int x=0;x<dw;x++){int a=(y*dw+x)*4,s=pattern==0?0:(y*sw+x)*4;if(!actual.AsSpan(a,3).SequenceEqual(rgba.AsSpan(s,3)))throw new Exception("Independent identity/constant RGB control failed");}controls++;}
  cases.Add(first);
 }
}
if(Dib.LiveHandles!=0)throw new Exception("Native GDI handles retained");
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
var report=new {SchemaVersion=1,Kind=mixedProbe?"public_windows_gdi_mixed_selection_not_original_vendor_runtime":integerProbe?"public_windows_gdi_integer_selection_not_original_vendor_runtime":"public_windows_gdi_not_original_vendor_runtime",OS=RuntimeInformation.OSDescription,Architecture=RuntimeInformation.ProcessArchitecture.ToString(),Runtime=RuntimeInformation.FrameworkDescription,OriginalTargetExecuted=false,SourceSetup="Observed Rhino5 positive-height BI_RGB DIB/memory DC/select/SRCCOPY; no MFC wrapper execution or undocumented DC-state equivalence claimed.",Sources=sources,Cases=cases,Controls=controls,LiveOwnedHandles=Dib.LiveHandles,Scope="Actual current-host Windows GDI output only. Portable resampler, original vendor/OS/driver/MFC, palette/profile/PC/physical parity separate."};
File.WriteAllText(output,JsonSerializer.Serialize(report));
Console.WriteLine("PASS public Windows GDI RGB:"+cases.Count+" measured cases/"+controls+" repeatability and independent RGB controls; original vendor runtime/portable filter parity separate.");

static string Hash(byte[] b)=>Convert.ToHexString(SHA256.HashData(b)).ToLowerInvariant();
static byte[] Pixels(int w,int h,int pattern){byte[] b=new byte[w*h*4];for(int y=0;y<h;y++)for(int x=0;x<w;x++){int i=(y*w+x)*4;b[i]=(byte)(pattern==0?17:pattern==1?(x*31+y*79)&255:(x+y)%2==0?0:255);b[i+1]=(byte)(pattern==0?91:pattern==1?(x*73+y*13)&255:(x+y)%3==0?255:0);b[i+2]=(byte)(pattern==0?203:pattern==1?(x*17+y*101)&255:(x+y)%5==0?255:0);b[i+3]=(byte)((x*43+y*89+7)&255);if(pattern==3){byte value=x==w-1&&y==h-1?(byte)0:(byte)255;b[i]=b[i+1]=b[i+2]=b[i+3]=value;}}if(pattern>=4){for(int y=0;y<h;y++)for(int x=0;x<w;x++){int i=(y*w+x)*4;b[i]=(byte)((x*31+y*79+11)&255);b[i+1]=(byte)((x*73+y*13+23)&255);b[i+2]=(byte)((x*17+y*101+37)&255);}int cx=Math.Max(0,(w-2)/2),cy=Math.Max(0,(h-2)/2);byte[] a=pattern==6?[128,0,0]:pattern==5?[220,180,40]:[32,80,160],c=pattern==6?[0,64,0]:pattern==5?[32,80,160]:[220,180,40];for(int yy=0;yy<2&&cy+yy<h;yy++)for(int xx=0;xx<2&&cx+xx<w;xx++){var color=(xx+yy)%2==0?a:c;int i=((cy+yy)*w+cx+xx)*4;for(int k=0;k<3;k++)b[i+k]=color[k];}}return b;}
static ResizeCase Resize(string sourceId,byte[] rgba,int sw,int sh,int dw,int dh,int depth,int pattern,int seed,int requestedMode)
{
 using var source=new Dib(sw,sh,depth);using var destination=new Dib(dw,dh,depth);source.WriteRgba(rgba);
 byte[] initial=destination.ReadStored();destination.Fill((byte)seed);
 int previous=Native.GetStretchBltMode(destination.Dc),map=Native.GetMapMode(destination.Dc),icm=Native.SetICMMode(destination.Dc,3);
 if(previous==0||map==0||!Native.GetBrushOrgEx(destination.Dc,out var brush))throw new Win32Exception(Marshal.GetLastWin32Error());
 int selected=requestedMode==0?(sw/dw>=2||sh/dh>=2?4:0):requestedMode;
 if(selected!=0&&Native.SetStretchBltMode(destination.Dc,selected)==0)throw new Win32Exception(Marshal.GetLastWin32Error());
 if(!Native.StretchBlt(destination.Dc,0,0,dw,dh,source.Dc,0,0,sw,sh,0xcc0020))throw new Win32Exception(Marshal.GetLastWin32Error());
 byte[] stored=destination.ReadStored(),result=destination.ReadRgba();
 return new(sourceId,dw,dh,depth,pattern,seed,requestedMode,previous,Native.GetStretchBltMode(destination.Dc),brush.X,brush.Y,map,icm,initial.All(x=>x==0),destination.Stride,Hash(stored),Convert.ToBase64String(stored),Convert.ToBase64String(result));
}
record SourceCase(string Id,int Width,int Height,int Depth,int Stride,string RgbaBase64,string StoredSha256,string StoredBase64);
record ResizeCase(string SourceId,int Width,int Height,int Depth,int Pattern,int Seed,int RequestedMode,int PreviousStretchMode,int EffectiveStretchMode,int BrushX,int BrushY,int MapMode,int IcmMode,bool InitialAllZero,int Stride,string StoredSha256,string StoredBase64,string RgbaBase64);

sealed class Dib:IDisposable
{
 public static int LiveHandles;public readonly int Width,Height,Depth,Stride;private nint bitmap,bits,oldBitmap;public nint Dc{get;private set;}
 public Dib(int width,int height,int depth)
 {
  Width=width;Height=height;Depth=depth;if(width<1||height<1||(depth!=24&&depth!=32))throw new ArgumentException("Bounded24/32bit reference");Stride=checked(((width*depth+31)/32)*4);
  var info=new Native.Header{Size=40,Width=width,Height=height,Planes=1,BitCount=(ushort)depth,SizeImage=(uint)checked(Stride*height)};
  try{bitmap=Native.CreateDIBSection(0,ref info,0,out bits,0,0);if(bitmap!=0)LiveHandles++;if(bitmap==0||bits==0)throw new Win32Exception(Marshal.GetLastWin32Error());Dc=Native.CreateCompatibleDC(0);if(Dc==0)throw new Win32Exception(Marshal.GetLastWin32Error());LiveHandles++;oldBitmap=Native.SelectObject(Dc,bitmap);if(oldBitmap==0||oldBitmap==-1){oldBitmap=0;throw new Win32Exception(Marshal.GetLastWin32Error());}}
  catch{Dispose();throw;}
 }
 public void Fill(byte value){byte[] b=new byte[Stride*Height];Array.Fill(b,value);Marshal.Copy(b,0,bits,b.Length);}
 public void WriteRgba(byte[] rgba){if(rgba.Length!=Width*Height*4)throw new ArgumentException("RGBA shape");byte[] b=new byte[Stride*Height];Array.Fill(b,(byte)165);for(int y=0;y<Height;y++)for(int x=0;x<Width;x++){int s=(y*Width+x)*4,d=(Height-1-y)*Stride+x*(Depth/8);b[d]=rgba[s+2];b[d+1]=rgba[s+1];b[d+2]=rgba[s];if(Depth==32)b[d+3]=rgba[s+3];}Marshal.Copy(b,0,bits,b.Length);}
 public byte[] ReadStored(){if(!Native.GdiFlush())throw new Win32Exception(Marshal.GetLastWin32Error());byte[] b=new byte[Stride*Height];Marshal.Copy(bits,b,0,b.Length);return b;}
 public byte[] ReadRgba(){var b=ReadStored();byte[] r=new byte[Width*Height*4];for(int y=0;y<Height;y++)for(int x=0;x<Width;x++){int s=(Height-1-y)*Stride+x*(Depth/8),d=(y*Width+x)*4;r[d]=b[s+2];r[d+1]=b[s+1];r[d+2]=b[s];r[d+3]=Depth==32?b[s+3]:(byte)255;}return r;}
 public void Dispose(){var errors=new List<string>();if(oldBitmap!=0&&Dc!=0){var restored=Native.SelectObject(Dc,oldBitmap);if(restored==0||restored==-1)errors.Add("Restore bitmap");oldBitmap=0;}if(bitmap!=0){if(Native.DeleteObject(bitmap))LiveHandles--;else errors.Add("Delete bitmap");bitmap=0;}if(Dc!=0){if(Native.DeleteDC(Dc))LiveHandles--;else errors.Add("Delete HDC");Dc=0;}bits=0;if(errors.Count!=0)throw new Exception(string.Join(",",errors));}
}
static partial class Native
{
 [StructLayout(LayoutKind.Sequential)]public struct Header{public uint Size;public int Width,Height;public ushort Planes,BitCount;public uint Compression,SizeImage;public int XPelsPerMeter,YPelsPerMeter;public uint ClrUsed,ClrImportant;}
 [StructLayout(LayoutKind.Sequential)]public struct Point{public int X,Y;}
 [DllImport("gdi32.dll",SetLastError=true)]public static extern nint CreateDIBSection(nint hdc,ref Header info,uint usage,out nint bits,nint section,uint offset);
 [DllImport("gdi32.dll",SetLastError=true)]public static extern nint CreateCompatibleDC(nint hdc);
 [DllImport("gdi32.dll",SetLastError=true)]public static extern nint SelectObject(nint hdc,nint value);
 [DllImport("gdi32.dll",SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]public static extern bool DeleteObject(nint value);
 [DllImport("gdi32.dll",SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]public static extern bool DeleteDC(nint value);
 [DllImport("gdi32.dll",SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]public static extern bool StretchBlt(nint dest,int x,int y,int width,int height,nint source,int sx,int sy,int sw,int sh,uint rop);
 [DllImport("gdi32.dll",SetLastError=true)]public static extern int SetStretchBltMode(nint hdc,int mode);
 [DllImport("gdi32.dll",SetLastError=true)]public static extern int GetStretchBltMode(nint hdc);
 [DllImport("gdi32.dll",SetLastError=true)]public static extern int GetMapMode(nint hdc);
 [DllImport("gdi32.dll",SetLastError=true)]public static extern int SetICMMode(nint hdc,int mode);
 [DllImport("gdi32.dll",SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]public static extern bool GetBrushOrgEx(nint hdc,out Point point);
 [DllImport("gdi32.dll",SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]public static extern bool GdiFlush();
}
