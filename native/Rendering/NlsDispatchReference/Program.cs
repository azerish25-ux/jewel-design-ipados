using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64) throw new PlatformNotSupportedException("Pinned dispatch symbols require Windows x64.");
string root = Path.GetFullPath(Environment.CurrentDirectory), output = Path.GetFullPath(args.Single());
if (!root.StartsWith("D:" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || !output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("D output required.");
int result = Native.CompareStringW(1, 0x20001, "a\u0300", -1, "\u0300a", -1);
nint kernel = NativeLibrary.Load("kernel32.dll");
var entries = new List<object>();
foreach (string name in new[] { "CompareStringW", "CompareStringEx", "LCMapStringW", "LCMapStringEx" })
{
    nint function = NativeLibrary.GetExport(kernel, name);
    for (int depth = 0; depth < 8; depth++)
    {
    if (!Native.GetModuleHandleExW(6, function, out nint module)) throw new Exception("Export module lookup failed.");
    char[] filename = new char[32768];
    uint count = Native.GetModuleFileNameW(module, filename, (uint)filename.Length);
    string path = new(filename, 0, (int)count);
    long rva = function - module;
    byte[] code = new byte[64];
    Marshal.Copy(function, code, 0, code.Length);
    entries.Add(new { Name = name, Depth = depth, Module = System.IO.Path.GetFileName(path), FileName = path, Rva = rva, First64Hex = Convert.ToHexStringLower(code), OnDiskSha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path))) });
    if (code[0] == 0x48 && code[1] == 0xff && code[2] == 0x25) function = Marshal.ReadIntPtr(function + 7 + BitConverter.ToInt32(code, 3));
    else if (code[0] == 0xff && code[1] == 0x25) function = Marshal.ReadIntPtr(function + 6 + BitConverter.ToInt32(code, 2));
    else if (code[0] == 0xe9) function += 5 + BitConverter.ToInt32(code, 1);
    else break;
    }
}
nint kb = Native.GetModuleHandleW("kernelbase.dll");
if (Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, "kernelbase.dll")))) != "5c29b73622ce0976e1ada566f31f2197ba0f8beaea89c3ec033366126172c3d0" ||
    Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(Path.Combine(Environment.SystemDirectory, "kernel32.dll")))) != "e132eba4166256e2f414b91fe7d5386590bc5ea8626aee5af9c9738383cf2ae1") throw new InvalidDataException("NLS symbol offsets require the pinned system image identities.");
nint k32 = Native.GetModuleHandleW("kernel32.dll");
nint weightAddress = Marshal.ReadIntPtr(k32 + 0xc23a8), expansionAddress = Marshal.ReadIntPtr(k32 + 0xc23b8);
if (!Readable(weightAddress, 65536 * 4) || !Readable(expansionAddress, 730 * 4)) throw new Exception("Active native table spans are not readable.");
byte[] weights = new byte[65536 * 4], expansions = new byte[730 * 4];
Marshal.Copy(weightAddress, weights, 0, weights.Length); Marshal.Copy(expansionAddress, expansions, 0, expansions.Length);
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
File.WriteAllBytes(output + ".weights.bin", weights); File.WriteAllBytes(output + ".expansions.bin", expansions);
var inputs = new { CharacterUnits = 65536, ExpansionPairs = 730, WeightSha256 = Convert.ToHexStringLower(SHA256.HashData(weights)), ExpansionSha256 = Convert.ToHexStringLower(SHA256.HashData(expansions)) };

var globals = new List<object>();
var objects = new HashSet<nint>();
foreach (var item in new[] { ("sortHash", 3859488), ("sortGuidHash", 3860512) })
{
    nint pointer = kb + item.Item2;
    var candidates = new List<object>();
    if (Readable(pointer, 1024))
        for (int i = 0; i < 128; i++)
        {
            nint candidate = Marshal.ReadIntPtr(pointer + i * 8);
            if (!Readable(candidate, 0x190) || !objects.Add(candidate)) continue;
            nint callback = Marshal.ReadIntPtr(candidate + 0xf0);
            object? owner = ImageOwner(callback);
            if (owner is not null) candidates.Add(new { Index = i, Offset = 0xf0, Callback = owner, Field130Null = Marshal.ReadIntPtr(candidate + 0x130) == 0, Field138Null = Marshal.ReadIntPtr(candidate + 0x138) == 0, Flags140 = (uint)Marshal.ReadInt32(candidate + 0x140), Field150Null = Marshal.ReadIntPtr(candidate + 0x150) == 0 });
        }
    globals.Add(new { Name = item.Item1, Rva = item.Item2, Candidates = candidates });
}
var modules = Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(m => m.FileName.StartsWith(Environment.SystemDirectory, StringComparison.OrdinalIgnoreCase)).Select(m => m.ModuleName).Order().ToArray();
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
File.WriteAllText(output, JsonSerializer.Serialize(new { SchemaVersion = 1, Kind = "owned_self_process_public_NLS_dispatch", OriginalTargetExecuted = false, CompareResult = result, ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(), Entries = entries, ActiveInputs = inputs, NlsGlobals = globals, SystemModules = modules }));
Console.WriteLine(File.ReadAllText(output));
static bool Readable(nint address, int count)
{
    if (address == 0 || Native.VirtualQuery(address, out var info, (nuint)Marshal.SizeOf<Native.MemoryInfo>()) == 0) return false;
    return info.State == 0x1000 && (info.Protect & 0x101) == 0 && (info.Protect & 0xee) != 0 && (nuint)address + (nuint)count <= (nuint)info.BaseAddress + info.RegionSize;
}
static object? ImageOwner(nint address)
{
    if (address == 0 || Native.VirtualQuery(address, out var info, (nuint)Marshal.SizeOf<Native.MemoryInfo>()) == 0 || info.Type != 0x1000000 || (info.Protect & 0xf0) == 0 || !Readable(address, 32)) return null;
    char[] path = new char[32768];
    uint count = Native.K32GetMappedFileNameW(new nint(-1), address, path, (uint)path.Length);
    byte[] code = new byte[32]; Marshal.Copy(address, code, 0, code.Length);
    return new { MappedPath = new string(path, 0, (int)count), Rva = (long)(address - info.AllocationBase), First32Hex = Convert.ToHexStringLower(code) };
}
static class Native
{
    [StructLayout(LayoutKind.Sequential)] internal struct MemoryInfo { internal nint BaseAddress, AllocationBase; internal uint AllocationProtect; internal ushort PartitionId; internal nuint RegionSize; internal uint State, Protect, Type; }
    [DllImport("kernel32.dll", ExactSpelling = true)] internal static extern nuint VirtualQuery(nint address, out MemoryInfo info, nuint size);
    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)] internal static extern nint GetModuleHandleW(string name);
    [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Unicode)] internal static extern uint K32GetMappedFileNameW(nint process, nint address, [Out] char[] filename, uint capacity);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)] internal static extern int CompareStringW(uint locale, uint flags, string first, int firstCount, string second, int secondCount);
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetModuleHandleExW(uint flags, nint address, out nint module);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)] internal static extern uint GetModuleFileNameW(nint module, [Out] char[] filename, uint capacity);
}
