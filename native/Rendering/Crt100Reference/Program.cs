using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
if(!OperatingSystem.IsWindows()||RuntimeInformation.ProcessArchitecture!=Architecture.X64)throw new PlatformNotSupportedException();
string root=Path.GetFullPath(Environment.CurrentDirectory),dll=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);
if(!root.StartsWith("D:"+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!dll.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!output.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||File.Exists(output))throw new InvalidOperationException("Fresh D repository output and runtime required");
string hash=Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(dll)));
if(hash!="2b7ab898b2f64f4cd1ae651c93d7eb9556208fd8d8d40d425e856c83d86d2346")throw new InvalidDataException("Approved Microsoft system CRT100 identity changed");
byte[] packed=File.ReadAllBytes(Path.Combine(root,"reference/fixtures/windows-filename-semantics.json.br"));
if(Convert.ToHexStringLower(SHA256.HashData(packed))!="402d198c72fc82f0a4d78574eadd035e41ac439747245c266293502a40196f04")throw new InvalidDataException("Native controls changed");
using var source=new MemoryStream(packed);using var brotli=new System.IO.Compression.BrotliStream(source,System.IO.Compression.CompressionMode.Decompress);using var fixture=JsonDocument.Parse(brotli);
var pairs=fixture.RootElement.GetProperty("CrtComparisons").EnumerateArray().Select(x=>(Left:x.GetProperty("Left").GetString()!,Right:x.GetProperty("Right").GetString()!,Expected:x.GetProperty("Result").GetInt32())).ToArray();
byte[] cLower=Convert.FromBase64String(fixture.RootElement.GetProperty("CrtLowerUtf16Base64").GetString()!);
nint library=NativeLibrary.Load(dll);var profiles=new List<object>();
try{
 var create=Marshal.GetDelegateForFunctionPointer<Create>(NativeLibrary.GetExport(library,"_create_locale"));
 var free=Marshal.GetDelegateForFunctionPointer<Free>(NativeLibrary.GetExport(library,"_free_locale"));
 var lower=Marshal.GetDelegateForFunctionPointer<Lower>(NativeLibrary.GetExport(library,"_towlower_l"));
 var compare=Marshal.GetDelegateForFunctionPointer<Compare>(NativeLibrary.GetExport(library,"_wcsicmp_l"));
 foreach(string name in new[]{"C","English_United States.1252","Turkish"}){
  nint locale=create(2,name);if(locale==0)throw new InvalidOperationException("Explicit CRT100 locale unavailable: "+name);
  try{byte[] mapping=new byte[131072];int changes=0,repeatMismatches=0,pairDifferences=0;var comparisons=new List<object>();
   for(int unit=0;unit<65536;unit++){uint first=lower((uint)unit,locale),second=lower((uint)unit,locale);if(first>65535||first!=second)throw new InvalidDataException("CRT100 lower invalid/nonrepeatable");mapping[unit*2]=(byte)first;mapping[unit*2+1]=(byte)(first>>8);if(first!=(uint)unit)changes++;}
   foreach(var p in pairs){int first=compare(p.Left,p.Right,locale),second=compare(p.Left,p.Right,locale);if(first!=second)repeatMismatches++;if(first!=p.Expected)pairDifferences++;comparisons.Add(new{p.Left,p.Right,Result=first,Repeat=second});}
   int mappingDifferences=0;for(int i=0;i<65536;i++)if(mapping[i*2]!=cLower[i*2]||mapping[i*2+1]!=cLower[i*2+1])mappingDifferences++;
   if(repeatMismatches!=0||name=="C"&&(mappingDifferences!=0||pairDifferences!=0))throw new InvalidDataException("CRT100 C reference/repeat mismatch");
   profiles.Add(new{Locale=name,LowerUtf16Base64=Convert.ToBase64String(mapping),LowerSha256=Convert.ToHexStringLower(SHA256.HashData(mapping)),ChangedUnits=changes,UcrtCLowerDifferences=mappingDifferences,UcrtCPairDifferences=pairDifferences,RepeatMismatches=repeatMismatches,Comparisons=comparisons});
   Console.WriteLine($"PASS CRT100 {name}:65536 lower units/{pairs.Length} repeated pairs/{mappingDifferences} lower and{pairDifferences} pair differences from UCRT C");
  }finally{free(locale);}
 }
}finally{NativeLibrary.Free(library);}
Directory.CreateDirectory(Path.GetDirectoryName(output)!);using var destination=new FileStream(output,FileMode.CreateNew);JsonSerializer.Serialize(destination,new{SchemaVersion=1,Kind="installed_microsoft_crt100_explicit_locale_reference",OriginalTargetExecuted=false,RuntimeSha256=hash,RuntimeFileVersion=System.Diagnostics.FileVersionInfo.GetVersionInfo(dll).FileVersion,OS=RuntimeInformation.OSDescription,Architecture=RuntimeInformation.ProcessArchitecture.ToString(),Profiles=profiles,Scope="Owned public Microsoft CRT100 explicit locale calls. Not original loaded-runtime/current-locale proof; no production fallback/state adoption."});
[UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet=CharSet.Ansi)]delegate nint Create(int category,string locale);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]delegate void Free(nint locale);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]delegate uint Lower(uint value,nint locale);
[UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet=CharSet.Unicode)]delegate int Compare(string left,string right,nint locale);
