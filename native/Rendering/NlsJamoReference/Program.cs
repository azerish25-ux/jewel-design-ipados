using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
if (!OperatingSystem.IsWindows()) throw new PlatformNotSupportedException();
string root = Path.GetFullPath(Environment.CurrentDirectory), output = Path.GetFullPath(args.Single());
if (!root.StartsWith("D:" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || !output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("D output required.");
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
var version = ReadVersion();
int[] leading = Enumerable.Range(0x1100, 96).Concat(Enumerable.Range(0xa960, 29)).ToArray();
int[] vowels = Enumerable.Range(0x1160, 72).Concat(Enumerable.Range(0xd7b0, 23)).ToArray();
int[] trailing = Enumerable.Range(0x11a8, 88).Concat(Enumerable.Range(0xd7cb, 49)).ToArray();
char[] first = new char[4], second = new char[4];
int failures = 0, comparisons = 0;
using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
string raw = output + ".native-results.bin";
using (var stream = new FileStream(raw, FileMode.CreateNew, FileAccess.Write, FileShare.None, 262144))
{
    byte[] row = new byte[vowels.Length * trailing.Length * 4];
    for (int l = 0; l < leading.Length; l++)
    {
        int at = 0;
        for (int v = 0; v < vowels.Length; v++)
            for (int t = 0; t < trailing.Length; t++)
            {
                first[0] = (char)leading[l]; first[1] = (char)vowels[v]; first[2] = (char)trailing[t];
                for (int mode = 0; mode < 4; mode++)
                {
                    first.CopyTo(second, 0);
                    if (mode == 0) second[2] = '\0';
                    else if (mode == 1) second[2] = (char)trailing[Math.Max(0, t - 1)];
                    else if (mode == 2) second[1] = (char)vowels[Math.Max(0, v - 1)];
                    else second[0] = (char)leading[Math.Max(0, l - 1)];
                    int result = Compare(first, second); if (result == 0) failures++;
                    if (result < 0 || result > 3) throw new Exception("Native comparison result domain.");
                    row[at++] = (byte)result; comparisons++;
                }
            }
        stream.Write(row); hash.AppendData(row);
    }
}
if (version != ReadVersion()) throw new Exception("NLS version changed during acquisition.");
File.WriteAllText(output, JsonSerializer.Serialize(new { SchemaVersion = 1, Kind = "public_windows_NLS1_all_jamo_triples_not_original_runtime", OriginalTargetExecuted = false, OS = RuntimeInformation.OSDescription, Version = version, Locale = 1, Flags = 0x20001, Leading = leading, Vowels = vowels, Trailing = trailing, TripleInputs = leading.Length * vowels.Length * trailing.Length, Comparisons = comparisons, Failures = failures, Modes = new[] { "without-trailing", "previous-trailing", "previous-vowel", "previous-leading" }, NativeRecordsSha256 = Convert.ToHexStringLower(hash.GetHashAndReset()), NativeRecordsBytes = new FileInfo(raw).Length, RecordFormat = "Ascending listed leading/vowel/trailing Cartesian product;4 native CSTR bytes per triple in Modes order." }));
Console.WriteLine($"PASS native all-Jamo direct:{comparisons} comparisons/{failures} failures.");
static unsafe int Compare(char[] a, char[] b) { fixed (char* first = a) fixed (char* second = b) return Native.CompareStringW(1, 0x20001, first, -1, second, -1); }
static VersionInfo ReadVersion() { var v = new Native.NlsVersion { Size = (uint)Marshal.SizeOf<Native.NlsVersion>() }; if (v.Size != 32 || !Native.GetNLSVersion(1, 1, ref v)) throw new Exception("Native NLS version unavailable."); return new(v.Version, v.DefinedVersion, v.EffectiveId, v.CustomVersion); }
sealed record VersionInfo(uint NlsVersion, uint DefinedVersion, uint EffectiveId, Guid CustomVersion);
static class Native { [StructLayout(LayoutKind.Sequential)] internal struct NlsVersion { internal uint Size, Version, DefinedVersion, EffectiveId; internal Guid CustomVersion; }
[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetNLSVersion(int function, uint locale, ref NlsVersion version);
 [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] internal static extern unsafe int CompareStringW(uint locale, uint flags, char* first, int firstCount, char* second, int secondCount); }
