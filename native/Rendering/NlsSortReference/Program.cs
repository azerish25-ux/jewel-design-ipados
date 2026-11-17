using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

if (!OperatingSystem.IsWindows() || !BitConverter.IsLittleEndian) throw new PlatformNotSupportedException();
string root = Path.GetFullPath(Environment.CurrentDirectory), output = Path.GetFullPath(args.Single());
if (!root.StartsWith("D:" + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) || !output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("D repository output required.");
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
var version = Version();
var versionModules = SortingModules();
string rawFile = output + ".native-keys.bin";
var planes = new List<Plane>();
var unitKeys = new byte[65536][];
int failures = 0, maximumKeyBytes = 0, total = 0;
using var allHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
using (var stream = new FileStream(rawFile, FileMode.CreateNew, FileAccess.Write, FileShare.None, 262144))
{
    char[] source = new char[3];
    byte[] header = new byte[8];
    for (int plane = 0; plane <= 16; plane++)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        int planeErrors = 0;
        for (int key = plane << 16; key < (plane + 1) * 65536; key++)
        {
            if (plane == 0) { source[0] = (char)key; source[1] = '\0'; }
            else { int v = key - 0x10000; source[0] = (char)(0xd800 + (v >> 10)); source[1] = (char)(0xdc00 + (v & 1023)); source[2] = '\0'; }
            KeyResult result = Key(source);
            if (plane == 0) unitKeys[key] = result.Bytes;
            if (result.Error != 0) { failures++; planeErrors++; }
            maximumKeyBytes = Math.Max(maximumKeyBytes, result.Bytes.Length);
            BinaryPrimitives.WriteInt32LittleEndian(header, result.Bytes.Length);
            BinaryPrimitives.WriteInt32LittleEndian(header.AsSpan(4), result.Error);
            stream.Write(header); stream.Write(result.Bytes);
            hash.AppendData(header); hash.AppendData(result.Bytes); allHash.AppendData(header); allHash.AppendData(result.Bytes);
            total++;
        }
        planes.Add(new(plane, 65536, planeErrors, Convert.ToHexStringLower(hash.GetHashAndReset())));
        Console.WriteLine($"Captured NLS1 key plane{plane}:65536 cases/{planeErrors} failures.");
    }
}
string[] tokens = ["", "Ring.PNG", "ring.png", "A", "a", "AB", "A-B", "A'B", "-AB", "AB-", "A B", "A_B", "a.b", "A\\B", "A/B", "ＡＢ", "é", "e\u0301", "è", "a\u0301\u0308", "a\u0308\u0301", "ß", "ss", "ẞ", "Æ", "AE", "Œ", "oe", "I", "i", "İ", "ı", "Σ", "σ", "ς", "\u00ad", "\u200b", "\u200c", "\u200d", "\u200f", "a\0tail", "\0tail", "カナ", "ｶﾅ", "かな", "カ\u3099", "ガ", "ｶ\uff9e", "ー", "々", "ゝ", "ヽ", "ァ", "ぁ", "a\u0300", "\u0300a", "\u0300\u0301", "가", "\u1100\u1161", "\u1100\u1161\u11a8", "각", "\u1100\u1160", "\u1100\u1100\u1161", "😀", "😃", "𐐀", "𐐨", "\ud800", "\udc00", "\ud800A\udc00", "\u0378", "\uffff", "\ue000", "猫", "豈", "2", "10", "٢", "۲"];
string[] contexts = ["", "a", "A", "1", " ", "-", "'", "\u0301", "\u200d", "\0"];
var strings = new List<StringCase>();
var sourceStrings = new List<string>();
int keyErrors = 0;
void Add(string source, string group)
{
    KeyResult key = Key(Terminated(source));
    if (key.Error != 0) keyErrors++;
    strings.Add(new(group, Convert.ToBase64String(Utf16(source)), Convert.ToBase64String(key.Bytes), key.Error));
    sourceStrings.Add(source);
}
foreach (string token in tokens) Add(token, "fixed");
foreach (string token in tokens)
    foreach (string before in contexts)
        foreach (string after in contexts)
            Add(before + token + after, "context");
