using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

if (!OperatingSystem.IsWindows() || !BitConverter.IsLittleEndian)
    throw new PlatformNotSupportedException("Public Windows NLS reference requires little-endian Windows.");
string root = Path.GetFullPath(Environment.CurrentDirectory);
string output = Path.GetFullPath(args.Single());
if (!root.StartsWith("D:" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
    !output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
    throw new InvalidOperationException("Reference output must remain in the D repository.");
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
var initialVersion = ReadVersion();
var mappings = new List<Mapping>();
var planes = new List<Plane>();
var sparse = new Dictionary<int, char[]>();
int portableDifferences = 0;
int errors = 0;
int total = 0;
string rawFile = output + ".native-records.bin";
using var allHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
using (var raw = new FileStream(rawFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, 262144))
{
    char[] source = new char[2];
    for (int plane = 0; plane <= 16; plane++)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        int first = plane == 0 ? 0 : plane << 16;
        int changed = 0;
        int failed = 0;
        for (int key = first; key < first + 65536; key++)
        {
            int length;
            if (plane == 0) { source[0] = (char)key; length = 1; }
            else { int v = key - 0x10000; source[0] = (char)(0xd800 + (v >> 10)); source[1] = (char)(0xdc00 + (v & 1023)); length = 2; }
            NativeResult result = Map(source, length);
            byte[] header = new byte[8];
            BinaryPrimitives.WriteInt32LittleEndian(header, result.Count);
            BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(4), result.Error);
            byte[] bytes = Utf16(result.Value);
            foreach (var target in new[] { hash, allHash }) { target.AppendData(header); target.AppendData(bytes); }
            raw.Write(header); raw.Write(bytes);
            if (result.Count == 0) { failed++; errors++; }
            if (!source.AsSpan(0, length).SequenceEqual(result.Value) || result.Count == 0)
            {
                mappings.Add(new(key, Convert.ToBase64String(bytes), result.Count, result.Error));
                sparse.Add(key, result.Value);
                changed++;
            }
            string portable = plane == 0 ? char.ToLowerInvariant(source[0]).ToString() : Rune.ToLowerInvariant(new Rune(key)).ToString();
            if (!portable.AsSpan().SequenceEqual(result.Value)) portableDifferences++;
            total++;
        }
        planes.Add(new(plane, 65536, changed, failed, Convert.ToHexString(hash.GetHashAndReset()).ToLowerInvariant()));
        Console.WriteLine($"Captured NLS1 plane{plane}:65536 mappings/{changed} changes/{failed} failures.");
    }
}
var controls = new List<Control>();
string[] tokens = ["I", "İ", "I\u0307", "SS", "ß", "ẞ", "Σ", "ΟΣ", "ΟΣΟΣ", "ΑΣ\u0301", "ᾈ", "\u212a", "ＡＢＣ", "𐐀𐐁", "𐒰", "\ud800", "\udc00", "A\0B", "İ\0Σ", "\u0345\u0300"];
string[] contexts = ["", "A", "a", "1", " ", ".", "\0", "\u0301"];
int contextMismatches = 0;
void Capture(char[] input, string group)
{
    NativeResult result = input.Length == 0 ? new([], 0, 0) : Map(input, input.Length);
    char[] composed = Compose(input, sparse);
    if (!composed.AsSpan().SequenceEqual(result.Value)) contextMismatches++;
    controls.Add(new(group, Convert.ToBase64String(Utf16(input)), Convert.ToBase64String(Utf16(result.Value)), result.Count, result.Error, input.Length != 0));
}
foreach (string token in tokens)
    foreach (string before in contexts)
        foreach (string after in contexts)
            Capture((before + token + after).ToCharArray(), "context");
