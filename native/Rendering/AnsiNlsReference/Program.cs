using System.Buffers;
using System.Buffers.Binary;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
try { unsafe {
 if(!OperatingSystem.IsWindows()||!BitConverter.IsLittleEndian||args.Length!=1)throw new PlatformNotSupportedException();
 string root=Path.GetFullPath(Environment.CurrentDirectory),output=Path.GetFullPath(args[0]);
 if(!root.StartsWith("D:"+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!output.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||File.Exists(output))throw new InvalidOperationException("Fresh D repository output required");
 var before=Version();if(before.NlsVersion!=0x60502||before.EffectiveId!=1025)throw new InvalidDataException("Recorded NLS1 profile changed");
 string localeName=Locale(0x5c),codePageText=Locale(0x1004);uint codePage=uint.Parse(codePageText,CultureInfo.InvariantCulture),acp=Native.GetACP();
 if(codePage!=1256||!Native.GetCPInfo(codePage,out var info)||info.MaxCharSize!=1)throw new InvalidDataException("Locale1 single-byte code page changed");
 byte[] decoded=new byte[512],lower=new byte[256],compareA=new byte[65536],compareW=new byte[65536],lowerPairs=new byte[131072],decodedPairs=new byte[262144];char[] units=new char[256],acpUnits=new char[256];
 byte* single=stackalloc byte[2],target=stackalloc byte[32],left=stackalloc byte[2],right=stackalloc byte[2],pair=stackalloc byte[2];char* wide=stackalloc char[8],wideLeft=stackalloc char[2],wideRight=stackalloc char[2];single[1]=0;left[1]=right[1]=0;wideLeft[1]=wideRight[1]='\0';
 for(int key=0;key<256;key++) {
  single[0]=(byte)key;int count=Native.MultiByteToWideChar(codePage,0,single,1,wide,8);if(count!=1)throw new InvalidDataException("Single-byte decode width/error");units[key]=wide[0];BinaryPrimitives.WriteUInt16LittleEndian(decoded.AsSpan(key*2),wide[0]);
  count=Native.MultiByteToWideChar(acp,0,single,1,wide,8);if(count!=1)throw new InvalidDataException("ACP control decode width/error");acpUnits[key]=wide[0];
  count=Native.LCMapStringA(1,0x100,single,1,target,32);if(count!=1)throw new InvalidDataException("Single-byte lower width/error");lower[key]=target[0];
 }
 int acpDisagreements=0,wideDisagreements=0,pairLowerDifferences=0,pairDecodeDifferences=0;
 for(int a=0;a<256;a++)for(int b=0;b<256;b++) {
  int index=a*256+b;left[0]=(byte)a;right[0]=(byte)b;wideLeft[0]=units[a];wideRight[0]=units[b];int first=Native.CompareStringA(1,0x20001,left,-1,right,-1),repeat=Native.CompareStringA(1,0x20001,left,-1,right,-1),control=Native.CompareStringW(1,0x20001,wideLeft,-1,wideRight,-1);
  if(first is <1 or >3||first!=repeat||control is <1 or >3)throw new InvalidDataException("Native ANSI comparison failure/nonrepeatability");compareA[index]=(byte)first;compareW[index]=(byte)control;if(first!=control)wideDisagreements++;
  wideLeft[0]=acpUnits[a];wideRight[0]=acpUnits[b];int acpControl=Native.CompareStringW(1,0x20001,wideLeft,-1,wideRight,-1);if(acpControl is <1 or >3)throw new InvalidDataException("ACP comparison control failed");if(acpControl!=first)acpDisagreements++;
  pair[0]=(byte)a;pair[1]=(byte)b;int count=Native.LCMapStringA(1,0x100,pair,2,target,32);if(count!=2)throw new InvalidDataException("Pair ANSI lower width/error");lowerPairs[index*2]=target[0];lowerPairs[index*2+1]=target[1];if(target[0]!=lower[a]||target[1]!=lower[b])pairLowerDifferences++;
  count=Native.MultiByteToWideChar(codePage,0,pair,2,wide,8);if(count!=2)throw new InvalidDataException("Pair ANSI decode width/error");BinaryPrimitives.WriteUInt16LittleEndian(decodedPairs.AsSpan(index*4),wide[0]);BinaryPrimitives.WriteUInt16LittleEndian(decodedPairs.AsSpan(index*4+2),wide[1]);if(wide[0]!=units[a]||wide[1]!=units[b])pairDecodeDifferences++;
 }
 int zeroCount=Native.LCMapStringA(1,0x100,single,0,target,32),zeroError=Marshal.GetLastPInvokeError();
 var planes=new List<object>();var invalidSamples=new List<object>();int total=0,totalInvalid=0,totalChanges=0,compositionDifferences=0;
 byte* utf8=stackalloc byte[4];byte[] header=new byte[8];
 for(int plane=0;plane<=16;plane++) {
  using var digest=IncrementalHash.CreateHash(HashAlgorithmName.SHA256);int count=0,changed=0,invalid=0;
  for(int key=plane<<16;key<(plane+1)*65536;key++) {
   if(key is >=0xd800 and <=0xdfff)continue;int size=Encode((uint)key,new Span<byte>(utf8,4)),mapped=Native.LCMapStringA(1,0x100,utf8,size,target,32);if(mapped<1||mapped>32)throw new InvalidDataException("Scalar ANSI lower failed");
   bool same=mapped==size,composition=mapped==size;for(int i=0;i<mapped;i++){if(i>=size||target[i]!=utf8[i])same=false;if(i>=size||target[i]!=lower[utf8[i]])composition=false;}if(!composition)compositionDifferences++;if(!same)changed++;
   var status=Rune.DecodeFromUtf8(new ReadOnlySpan<byte>(target,mapped),out _,out int consumed);if(status!=OperationStatus.Done||consumed!=mapped){invalid++;if(invalidSamples.Count<16)invalidSamples.Add(new{InputScalar=key,InputUtf8Base64=Convert.ToBase64String(new ReadOnlySpan<byte>(utf8,size)),OutputUtf8Base64=Convert.ToBase64String(new ReadOnlySpan<byte>(target,mapped))});}
   BinaryPrimitives.WriteInt32LittleEndian(header,key);BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(4),mapped);digest.AppendData(header);digest.AppendData(new ReadOnlySpan<byte>(target,mapped));count++;
  }
  planes.Add(new{Index=plane,Cases=count,Changed=changed,InvalidUtf8Outputs=invalid,NativeSha256=Convert.ToHexStringLower(digest.GetHashAndReset())});total+=count;totalChanges+=changed;totalInvalid+=invalid;
 }
 var after=Version();if(before!=after)throw new InvalidDataException("NLS version changed during acquisition");
 var result=new{SchemaVersion=1,Kind="public_windows_nls1_ansi_reference_not_original_runtime",OriginalTargetExecuted=false,OS=RuntimeInformation.OSDescription,Locale=1,LowerFlags=0x100,CompareFlags=0x20001,DecodeFlags=0,LocaleName=localeName,CodePage=codePage,SystemACP=acp,MaxCharSize=info.MaxCharSize,Before=before,After=after,DecodedSingleUtf16Base64=Convert.ToBase64String(decoded),DecodedSingleSha256=Hash(decoded),LowerSingleBase64=Convert.ToBase64String(lower),LowerSingleSha256=Hash(lower),PairCompareABase64=Convert.ToBase64String(compareA),PairCompareWBase64=Convert.ToBase64String(compareW),PairLowerOutputBase64=Convert.ToBase64String(lowerPairs),PairDecodedSha256=Hash(decodedPairs),PairComparisons=65536,WideComparisonDisagreements=wideDisagreements,ACPComparisonDisagreements=acpDisagreements,PairLowerCompositionDifferences=pairLowerDifferences,PairDecodeCompositionDifferences=pairDecodeDifferences,ZeroLengthLower=new{Count=zeroCount,Error=zeroError},ValidUnicodeLowerCases=total,ChangedUnicodeCases=totalChanges,InvalidUtf8OutputCount=totalInvalid,ScalarCompositionDifferences=compositionDifferences,Planes=planes,InvalidUtf8Samples=invalidSamples,Scope="Owned public Windows ANSI backend controls. UTF8 inputs are source-derived valid scalars; original conversion/ASCII-bypass/fallback/byte-to-wide assignment and current-runtime/application behavior remain separate."};
 Directory.CreateDirectory(Path.GetDirectoryName(output)!);using var destination=new FileStream(output,FileMode.CreateNew);JsonSerializer.Serialize(destination,result);Console.WriteLine($"PASS NLS1 ANSI reference:CP{codePage}/65536 repeated pairs/{total} valid scalar lower calls/{totalInvalid} invalid UTF8 outputs/{wideDisagreements} locale-W and{acpDisagreements} ACP differences");
}}
catch(Exception e){Console.Error.WriteLine(e.GetType().Name+": "+e.Message);Environment.ExitCode=1;}
static string Hash(byte[] bytes)=>Convert.ToHexStringLower(SHA256.HashData(bytes));
static unsafe string Locale(uint kind){char* chars=stackalloc char[100];int n=Native.GetLocaleInfoW(1,kind,chars,100);if(n<=1)throw new InvalidOperationException("Locale query failed");return new string(chars,0,n-1);}
static VersionInfo Version(){var v=new Native.NlsVersion{Size=32};if(!Native.GetNLSVersion(1,1,ref v))throw new InvalidOperationException("NLS version query failed");return new(v.Version,v.DefinedVersion,v.EffectiveId,v.CustomVersion);}
static int Encode(uint scalar,Span<byte> bytes){if(scalar<0x80){bytes[0]=(byte)scalar;return 1;}if(scalar<0x800){bytes[0]=(byte)(0xc0|(scalar>>6));bytes[1]=(byte)(0x80|(scalar&63));return 2;}if(scalar<0x10000){bytes[0]=(byte)(0xe0|(scalar>>12));bytes[1]=(byte)(0x80|((scalar>>6)&63));bytes[2]=(byte)(0x80|(scalar&63));return 3;}bytes[0]=(byte)(0xf0|(scalar>>18));bytes[1]=(byte)(0x80|((scalar>>12)&63));bytes[2]=(byte)(0x80|((scalar>>6)&63));bytes[3]=(byte)(0x80|(scalar&63));return 4;}
sealed record VersionInfo(uint NlsVersion,uint DefinedVersion,uint EffectiveId,Guid CustomVersion);
static unsafe class Native {
 [StructLayout(LayoutKind.Sequential)]internal struct NlsVersion{internal uint Size,Version,DefinedVersion,EffectiveId;internal Guid CustomVersion;}
 [StructLayout(LayoutKind.Sequential)]internal struct CpInfo{internal uint MaxCharSize;internal fixed byte DefaultChar[2];internal fixed byte LeadByte[12];}
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)]internal static extern int GetLocaleInfoW(uint locale,uint kind,char* output,int capacity);
 [DllImport("kernel32.dll",ExactSpelling=true)]internal static extern uint GetACP();
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]internal static extern bool GetCPInfo(uint codePage,out CpInfo info);
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)][return:MarshalAs(UnmanagedType.Bool)]internal static extern bool GetNLSVersion(int function,uint locale,ref NlsVersion version);
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)]internal static extern int MultiByteToWideChar(uint codePage,uint flags,byte* input,int inputCount,char* output,int capacity);
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)]internal static extern int LCMapStringA(uint locale,uint flags,byte* input,int inputCount,byte* output,int capacity);
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)]internal static extern int CompareStringA(uint locale,uint flags,byte* first,int firstCount,byte* second,int secondCount);
 [DllImport("kernel32.dll",ExactSpelling=true,SetLastError=true)]internal static extern int CompareStringW(uint locale,uint flags,char* first,int firstCount,char* second,int secondCount);
}
