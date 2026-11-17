using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text.Json;
if (!OperatingSystem.IsWindows()) throw new PlatformNotSupportedException();
string root = Path.GetFullPath(Environment.CurrentDirectory), input = Path.GetFullPath(args[0]), output = Path.GetFullPath(args[1]);
if (!root.StartsWith("D:" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || !input.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || !output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("D input/output required.");
var version = ReadVersion();
using var fixture = JsonDocument.Parse(File.ReadAllBytes(input));
var results = new List<object>(); int failures = 0, repeats = 0;
foreach (var pair in fixture.RootElement.GetProperty("Pairs").EnumerateArray())
{
    string first = pair.GetProperty("FirstUtf16Base64").GetString()!, second = pair.GetProperty("SecondUtf16Base64").GetString()!, group = pair.GetProperty("Group").GetString()!;
    char[] a = Decode(first), b = Decode(second);
    int actual = Compare(a, b), repeated = Compare(a, b);
    if (actual == 0) failures++; if (actual != repeated) repeats++;
    if (actual < 0 || actual > 3) throw new Exception("Native result domain.");
    results.Add(new { FirstUtf16Base64 = first, SecondUtf16Base64 = second, Group = group, NativeResult = actual, RepeatNativeResult = repeated });
}
if (version != ReadVersion()) throw new Exception("NLS version changed.");
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
File.WriteAllText(output, JsonSerializer.Serialize(new { SchemaVersion = 1, OriginalTargetExecuted = false, Kind = "public_windows_direct_NLS1_string_mutations", OS = RuntimeInformation.OSDescription, Version = version, Locale = 1, Flags = 0x20001, Comparisons = results.Count, Failures = failures, RepeatMismatches = repeats, Pairs = results }));
Console.WriteLine($"PASS native NLS1 stress:{results.Count} comparisons/{failures} failures/{repeats} repeat mismatches.");
static char[] Decode(string encoded) { byte[] bytes = Convert.FromBase64String(encoded); if ((bytes.Length & 1) != 0) throw new Exception("Odd UTF16."); char[] output = new char[bytes.Length / 2 + 1]; for (int i = 0; i < output.Length - 1; i++) output[i] = (char)BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(i * 2)); return output; }
static unsafe int Compare(char[] a, char[] b) { fixed (char* first = a) fixed (char* second = b) return Native.CompareStringW(1, 0x20001, first, -1, second, -1); }
static VersionInfo ReadVersion() { var v = new Native.NlsVersion { Size = (uint)Marshal.SizeOf<Native.NlsVersion>() }; if (v.Size != 32 || !Native.GetNLSVersion(1, 1, ref v)) throw new Exception("NLS version query failed."); return new(v.Version, v.DefinedVersion, v.EffectiveId, v.CustomVersion); }
sealed record VersionInfo(uint NlsVersion, uint DefinedVersion, uint EffectiveId, Guid CustomVersion);
static class Native {
[StructLayout(LayoutKind.Sequential)] internal struct NlsVersion { internal uint Size, Version, DefinedVersion, EffectiveId; internal Guid CustomVersion; }
[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] internal static extern unsafe int CompareStringW(uint locale, uint flags, char* first, int firstCount, char* second, int secondCount);
[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetNLSVersion(int function, uint locale, ref NlsVersion version);
}