Capture([], "original-empty-skip");
uint state = 0x20261116;
uint Next() { state ^= state << 13; state ^= state >> 17; state ^= state << 5; return state; }
for (int i = 0; i < 2048; i++)
{
    var text = new StringBuilder();
    for (int j = 0; j < 32; j++)
    {
        uint choice = Next();
        if ((choice & 3) == 0) text.Append(tokens[Next() % (uint)tokens.Length]);
        else if ((choice & 3) == 1) text.Append(char.ConvertFromUtf32((int)(0x10000 + Next() % 0x100000)));
        else text.Append((char)Next());
    }
    Capture(text.ToString().ToCharArray(), "deterministic-random");
}
var finalVersion = ReadVersion();
if (initialVersion != finalVersion) throw new InvalidOperationException("NLS version changed during acquisition.");
var report = new
{
    SchemaVersion = 1,
    Kind = "public_windows_NLS1_lower_0x100_not_original_runtime",
    OriginalTargetExecuted = false,
    OS = RuntimeInformation.OSDescription,
    Version = initialVersion,
    Locale = 1,
    Flags = 0x100,
    IndividualUtf16Units = 65536,
    SupplementaryScalarPairs = 1048576,
    Total = total,
    Errors = errors,
    Planes = planes,
    Mappings = mappings,
    Controls = controls,
    ContextCompositionMismatches = contextMismatches,
    PortableInvariantDifferences = portableDifferences,
    NativeRecordsSha256 = Convert.ToHexString(allHash.GetHashAndReset()).ToLowerInvariant(),
    NativeRecordsBytes = new FileInfo(rawFile).Length,
    RecordFormat = "Ascending plane0 UTF16 units then planes1..16 valid scalar pairs; each record int32LE count,int32LE error,exact native output UTF16LE.",
    Scope = "Complete individual-unit/scalar mapping for recorded Windows NLS version; explicit-length context controls. Original CRT/ANSI fallback and complete NLS comparison remain separate."
};
File.WriteAllText(output, JsonSerializer.Serialize(report));
Console.WriteLine($"PASS native NLS1 lower:{total} unit/scalar cases/{controls.Count} contexts/{mappings.Count} changed mappings/{contextMismatches} context-composition mismatches/{portableDifferences} invariant differences.");

static byte[] Utf16(char[] text) => MemoryMarshal.AsBytes(text.AsSpan()).ToArray();
static unsafe NativeResult Map(char[] input, int length)
{
    char[] output = new char[length + 16];
    int count;
    Marshal.SetLastPInvokeError(0);
    fixed (char* source = input)
    fixed (char* destination = output)
        count = Native.LCMapStringW(1, 0x100, source, length, destination, output.Length);
    int error = count == 0 ? Marshal.GetLastPInvokeError() : 0;
    if (count < 0 || count > output.Length) throw new InvalidOperationException("Native output escaped supplied capacity.");
    return new(output.AsSpan(0, count).ToArray(), count, error);
}
static VersionInfo ReadVersion()
{
    var info = new Native.NlsVersion { Size = (uint)Marshal.SizeOf<Native.NlsVersion>() };
    if (info.Size != 32 || !Native.GetNLSVersion(1, 1, ref info)) throw new InvalidOperationException("NLS version query failed: " + Marshal.GetLastPInvokeError());
    return new(info.Version, info.DefinedVersion, info.EffectiveId, info.CustomVersion);
}
static char[] Compose(char[] source, Dictionary<int, char[]> mappings)
{
    var output = new List<char>(source.Length);
    for (int i = 0; i < source.Length; i++)
    {
        int key = source[i];
        int count = 1;
        if (char.IsHighSurrogate(source[i]) && i + 1 < source.Length && char.IsLowSurrogate(source[i + 1])) { key = char.ConvertToUtf32(source[i], source[i + 1]); count = 2; }
        if (mappings.TryGetValue(key, out char[]? mapped)) output.AddRange(mapped);
        else for (int j = 0; j < count; j++) output.Add(source[i + j]);
        i += count - 1;
    }
    return output.ToArray();
}
sealed record NativeResult(char[] Value, int Count, int Error);
sealed record Mapping(int Input, string OutputUtf16Base64, int Count, int Error);
sealed record Plane(int Index, int Cases, int Changed, int Errors, string NativeSha256);
sealed record Control(string Group, string InputUtf16Base64, string OutputUtf16Base64, int Count, int Error, bool NativeCalled);
sealed record VersionInfo(uint NlsVersion, uint DefinedVersion, uint EffectiveId, Guid CustomVersion);
static class Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NlsVersion { internal uint Size, Version, DefinedVersion, EffectiveId; internal Guid CustomVersion; }
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    internal static extern unsafe int LCMapStringW(uint locale, uint flags, char* source, int sourceCount, char* target, int targetCount);
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetNLSVersion(int function, uint locale, ref NlsVersion version);
}
