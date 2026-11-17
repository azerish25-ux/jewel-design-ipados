using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
try {
 if(!OperatingSystem.IsWindows()||RuntimeInformation.ProcessArchitecture!=Architecture.X64)throw new PlatformNotSupportedException();
 if(args.Length!=2)throw new ArgumentException("Supply D-local Microsoft runtime and fresh output paths.");
 string root=Path.GetFullPath(Environment.CurrentDirectory),dll=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);
 if(!root.StartsWith("D:"+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!dll.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!output.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||File.Exists(output))throw new InvalidOperationException("Fresh D repository output and runtime required");
 string hash=Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(dll)));
 if(hash!="2b7ab898b2f64f4cd1ae651c93d7eb9556208fd8d8d40d425e856c83d86d2346")throw new InvalidDataException("Approved Microsoft system CRT100 identity changed");
 byte[] packed=File.ReadAllBytes(Path.Combine(root,"reference/fixtures/windows-crt100-locales.json.br"));
 if(Convert.ToHexStringLower(SHA256.HashData(packed))!="06b67db6802801b528d11831f7699a3bb83581ba457518963c3d3e5d57553474")throw new InvalidDataException("Explicit locale controls changed");
 using var source=new MemoryStream(packed);using var brotli=new System.IO.Compression.BrotliStream(source,System.IO.Compression.CompressionMode.Decompress);using var fixture=JsonDocument.Parse(brotli);
 var contexts=fixture.RootElement.GetProperty("Profiles")[0].GetProperty("Comparisons").EnumerateArray().SelectMany(x=>new[]{x.GetProperty("Left").GetString()!,x.GetProperty("Right").GetString()!}).Distinct(StringComparer.Ordinal).ToList();
 contexts.AddRange(new[]{"\0ABC","ABC\0DEF","A\ud800B","A\udfffB","\ud801\udc00\ud801\udc28","Iİıi","ΟΣΣςσ","ΟΣ","Σ","AΣ ","İ\u0307","I\u0307","A\u0307\u0323B","\ud83d\ude00ABC",new string(Enumerable.Range(1,65535).Select(x=>(char)x).ToArray())});
 nint module=NativeLibrary.Load(dll);var profiles=new List<object>();bool restored=false;
 try {
  var set=Marshal.GetDelegateForFunctionPointer<SetLocale>(NativeLibrary.GetExport(module,"setlocale"));
  var lower=Marshal.GetDelegateForFunctionPointer<LowerBuffer>(NativeLibrary.GetExport(module,"_wcslwr"));
  var compare=Marshal.GetDelegateForFunctionPointer<Compare>(NativeLibrary.GetExport(module,"_wcsicmp"));
  string original=Marshal.PtrToStringAnsi(set(2,null))??throw new InvalidOperationException("Current CRT locale query failed");
  try {
   foreach(var prior in fixture.RootElement.GetProperty("Profiles").EnumerateArray()) {
    string locale=prior.GetProperty("Locale").GetString()!;nint selected=set(2,locale);if(selected==0)throw new InvalidOperationException("CRT locale unavailable: "+locale);
    string resolved=Marshal.PtrToStringAnsi(selected)!;byte[] expected=Convert.FromBase64String(prior.GetProperty("LowerUtf16Base64").GetString()!),mapping=new byte[131072];int failures=0,differences=0;var pairs=new List<object>();var lowerCases=new List<object>();
    byte[] unitWholeCompare=new byte[65536*4];var unitWholeCompareNonzero=new List<object>();
    nint small=Marshal.AllocHGlobal(12);
    try {for(int unit=0;unit<65536;unit++) {
     ushort[] values=[(ushort)unit,0,0x0041,0xd800,0xdfff,0];for(int i=0;i<values.Length;i++)Marshal.WriteInt16(small,i*2,unchecked((short)values[i]));
     nint result=lower(small);if(result==0)failures++;else if(result!=small)throw new InvalidDataException("CRT lower returned unrelated storage");
     for(int i=1;i<values.Length;i++)if(unchecked((ushort)Marshal.ReadInt16(small,i*2))!=values[i])throw new InvalidDataException("CRT lower crossed terminator/guard");
     ushort actual=unchecked((ushort)Marshal.ReadInt16(small));mapping[unit*2]=(byte)actual;mapping[unit*2+1]=(byte)(actual>>8);if(actual!=(expected[unit*2]|expected[unit*2+1]<<8))differences++;
     string originalUnit=new((char)unit,1),lowerUnit=new((char)actual,1);int comparison=compare(originalUnit,lowerUnit);if(compare(originalUnit,lowerUnit)!=comparison)throw new InvalidDataException("Nonrepeatable unit-lower comparison");System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(unitWholeCompare.AsSpan(unit*4),comparison);if(comparison!=0)unitWholeCompareNonzero.Add(new{InputUtf16=unit,Result=comparison});
    }}finally{Marshal.FreeHGlobal(small);}
    int pairDifferences=0;foreach(var p in prior.GetProperty("Comparisons").EnumerateArray()) {
     string left=p.GetProperty("Left").GetString()!,right=p.GetProperty("Right").GetString()!;int first=compare(left,right),second=compare(left,right);if(first!=second)throw new InvalidDataException("Nonrepeatable current CRT comparison");if(first!=p.GetProperty("Result").GetInt32())pairDifferences++;pairs.Add(new{Left=left,Right=right,Result=first,Repeat=second});
    }
    byte[] supplementary=new byte[1048576*4];var supplementaryChanges=new List<object>();var supplementaryFailures=new List<uint>();byte[] supplementaryCompare=new byte[1048576*4];var supplementaryCompareNonzero=new List<object>();nint pairBuffer=Marshal.AllocHGlobal(14);
    try {for(uint scalar=0x10000;scalar<=0x10ffff;scalar++) {
     uint value=scalar-0x10000;ushort high=(ushort)(0xd800+(value>>10)),low=(ushort)(0xdc00+(value&1023));ushort[] chars=[high,low,0,0x0041,0xd800,0xdfff,0];for(int i=0;i<chars.Length;i++)Marshal.WriteInt16(pairBuffer,i*2,unchecked((short)chars[i]));
     nint result=lower(pairBuffer);if(result==0)supplementaryFailures.Add(scalar);else if(result!=pairBuffer)throw new InvalidDataException("Supplementary lower returned unrelated storage");for(int i=2;i<chars.Length;i++)if(unchecked((ushort)Marshal.ReadInt16(pairBuffer,i*2))!=chars[i])throw new InvalidDataException("Supplementary lower crossed terminator/guard");
     ushort actualHigh=unchecked((ushort)Marshal.ReadInt16(pairBuffer)),actualLow=unchecked((ushort)Marshal.ReadInt16(pairBuffer,2));int at=checked((int)(scalar-0x10000)*4);supplementary[at]=(byte)actualHigh;supplementary[at+1]=(byte)(actualHigh>>8);supplementary[at+2]=(byte)actualLow;supplementary[at+3]=(byte)(actualLow>>8);if(actualHigh!=high||actualLow!=low)supplementaryChanges.Add(new{InputScalar=scalar,OutputFirstUtf16=actualHigh,OutputSecondUtf16=actualLow});
     string originalPair=new([ (char)high,(char)low ]),lowerPair=new([ (char)actualHigh,(char)actualLow ]);int comparison=compare(originalPair,lowerPair);if(compare(originalPair,lowerPair)!=comparison)throw new InvalidDataException("Nonrepeatable supplementary-lower comparison");System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(supplementaryCompare.AsSpan(at),comparison);if(comparison!=0)supplementaryCompareNonzero.Add(new{InputScalar=scalar,Result=comparison});
    }}finally{Marshal.FreeHGlobal(pairBuffer);}
    int contextDifferences=0;foreach(string text in contexts) {
     byte[] input=Utf16(text);nint buffer=Marshal.AllocHGlobal(input.Length+10);
     try {Marshal.Copy(input,0,buffer,input.Length);ushort[] guard=[0,0x0041,0xd800,0xdfff,0];for(int i=0;i<guard.Length;i++)Marshal.WriteInt16(buffer,input.Length+i*2,unchecked((short)guard[i]));
      nint result=lower(buffer);if(result!=0&&result!=buffer)throw new InvalidDataException("Context lower returned unrelated storage");for(int i=0;i<guard.Length;i++)if(unchecked((ushort)Marshal.ReadInt16(buffer,input.Length+i*2))!=guard[i])throw new InvalidDataException("Context lower crossed guard");
      byte[] actual=new byte[input.Length];Marshal.Copy(buffer,actual,0,actual.Length);byte[] perUnit=(byte[])input.Clone();for(int i=0;i<perUnit.Length;i+=2){int unit=input[i]|input[i+1]<<8;if(unit==0)break;perUnit[i]=expected[unit*2];perUnit[i+1]=expected[unit*2+1];}if(!actual.AsSpan().SequenceEqual(perUnit))contextDifferences++;
      lowerCases.Add(new{InputUtf16Base64=Convert.ToBase64String(input),OutputUtf16Base64=Convert.ToBase64String(actual),Success=result!=0,PerUnitEqual=actual.AsSpan().SequenceEqual(perUnit)});
     }finally{Marshal.FreeHGlobal(buffer);}
    }
    profiles.Add(new{Locale=locale,ResolvedLocale=resolved,LowerUtf16Base64=Convert.ToBase64String(mapping),LowerSha256=Convert.ToHexStringLower(SHA256.HashData(mapping)),LowerFailures=failures,ExplicitLowerDifferences=differences,ExplicitPairDifferences=pairDifferences,WholeBufferPerUnitDifferences=contextDifferences,SupplementaryUnits=1048576,SupplementarySha256=Convert.ToHexStringLower(SHA256.HashData(supplementary)),SupplementaryChanges=supplementaryChanges,SupplementaryFailures=supplementaryFailures,UnitWholeCompareSha256=Convert.ToHexStringLower(SHA256.HashData(unitWholeCompare)),UnitWholeCompareNonzero=unitWholeCompareNonzero,SupplementaryCompareSha256=Convert.ToHexStringLower(SHA256.HashData(supplementaryCompare)),SupplementaryCompareNonzero=supplementaryCompareNonzero,Comparisons=pairs,LowerContexts=lowerCases});
    Console.WriteLine($"PASS current CRT100 {locale}:65536 guarded lower units/{pairs.Count} repeated pairs/{lowerCases.Count} whole buffers; {failures} unit failures/{differences} explicit lower differences/{pairDifferences} explicit pair differences/{contextDifferences} whole-vs-unit differences/{supplementaryChanges.Count} changed of1048576 supplementary scalars/{supplementaryFailures.Count} supplementary failures/{unitWholeCompareNonzero.Count} unit and{supplementaryCompareNonzero.Count} supplementary lower/compare disagreements");
   }
  }finally{if(set(2,original)==0)throw new InvalidOperationException("CRT locale restoration failed");restored=true;}
 }finally{NativeLibrary.Free(module);}
 Directory.CreateDirectory(Path.GetDirectoryName(output)!);using var destination=new FileStream(output,FileMode.CreateNew);JsonSerializer.Serialize(destination,new{SchemaVersion=1,Kind="installed_microsoft_crt100_current_wide_entrypoint_reference",OriginalTargetExecuted=false,RuntimeSha256=hash,RuntimeFileVersion=System.Diagnostics.FileVersionInfo.GetVersionInfo(dll).FileVersion,OS=RuntimeInformation.OSDescription,Architecture=RuntimeInformation.ProcessArchitecture.ToString(),LocaleRestored=restored,Profiles=profiles,Scope="Owned public _wcslwr/_wcsicmp with isolated process CRT locale changes. Does not identify original application runtime/current locale or complete fallback/ANSI/document behavior."});
}catch(Exception e){Console.Error.WriteLine(e.GetType().Name+": "+e.Message);Environment.ExitCode=1;}
static byte[] Utf16(string text){byte[] bytes=new byte[text.Length*2];for(int i=0;i<text.Length;i++){bytes[i*2]=(byte)text[i];bytes[i*2+1]=(byte)(text[i]>>8);}return bytes;}
[UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet=CharSet.Ansi)]delegate nint SetLocale(int category,string? name);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]delegate nint LowerBuffer(nint buffer);
[UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet=CharSet.Unicode)]delegate int Compare(string left,string right);
