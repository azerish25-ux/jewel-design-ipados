using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

// Owned public Win32 acquisition. No reference to any product helper or original executable.
internal static class TexturePreparationProbe
{
 internal sealed record Settings(int DeviceMaximum,uint MemoryBudget,float MemoryFraction,bool PowerOfTwo);
 internal sealed record Case(int SourceWidth,int SourceHeight,int Depth,int Pattern,bool Gray,Settings Settings,int Width,int Height,int Mode,int PreviousMode,int BrushX,int BrushY,int MapMode,string SourceRgbaBase64,string SourceStoredBase64,string SourceStoredSha256,string PreparedSourceStoredBase64,string NativeStretchStoredBase64,string OutputStoredBase64,string OutputStoredSha256,string OutputRgbaBase64,string? GrayAfterOutputRgbaBase64,string? OriginalAlphaOutputStoredBase64,int GrayAfterRgbDifferences,int OriginalAlphaDifferences);
 internal static void Run(string output)
 {
  (int W,int H,int Maximum,bool Pot,uint Budget)[] configs=[(3,2,16,false,1000000),(3,2,16,true,1000000),(7,5,4,false,1000000),(8,6,4,true,1000000),(9,7,4,false,1000000),(17,7,8,false,1000000),(2,5,8,true,32),(5,2,8,true,32),(9,7,16,true,100),(7,3,16,true,1000000)];
  var cases=new List<Case>();int repeats=0,constants=0,unchanged=0,alternatives=0;
  foreach(var config in configs)foreach(int depth in new[]{24,32})foreach(bool gray in new[]{false,true})foreach(int pattern in new[]{0,1,2}){
   var settings=new Settings(config.Maximum,config.Budget,1f,config.Pot);var first=Measure(config.W,config.H,depth,pattern,gray,settings);var second=Measure(config.W,config.H,depth,pattern,gray,settings);
   if(first!=second)throw new Exception("Nonrepeatable composed native case");repeats++;cases.Add(first);
   if(pattern==0){byte[] p=Convert.FromBase64String(first.OutputRgbaBase64);var source=Pixels(config.W,config.H,depth,pattern);if(gray)Gray(source,depth);for(int i=0;i<p.Length;i+=4)if(!p.AsSpan(i,3).SequenceEqual(source.AsSpan(0,3)))throw new Exception("Constant RGB control");constants++;}
   if(first.Mode==0){if(first.OutputStoredBase64!=first.PreparedSourceStoredBase64)throw new Exception("Unchanged upload mutated source");unchanged++;}if(gray)alternatives++;
  }
  if(Dib.LiveHandles!=0)throw new Exception("Owned native handles retained");if(!cases.Any(c=>c.GrayAfterRgbDifferences>0)||!cases.Any(c=>c.OriginalAlphaDifferences>0))throw new Exception("Pipeline order alternatives were not distinguished");
  Directory.CreateDirectory(Path.GetDirectoryName(output)!);File.WriteAllText(output,JsonSerializer.Serialize(new{SchemaVersion=1,Kind="public_windows_gdi_composed_texture_not_original_vendor_runtime",OS=RuntimeInformation.OSDescription,Architecture=RuntimeInformation.ProcessArchitecture.ToString(),OriginalTargetExecuted=false,Cases=cases,RepeatabilityControls=repeats,ConstantControls=constants,NoResizeControls=unchanged,AlternativeGrayPipelines=alternatives,LiveOwnedHandles=Dib.LiveHandles,Scope="Static-original normal24/32bit gray-before-size/filter/processed-source alpha OR order with independent public GDI RGB. Original codec/palette/option3/cache/driver/profile/PC/physical separate."}));
  Console.WriteLine("PASS composed texture:"+cases.Count+" native cases/"+repeats+" repeats/"+constants+" constant/"+unchanged+" unchanged/"+alternatives+" gray order alternatives;0 owned handles.");
 }
 private static string Hash(byte[] bytes)=>Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
 private static byte[] Pixels(int w,int h,int depth,int pattern){var rgba=new byte[w*h*4];for(int y=0;y<h;y++)for(int x=0;x<w;x++){int i=(y*w+x)*4;rgba[i]=(byte)(pattern==0?17:pattern==1?(x*31+y*79)&255:(x+y)%2==0?17:224);rgba[i+1]=(byte)(pattern==0?91:pattern==1?(x*73+y*13)&255:(x+y)%2==0?73:192);rgba[i+2]=(byte)(pattern==0?203:pattern==1?(x*17+y*101)&255:(x+y)%2==0?129:64);rgba[i+3]=depth==24?(byte)255:(byte)((x*43+y*89+7)&255);}return rgba;}
 private static void Gray(byte[] rgba,int depth){for(int i=0;i<rgba.Length;i+=4){byte g=(byte)(int)(((rgba[i]*.299+rgba[i+1]*.589)+rgba[i+2]*.111)+.5);rgba[i]=rgba[i+1]=rgba[i+2]=g;if(depth==32)rgba[i+3]=g;}}
 private static (int,int) Dimensions(int sw,int sh,int depth,Settings settings){int cw=(int)BitOperations.RoundUpToPowerOf2((uint)sw),ch=(int)BitOperations.RoundUpToPowerOf2((uint)sh),w=cw>settings.DeviceMaximum?settings.DeviceMaximum:settings.PowerOfTwo?cw:sw,h=ch>settings.DeviceMaximum?settings.DeviceMaximum:settings.PowerOfTwo?ch:sh;if(w!=sw&&h==sh)h=ch;else if(w==sw&&h!=sh)w=cw;while((float)(w*h*(depth/8))/(float)settings.MemoryBudget>settings.MemoryFraction){w/=2;h/=2;}if(w<1||h<1)throw new Exception("Invalid probe dimensions");return(w,h);}
 private static (byte[] Stored,int Mode,int Previous,int X,int Y,int Map) Stretch(byte[] rgba,int sw,int sh,int depth,int w,int h)
 {
  using var source=new Dib(sw,sh,depth);source.WriteRgba(rgba);
  if(w==sw&&h==sh){Native.GetBrushOrgEx(source.Dc,out var origin);return(source.ReadStored(),0,Native.GetStretchBltMode(source.Dc),origin.X,origin.Y,Native.GetMapMode(source.Dc));}
  using var destination=new Dib(w,h,depth);destination.Fill(0);int before=Native.GetStretchBltMode(destination.Dc);if(before!=1)throw new Exception("Measured fresh DC mode changed");if(!Native.GetBrushOrgEx(destination.Dc,out var brush))throw new Win32Exception(Marshal.GetLastWin32Error());int mode=sw/w>=2||sh/h>=2?4:before;if(mode==4&&Native.SetStretchBltMode(destination.Dc,4)==0)throw new Win32Exception(Marshal.GetLastWin32Error());if(!Native.StretchBlt(destination.Dc,0,0,w,h,source.Dc,0,0,sw,sh,0xcc0020))throw new Win32Exception(Marshal.GetLastWin32Error());return(destination.ReadStored(),Native.GetStretchBltMode(destination.Dc),before,brush.X,brush.Y,Native.GetMapMode(destination.Dc));
 }
 private static byte[] Normalize(byte[] stored,int w,int h,int depth){int stride=(w*depth/8+3)&~3;byte[] rgba=new byte[w*h*4];for(int y=0;y<h;y++)for(int x=0;x<w;x++){int s=(h-1-y)*stride+x*depth/8,i=(y*w+x)*4;rgba[i]=stored[s+2];rgba[i+1]=stored[s+1];rgba[i+2]=stored[s];rgba[i+3]=depth==24?(byte)255:stored[s+3];}return rgba;}
 // Independent logical top-row projection; product uses a stored-row helper.
 private static void Repair(byte[] stored,byte[] source,int sw,int sh,int w,int h,int depth){if(depth!=32||sw==w&&sh==h)return;for(int y=0;y<h;y++){int sy=sh-1-(int)((h-1-y)*((double)sh/h));for(int x=0;x<w;x++){int sx=(int)(x*((double)sw/w));stored[((h-1-y)*w+x)*4+3]|=source[(sy*sw+sx)*4+3];}}}
 private static Case Measure(int sw,int sh,int depth,int pattern,bool gray,Settings settings)
 {
  byte[] original=Pixels(sw,sh,depth,pattern),prepared=(byte[])original.Clone();using var input=new Dib(sw,sh,depth);input.WriteRgba(original);byte[] inputStored=input.ReadStored();if(gray)Gray(prepared,depth);input.WriteRgba(prepared);byte[] preparedStored=input.ReadStored();var(w,h)=Dimensions(sw,sh,depth,settings);var raw=Stretch(prepared,sw,sh,depth,w,h);byte[] result=(byte[])raw.Stored.Clone();Repair(result,prepared,sw,sh,w,h,depth);byte[] normalized=Normalize(result,w,h,depth);int grayDifferences=0,alphaDifferences=0;string? grayAlternative=null,alphaAlternative=null;
  if(gray){var alternative=Stretch(original,sw,sh,depth,w,h);byte[] after=(byte[])alternative.Stored.Clone();Repair(after,original,sw,sh,w,h,depth);byte[] rgba=Normalize(after,w,h,depth);Gray(rgba,depth);grayAlternative=Convert.ToBase64String(rgba);for(int i=0;i<rgba.Length;i+=4)for(int k=0;k<3;k++)if(rgba[i+k]!=normalized[i+k])grayDifferences++;}
  if(gray&&depth==32&&raw.Mode!=0){byte[] wrongAlpha=(byte[])raw.Stored.Clone();Repair(wrongAlpha,original,sw,sh,w,h,depth);alphaAlternative=Convert.ToBase64String(wrongAlpha);for(int i=3;i<result.Length;i+=4)if(wrongAlpha[i]!=result[i])alphaDifferences++;}
  return new(sw,sh,depth,pattern,gray,settings,w,h,raw.Mode,raw.Previous,raw.X,raw.Y,raw.Map,Convert.ToBase64String(original),Convert.ToBase64String(inputStored),Hash(inputStored),Convert.ToBase64String(preparedStored),Convert.ToBase64String(raw.Stored),Convert.ToBase64String(result),Hash(result),Convert.ToBase64String(normalized),grayAlternative,alphaAlternative,grayDifferences,alphaDifferences);
 }
}