uint state = 0x20261117;
uint Next() { state ^= state << 13; state ^= state >> 17; state ^= state << 5; return state; }
for (int i = 0; i < 4096; i++)
{
    var source = new StringBuilder();
    for (int j = 0; j < 16; j++)
    {
        uint n = Next();
        if ((n & 3) < 2) source.Append(tokens[Next() % (uint)tokens.Length]);
        else if ((n & 3) == 2) source.Append(char.ConvertFromUtf32((int)(65536 + Next() % 0x100000)));
        else source.Append((char)Next());
    }
    Add(source.ToString(), "random");
}
var targeted = new List<(int Left, int Right)>();
(string First, string Second)[] variants = [("ß", "ss"), ("Æ", "AE"), ("Œ", "OE"), ("é", "e\u0301"), ("a\u0300", "\u0300a"), ("a\u0301\u0308", "a\u0308\u0301"), ("ＡＢ", "AB"), ("カナ", "ｶﾅ"), ("が", "か\u3099"), ("ガ", "カ\u3099"), ("カ", "か"), ("가", "\u1100\u1161"), ("각", "\u1100\u1161\u11a8"), ("𐐀", "𐐨"), ("I", "İ"), ("ß.png", "ss.png"), ("カナ.png", "ｶﾅ.png")];
foreach (var variant in variants)
    foreach (string before in contexts)
        foreach (string after in contexts)
        {
            int left = sourceStrings.Count;
            Add(before + variant.First + after, "canonical-context");
            int right = sourceStrings.Count;
            Add(before + variant.Second + after, "canonical-context");
            targeted.Add((left, right));
        }
int baselineStringCount = sourceStrings.Count;
var boundaryPairs = new List<(int Left, int Right)>();
foreach (int length in new[] { 15, 16, 17, 127, 128, 129, 255, 256, 257, 32767, 32768, 65535 })
{
    foreach (var variant in new[] {
        (new string('a', length) + "'b", new string('a', length + 1) + "b"),
        ("a" + new string('\u0301', length), "á" + new string('\u0301', length - 1)),
        (new string('\u0301', length) + "a", "a" + new string('\u0301', length)) })
    {
        int left = sourceStrings.Count;
        Add(variant.Item1, "length-boundary");
        int right = sourceStrings.Count;
        Add(variant.Item2, "length-boundary");
        boundaryPairs.Add((left, right));
    }
}
var pairs = new List<Pair>();
int compareErrors = 0, keyCompareMismatches = 0, repeatMismatches = 0;
void Compare(int left, int right)
{
    int result = CompareNative(Terminated(sourceStrings[left]), Terminated(sourceStrings[right]));
    int repeated = CompareNative(Terminated(sourceStrings[left]), Terminated(sourceStrings[right]));
    if (result != repeated) repeatMismatches++;
    if (result == 0) compareErrors++;
    byte[] a = Convert.FromBase64String(strings[left].SortKeyBase64), b = Convert.FromBase64String(strings[right].SortKeyBase64);
    int keySign = Math.Sign(a.AsSpan().SequenceCompareTo(b));
    if (result != 0 && keySign != result - 2) keyCompareMismatches++;
    pairs.Add(new(left, right, result, repeated, keySign));
}
for (int i = 0; i < tokens.Length; i++) for (int j = 0; j < tokens.Length; j++) Compare(i, j);
for (int i = 0; i < baselineStringCount; i++) { Compare(i, (i + 1) % baselineStringCount); Compare(i, (int)(Next() % (uint)baselineStringCount)); }
foreach (var pair in targeted) { Compare(pair.Left, pair.Right); Compare(pair.Right, pair.Left); }
foreach (var pair in boundaryPairs) { Compare(pair.Left, pair.Right); Compare(pair.Right, pair.Left); }
int naiveMismatches = 0;
for (int i = 0; i < sourceStrings.Count; i++)
{
    byte[]? naive = NaiveGroups(sourceStrings[i], unitKeys);
    byte[] actual = Convert.FromBase64String(strings[i].SortKeyBase64);
    if (naive is null || !naive.AsSpan().SequenceEqual(actual)) naiveMismatches++;
}
if (version != Version()) throw new InvalidOperationException("NLS version changed during acquisition.");
File.WriteAllText(output, JsonSerializer.Serialize(new
{
    SchemaVersion = 1,
    Kind = "public_windows_NLS1_sort_0x20401_compare_0x20001_not_original_runtime",
    OriginalTargetExecuted = false,
    OS = RuntimeInformation.OSDescription,
    Version = version,
    Locale = 1,
    SortFlags = 0x20401,
    CompareFlags = 0x20001,
    CTerminated = true,
    VersionQuerySortingModules = versionModules,
    FinalSortingModules = SortingModules(),
    UnitScalarCases = total,
    UnitScalarKeyErrors = failures,
    MaximumSingleKeyBytes = maximumKeyBytes,
    Planes = planes,
    Strings = strings,
    Pairs = pairs,
    StringKeyErrors = keyErrors,
    TargetedVariantPairs = targeted.Count * 2,
    LengthBoundaryPairs = boundaryPairs.Count * 2,
    CompareErrors = compareErrors,
    SortKeyCompareMismatches = keyCompareMismatches,
    NativeComparisonRepeatMismatches = repeatMismatches,
    NaiveUnitGroupCompositionMismatches = naiveMismatches,
    NativeKeyRecordsSha256 = Convert.ToHexStringLower(allHash.GetHashAndReset()),
    NativeKeyRecordsBytes = new FileInfo(rawFile).Length,
    RecordFormat = "Ascending plane0 UTF16 units then supplementary planes1..16; each record int32LE key byte count,int32LE error,exact native key bytes. Source count-1 with terminal UTF16 NUL.",
    Scope = "Native key/comparison reference only. Full offline key engine, ON_wString empty/CRT fallback, original document and PC/physical remain open."
}));
Console.WriteLine($"PASS native NLS1 sort:{total} individual keys/{strings.Count} full strings/{pairs.Count} comparisons/{keyCompareMismatches} key/compare mismatches/{naiveMismatches} rejected naive compositions.");

