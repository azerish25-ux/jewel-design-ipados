using System.Security.Cryptography;

namespace JewelDesign.Rendering.Candidate;

/// <summary>Explicit recorded Microsoft CRT100 locales; this is not a device/current-locale selector.</summary>
public enum Rhino5Crt100Locale { C, EnglishUnitedStates1252, Turkish1254 }

/// <summary>
/// Offline value operations for three recorded MSVCR100 10.00.40219.473 profiles.
/// Whole lower processes supplementary pairs; comparison folds individual UTF16 units.
/// Both stop at C terminators. No device culture, normalization or Windows execution is used.
/// Other locales and original runtime/failure/state selection remain separate requirements.
/// </summary>
public sealed class Rhino5Crt100FilenameProfile
{
    public const string RuntimeSha256 = "2b7ab898b2f64f4cd1ae651c93d7eb9556208fd8d8d40d425e856c83d86d2346";
    public const string DataSha256 = "7aad75e02af1cab78bc7fc4d9a71bcec227a1fbc116cb0069a81e817691133cd";
    private static readonly Rhino5Crt100FilenameProfile[] Profiles = Load();
    private readonly ushort[] compareUnits, lowerUnits;
    private readonly uint[] scalarInputs, scalarOutputs;
    public Rhino5Crt100Locale Locale { get; }
    public string LocaleName { get; }
    public Rhino5FilenameSemantics FilenameSemantics { get; }

    private Rhino5Crt100FilenameProfile(Rhino5Crt100Locale locale, ushort[] compare, ushort[] lower, uint[] inputs, uint[] outputs)
    {
        Locale = locale;
        LocaleName = locale switch { Rhino5Crt100Locale.C => "C", Rhino5Crt100Locale.EnglishUnitedStates1252 => "English_United States.1252", Rhino5Crt100Locale.Turkish1254 => "Turkish", _ => throw new InvalidDataException("Unrecorded CRT profile.") };
        compareUnits = compare; lowerUnits = lower; scalarInputs = inputs; scalarOutputs = outputs;
        FilenameSemantics = new("MSVCR100/10.00.40219.473/recorded/" + LocaleName, name => Lower(name), (left, right) =>
        {
            // Original ON_wString guard observes stored length before its terminated native comparison.
            if (string.IsNullOrEmpty(right) || right[0] == 0) return string.IsNullOrEmpty(left) ? 0 : 1;
            if (string.IsNullOrEmpty(left)) return -1;
            return CompareRaw(left, right);
        });
    }

    public static Rhino5Crt100FilenameProfile Get(Rhino5Crt100Locale locale)
    {
        if ((uint)locale >= (uint)Profiles.Length) throw new ArgumentOutOfRangeException(nameof(locale));
        return Profiles[(int)locale];
    }

    /// <summary>Preserves stored UTF16 length and every unit after the first NUL.</summary>
    public string Lower(string source, int maximumUtf16Units = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf16Units);
        if (source.Length > maximumUtf16Units) throw new InvalidDataException("CRT lower source exceeds its explicit UTF16 budget.");
        char[] destination = source.ToCharArray();
        for (int i = 0; i < source.Length && source[i] != 0; i++)
        {
            if (char.IsHighSurrogate(source[i]) && i + 1 < source.Length && char.IsLowSurrogate(source[i + 1]))
            {
                uint scalar = 0x10000u + (((uint)source[i] - 0xd800) << 10) + (uint)source[i + 1] - 0xdc00;
                int found = Array.BinarySearch(scalarInputs, scalar);
                if (found >= 0)
                {
                    uint mapped = scalarOutputs[found] - 0x10000;
                    destination[i] = (char)(0xd800 + (mapped >> 10));
                    destination[i + 1] = (char)(0xdc00 + (mapped & 1023));
                }
                i++;
            }
            else destination[i] = (char)lowerUnits[source[i]];
        }
        return new(destination);
    }

    /// <summary>Raw _wcsicmp value behavior: retains the signed unit difference, without the stored-length wrapper.</summary>
    public int CompareRaw(string first, string second, int maximumUtf16Units = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(first); ArgumentNullException.ThrowIfNull(second);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf16Units);
        if (first.Length > maximumUtf16Units || second.Length > maximumUtf16Units) throw new InvalidDataException("CRT compare input exceeds its explicit UTF16 budget.");
        for (int i = 0; ; i++)
        {
            ushort a = compareUnits[i < first.Length ? first[i] : 0], b = compareUnits[i < second.Length ? second[i] : 0];
            if (a != b) return a - b;
            if (a == 0) return 0;
        }
    }

    private static Rhino5Crt100FilenameProfile[] Load()
    {
        using var stream = typeof(Rhino5Crt100FilenameProfile).Assembly.GetManifestResourceStream("Crt100Profiles40219") ?? throw new InvalidDataException("Recorded CRT profiles are missing.");
        using var buffer = new MemoryStream(); stream.CopyTo(buffer); byte[] bytes = buffer.ToArray();
        if (bytes.Length != 16476 || Convert.ToHexStringLower(SHA256.HashData(bytes)) != DataSha256) throw new InvalidDataException("Recorded CRT profile identity changed.");
        using var input = new MemoryStream(bytes, false); using var reader = new BinaryReader(input);
        if (!reader.ReadBytes(8).AsSpan().SequenceEqual("JDCRT100"u8) || reader.ReadUInt32() != 1 || reader.ReadUInt32() != 3) throw new InvalidDataException("Unsupported CRT profile format.");
        var profiles = new Rhino5Crt100FilenameProfile[3];
        for (int index = 0; index < profiles.Length; index++)
        {
            uint id = reader.ReadUInt32(), compareCount = reader.ReadUInt32(), lowerCount = reader.ReadUInt32(), scalarCount = reader.ReadUInt32();
            if (id != index || compareCount > 65536 || lowerCount > 65536 || scalarCount > 1048576) throw new InvalidDataException("Invalid recorded CRT profile counts.");
            ushort[] compare = ReadUnits(reader, compareCount), lower = ReadUnits(reader, lowerCount);
            uint[] inputs = new uint[scalarCount], outputs = new uint[scalarCount];
            for (int i = 0; i < inputs.Length; i++)
            {
                uint key = reader.ReadUInt32(), value = reader.ReadUInt32();
                if (key < 0x10000 || key > 0x10ffff || value < 0x10000 || value > 0x10ffff || key == value || i > 0 && key <= inputs[i - 1]) throw new InvalidDataException("Invalid CRT supplementary mapping.");
                inputs[i] = key; outputs[i] = value;
            }
            profiles[index] = new((Rhino5Crt100Locale)index, compare, lower, inputs, outputs);
        }
        if (input.Position != input.Length) throw new InvalidDataException("Trailing CRT profile bytes.");
        return profiles;
    }

    private static ushort[] ReadUnits(BinaryReader reader, uint count)
    {
        ushort[] units = new ushort[65536]; for (int i = 0; i < units.Length; i++) units[i] = (ushort)i;
        int previous = -1;
        for (uint i = 0; i < count; i++)
        {
            ushort key = reader.ReadUInt16(), value = reader.ReadUInt16();
            if (key <= previous || key == value || key is >= 0xd800 and <= 0xdfff || key == 0 || value == 0) throw new InvalidDataException("Invalid CRT unit mapping.");
            units[key] = value; previous = key;
        }
        return units;
    }
}
