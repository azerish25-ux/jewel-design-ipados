using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
if (!OperatingSystem.IsWindows() || !BitConverter.IsLittleEndian) throw new PlatformNotSupportedException();
string root = Path.GetFullPath(Environment.CurrentDirectory), output = Path.GetFullPath(args.Single());
if (!root.StartsWith("D:" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || !output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("D output required.");
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
var version = ReadVersion();
char[] source = new char[3], previous = new char[3], empty = ['\0'], letter = ['a', '\0'], lower = new char[19];
var planes = new List<object>();
int failures = 0, lowerFailures = 0;
using var all = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
string rawFile = output + ".native-results.bin";
using (var stream = new FileStream(rawFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, 262144))
{
    for (int plane = 0; plane < 17; plane++)
    {
        byte[] results = new byte[65536 * 4];
        for (int i = 0; i < 65536; i++)
        {
            int key = (plane << 16) + i;
            Encode(key, source); Encode(Math.Max(0, key - 1), previous);
            int count = Lower(source, plane == 0 ? 1 : 2, lower);
            if (count == 0) lowerFailures++;
            if (count < 0 || count >= lower.Length) throw new Exception("Native lower output capacity.");
            lower[count] = '\0';
            int[] values = [Compare(source, empty), Compare(source, letter), Compare(source, previous), Compare(source, lower)];
            for (int mode = 0; mode < 4; mode++) { if (values[mode] == 0) failures++; if (values[mode] < 0 || values[mode] > 3) throw new Exception("Native result domain."); results[i * 4 + mode] = (byte)values[mode]; }
        }
        stream.Write(results); all.AppendData(results);
        planes.Add(new { Index = plane, Inputs = 65536, Comparisons = 65536 * 4, NativeSha256 = Convert.ToHexStringLower(SHA256.HashData(results)) });
        Console.WriteLine($"Captured direct NLS1 plane{plane}:262144 comparisons.");
    }
}
if (version != ReadVersion()) throw new Exception("NLS version changed.");
File.WriteAllText(output, JsonSerializer.Serialize(new { SchemaVersion = 1, Kind = "public_windows_NLS1_direct_exhaustive_not_original_runtime", OriginalTargetExecuted = false, OS = RuntimeInformation.OSDescription, Version = version, Locale = 1, Flags = 0x20001, UnitScalarInputs = 1114112, Comparisons = 4456448, Modes = new[] { "empty", "ascii-a", "previous-input", "native-lower" }, CompareFailures = failures, LowerFailures = lowerFailures, Planes = planes, NativeRecordsBytes = new FileInfo(rawFile).Length, NativeRecordsSha256 = Convert.ToHexStringLower(all.GetHashAndReset()), RecordFormat = "Ascending individual BMP UTF16 units then supplementary scalars;4 native CSTR byte results per input, ordered by Modes." }));
Console.WriteLine($"PASS exhaustive direct NLS1:4456448 comparisons/{failures} failures/{lowerFailures} lower failures.");
static void Encode(int key, char[] target) { if (key <= 65535) { target[0] = (char)key; target[1] = '\0'; } else { int v = key - 65536; target[0] = (char)(0xd800 + (v >> 10)); target[1] = (char)(0xdc00 + (v & 1023)); } target[2] = '\0'; }
static unsafe int Compare(char[] a, char[] b) { fixed (char* first = a) fixed (char* second = b) return Native.CompareStringW(1, 0x20001, first, -1, second, -1); }
static unsafe int Lower(char[] input, int length, char[] output) { fixed (char* source = input) fixed (char* target = output) return Native.LCMapStringW(1, 0x100, source, length, target, output.Length); }
static VersionInfo ReadVersion() { var v = new Native.NlsVersion { Size = (uint)Marshal.SizeOf<Native.NlsVersion>() }; if (v.Size != 32 || !Native.GetNLSVersion(1, 1, ref v)) throw new Exception("NLS version unavailable."); return new(v.Version, v.DefinedVersion, v.EffectiveId, v.CustomVersion); }
sealed record VersionInfo(uint NlsVersion, uint DefinedVersion, uint EffectiveId, Guid CustomVersion);
static class Native
{
    [StructLayout(LayoutKind.Sequential)] internal struct NlsVersion { internal uint Size, Version, DefinedVersion, EffectiveId; internal Guid CustomVersion; }
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] internal static extern unsafe int CompareStringW(uint locale, uint flags, char* first, int firstCount, char* second, int secondCount);
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] internal static extern unsafe int LCMapStringW(uint locale, uint flags, char* source, int sourceCount, char* output, int outputCount);
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetNLSVersion(int function, uint locale, ref NlsVersion version);
}