static object[] SortingModules() => Process.GetCurrentProcess().Modules.Cast<ProcessModule>()
    .Where(module => module.ModuleName.StartsWith("Sort", StringComparison.OrdinalIgnoreCase))
    .Select(module => (object)new { Name = module.ModuleName, FileName = module.FileName, Sha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(module.FileName))) }).ToArray();

static byte[] Utf16(string source) => MemoryMarshal.AsBytes(source.AsSpan()).ToArray();
static char[] Terminated(string source) => (source + "\0").ToCharArray();
static unsafe KeyResult Key(char[] source)
{
    byte[] bytes = new byte[source.Length * 32 + 256];
    int count;
    fixed (char* input = source)
    fixed (byte* output = bytes) count = Native.LCMapStringW(1, 0x20401, input, -1, output, bytes.Length);
    int error = count == 0 ? Marshal.GetLastPInvokeError() : 0;
    if (count < 0 || count > bytes.Length) throw new InvalidOperationException("Native key escaped capacity.");
    return new(bytes.AsSpan(0, count).ToArray(), error);
}
static unsafe int CompareNative(char[] first, char[] second)
{
    fixed (char* a = first)
    fixed (char* b = second) return Native.CompareStringW(1, 0x20001, a, -1, b, -1);
}
static VersionInfo Version()
{
    var version = new Native.NlsVersion { Size = (uint)Marshal.SizeOf<Native.NlsVersion>() };
    if (version.Size != 32 || !Native.GetNLSVersion(1, 1, ref version)) throw new Exception("Public NLS version unavailable.");
    return new(version.Version, version.DefinedVersion, version.EffectiveId, version.CustomVersion);
}
static byte[]? NaiveGroups(string source, byte[][] keys)
{
    var groups = Enumerable.Range(0, 5).Select(_ => new List<byte>()).ToArray();
    foreach (char c in source)
    {
        if (c == 0) break;
        int group = 0;
        foreach (byte value in keys[c])
        {
            if (value == 0) break;
            if (value == 1) { group++; if (group > 4) return null; }
            else groups[group].Add(value);
        }
    }
    var output = new List<byte>();
    for (int i = 0; i < 5; i++) { output.AddRange(groups[i]); output.Add(i == 4 ? (byte)0 : (byte)1); }
    return output.ToArray();
}
sealed record KeyResult(byte[] Bytes, int Error);
sealed record Plane(int Index, int Cases, int Errors, string NativeSha256);
sealed record StringCase(string Group, string InputUtf16Base64, string SortKeyBase64, int KeyError);
sealed record Pair(int Left, int Right, int NativeResult, int RepeatNativeResult, int SortKeySign);
sealed record VersionInfo(uint NlsVersion, uint DefinedVersion, uint EffectiveId, Guid CustomVersion);
static class Native
{
    [StructLayout(LayoutKind.Sequential)] internal struct NlsVersion { internal uint Size, Version, DefinedVersion, EffectiveId; internal Guid CustomVersion; }
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] internal static extern unsafe int LCMapStringW(uint locale, uint flags, char* source, int sourceCount, byte* target, int targetCount);
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] internal static extern unsafe int CompareStringW(uint locale, uint flags, char* first, int firstCount, char* second, int secondCount);
    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)] [return: MarshalAs(UnmanagedType.Bool)] internal static extern bool GetNLSVersion(int function, uint locale, ref NlsVersion version);
}
