using System.Runtime.InteropServices;
using JewelDesign.Geometry.Candidate;
if(!OperatingSystem.IsWindows()||args.Length!=1)throw new ArgumentException("Supply the D:-local Windows candidate DLL path.");
var library=Path.GetFullPath(args[0]);if(!File.Exists(library))throw new FileNotFoundException("Build the actual native candidate test DLL first.",library);
// Keep the test module loaded for process lifetime, including SafeHandle finalizers.
var module=NativeLibrary.Load(library);
NativeLibrary.SetDllImportResolver(typeof(CandidateContext).Assembly,(name,assembly,searchPath)=>name=="__Internal"?module:IntPtr.Zero);
NativeLibrary.SetDllImportResolver(typeof(CandidateGeometryContractChecks.Assertions).Assembly,(name,assembly,searchPath)=>name=="__Internal"?module:IntPtr.Zero);
Console.WriteLine(CandidateGeometryContractChecks.Assertions.Run());
