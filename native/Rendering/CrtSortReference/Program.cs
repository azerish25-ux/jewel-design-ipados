using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
try {
 if(!OperatingSystem.IsWindows()||RuntimeInformation.ProcessArchitecture!=Architecture.X64||args.Length!=2)throw new PlatformNotSupportedException();
 string root=Path.GetFullPath(Environment.CurrentDirectory),dll=Path.GetFullPath(args[0]),output=Path.GetFullPath(args[1]);
 if(!root.StartsWith("D:"+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!dll.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||!output.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||File.Exists(output))throw new InvalidOperationException("Fresh D repository output and pinned Microsoft runtime required");
 string dllHash=Hash(File.ReadAllBytes(dll));if(dllHash!="2b7ab898b2f64f4cd1ae651c93d7eb9556208fd8d8d40d425e856c83d86d2346")throw new InvalidDataException("Microsoft CRT100 identity changed");
 nint module=NativeLibrary.Load(dll);var cases=new List<object>();int comparisons=0,managedDifferences=0;uint randomState=0x51ea9b27;
 try {
  var sort=Marshal.GetDelegateForFunctionPointer<Sort>(NativeLibrary.GetExport(module,"qsort"));
  uint Next(){randomState^=randomState<<13;randomState^=randomState>>17;randomState^=randomState<<5;return randomState;}
  (int[] Ids,int Count,string TraceHash) Run(int[] keys,int magnitude){int bytes=checked(keys.Length*8);nint allocation=Marshal.AllocHGlobal(bytes+64),data=allocation+32;var trace=new List<(int,int,int)>();try{
   for(int i=0;i<bytes+64;i++)Marshal.WriteByte(allocation,i,0xa5);for(int i=0;i<keys.Length;i++)Marshal.WriteInt64(data,i*8,unchecked((long)(((ulong)(uint)keys[i]<<32)|(uint)i)));
   Compare comparer=(a,b)=>{ulong left=unchecked((ulong)Marshal.ReadInt64(a)),right=unchecked((ulong)Marshal.ReadInt64(b));int result=unchecked((int)(left>>32)).CompareTo(unchecked((int)(right>>32)))*magnitude;trace.Add((unchecked((int)(uint)left),unchecked((int)(uint)right),result));return result;};
   sort(data,(nuint)keys.Length,8,comparer);GC.KeepAlive(comparer);for(int i=0;i<32;i++)if(Marshal.ReadByte(allocation,i)!=0xa5||Marshal.ReadByte(allocation,32+bytes+i)!=0xa5)throw new InvalidDataException("Native qsort crossed array guard");
   int[] ids=new int[keys.Length];bool[] seen=new bool[keys.Length];for(int i=0;i<ids.Length;i++){ulong record=unchecked((ulong)Marshal.ReadInt64(data,i*8));int id=unchecked((int)(uint)record);if(id<0||id>=ids.Length||seen[id]||unchecked((int)(record>>32))!=keys[id])throw new InvalidDataException("Native qsort identity corruption");seen[id]=true;ids[i]=id;if(i>0&&keys[ids[i-1]]>keys[id])throw new InvalidDataException("Native qsort did not sort");}
   byte[] traceBytes=new byte[checked(trace.Count*12)];for(int i=0;i<trace.Count;i++){BinaryPrimitives.WriteInt32LittleEndian(traceBytes.AsSpan(i*12),trace[i].Item1);BinaryPrimitives.WriteInt32LittleEndian(traceBytes.AsSpan(i*12+4),trace[i].Item2);BinaryPrimitives.WriteInt32LittleEndian(traceBytes.AsSpan(i*12+8),trace[i].Item3);}return(ids,trace.Count,Hash(traceBytes));
  }finally{Marshal.FreeHGlobal(allocation);}}
  void Add(string group,int[] input,int magnitude=1){var a=Run(input,magnitude);var b=Run(input,magnitude);if(!a.Ids.AsSpan().SequenceEqual(b.Ids)||a.Count!=b.Count||a.TraceHash!=b.TraceHash)throw new InvalidDataException("Native qsort nonrepeatable");var managed=Enumerable.Range(0,input.Length).ToList();managed.Sort((x,y)=>input[x].CompareTo(input[y])*magnitude);if(!a.Ids.AsSpan().SequenceEqual(managed.ToArray()))managedDifferences++;comparisons+=a.Count;cases.Add(new{Group=group,InputKeys=input,Magnitude=magnitude,NativeIds=a.Ids,NativeCompareCount=a.Count,NativeTraceSha256=a.TraceHash,RepeatAgrees=true,GuardsIntact=true,ManagedIds=managed.ToArray()});}
  for(int length=0;length<=8;length++){int total=1;for(int i=0;i<length;i++)total*=3;for(int mask=0;mask<total;mask++){int n=mask;int[] keys=new int[length];for(int i=0;i<length;i++){keys[i]=n%3-1;n/=3;}Add("ternary-"+length,keys);}}
  int[] sizes=Enumerable.Range(0,41).Concat(new[]{63,64,65,127,128,129,255,256,257,511,512,513,1023,1024,1025,4095,4096,4097}).ToArray();
  foreach(int size in sizes){
   Add("equal",new int[size]);Add("ascending",Enumerable.Range(0,size).ToArray());Add("descending",Enumerable.Range(0,size).Reverse().ToArray());Add("alternating",Enumerable.Range(0,size).Select(i=>i%2).ToArray());Add("mod7",Enumerable.Range(0,size).Select(i=>(i*17)%7-3).ToArray());Add("organ-pipe",Enumerable.Range(0,size).Select(i=>Math.Min(i,size-1-i)).ToArray());Add("extremes",Enumerable.Range(0,size).Select(i=>i%3==0?int.MinValue:i%3==1?int.MaxValue:0).ToArray());var almost=Enumerable.Range(0,size).ToArray();if(size>2)(almost[1],almost[^2])=(almost[^2],almost[1]);Add("almost-sorted",almost);Add("random",Enumerable.Range(0,size).Select(_=>unchecked((int)Next())).ToArray());Add("random-duplicates",Enumerable.Range(0,size).Select(_=>(int)(Next()%13)-6).ToArray(),53);
  }
 }finally{NativeLibrary.Free(module);}
 Directory.CreateDirectory(Path.GetDirectoryName(output)!);using var target=new FileStream(output,FileMode.CreateNew);JsonSerializer.Serialize(target,new{SchemaVersion=1,Kind="public_microsoft_crt100_qsort_width8_reference",OriginalTargetExecuted=false,RuntimeSha256=dllHash,RuntimeFileVersion=System.Diagnostics.FileVersionInfo.GetVersionInfo(dll).FileVersion,OS=RuntimeInformation.OSDescription,RecordWidth=8,Cases=cases,CaseCount=cases.Count,NativeComparisonCount=comparisons,ManagedOrderDifferences=managedDifferences,Scope="Owned public CRT qsort on key/id records with repeated identity/order/callback trace and guards. Not original image-entry execution or complete cache/application parity."});Console.WriteLine($"PASS CRT100 qsort reference:{cases.Count} repeated cases/{comparisons} native comparisons/{managedDifferences} managed order differences");
}catch(Exception e){Console.Error.WriteLine(e.GetType().Name+": "+e.Message);Environment.ExitCode=1;}
static string Hash(byte[] bytes)=>Convert.ToHexStringLower(SHA256.HashData(bytes));
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]delegate int Compare(nint a,nint b);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]delegate void Sort(nint data,nuint count,nuint width,Compare compare);
