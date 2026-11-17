namespace JewelDesign.Rendering.Candidate;

public enum Rhino5StringRuntimeMode { Crt, UnicodeNls, AnsiNls }
/// <summary>Independent flag observations; this does not promise an atomic multi-field snapshot.</summary>
public readonly record struct Rhino5StringRuntimeState(uint LocaleFlag, uint UnicodeFlag, Rhino5Crt100Locale RecordedCrtLocale);

/// <summary>
/// Application-owned value dispatcher for the original zero-filled string globals and locale setter.
/// The owner supplies its recorded CRT fallback explicitly; selecting a core locale does not change it.
/// Flags publish in original order. Coherent application transitions remain the caller's responsibility.
/// Original races, arbitrary/thread-local CRT locales, native failure/errno/log callbacks and document adoption are separate work.
/// </summary>
public sealed class Rhino5StringRuntimeProfile
{
    private uint localeFlag, unicodeFlag;
    private Rhino5Crt100FilenameProfile recordedCrt;
    public Rhino5FilenameSemantics FilenameSemantics { get; }

    public Rhino5StringRuntimeProfile(Rhino5Crt100FilenameProfile recordedCrtFallback)
    {
        ArgumentNullException.ThrowIfNull(recordedCrtFallback);
        recordedCrt = recordedCrtFallback;
        FilenameSemantics = new("original-mutable-string-runtime/NLS1-60502/explicit-recorded-CRT", value => Lower(value), CompareStored);
    }

    /// <summary>The original getter returns a Boolean0/1 flag, not the requested LCID.</summary>
    public uint WindowsLocaleId => Volatile.Read(ref localeFlag);
    public Rhino5Crt100FilenameProfile RecordedCrtFallback => Volatile.Read(ref recordedCrt);
    public Rhino5StringRuntimeState State => new(WindowsLocaleId, Volatile.Read(ref unicodeFlag), RecordedCrtFallback.Locale);
    public Rhino5StringRuntimeMode Mode
    {
        get
        {
            if (WindowsLocaleId == 0) return Rhino5StringRuntimeMode.Crt;
            return Volatile.Read(ref unicodeFlag) != 0 ? Rhino5StringRuntimeMode.UnicodeNls : Rhino5StringRuntimeMode.AnsiNls;
        }
    }

    /// <summary>Returns previous locale flag; writes Unicode selection before the coerced locale flag.</summary>
    public uint SetWindowsLocaleId(uint requestedLocale, uint useAnsi)
    {
        uint previous = WindowsLocaleId;
        Volatile.Write(ref unicodeFlag, useAnsi == 0 ? 1u : 0u);
        Volatile.Write(ref localeFlag, requestedLocale != 0 ? 1u : 0u);
        return previous;
    }

    /// <summary>Explicit host-model configuration, not an emulation of CRT setlocale or a side effect of the core setter.</summary>
    public Rhino5Crt100FilenameProfile SetRecordedCrtFallback(Rhino5Crt100FilenameProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return Interlocked.Exchange(ref recordedCrt, profile);
    }

    public string Lower(string source, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(source); Budget(source, maximumUtf16Units, maximumUtf8Bytes);
        if (source.Length == 0) return source;
        return Mode switch
        {
            Rhino5StringRuntimeMode.Crt => RecordedCrtFallback.Lower(source, maximumUtf16Units),
            Rhino5StringRuntimeMode.UnicodeNls => Rhino5UnicodeNlsLowercase.Reference60502.Lower(source, maximumUtf16Units),
            _ => Rhino5AnsiNlsProfile.Reference60502.Lower(source, RecordedCrtFallback, maximumUtf16Units, maximumUtf8Bytes)
        };
    }

    /// <summary>Original on_wcsicmp pointer guards and selected value operations; CRT magnitudes remain intact.</summary>
    public int CompareRaw(string? first, string? second, int maximumUtf16Units = int.MaxValue, int maximumUtf8Bytes = int.MaxValue)
    {
        Budget(first, maximumUtf16Units, maximumUtf8Bytes); Budget(second, maximumUtf16Units, maximumUtf8Bytes);
        if (first is null) return second is null ? 0 : -1;
        if (second is null) return 1;
        return Mode switch
        {
            Rhino5StringRuntimeMode.Crt => RecordedCrtFallback.CompareRaw(first, second, maximumUtf16Units),
            Rhino5StringRuntimeMode.UnicodeNls => Rhino5NlsDirectComparer.Reference60502.Compare(first, second, maximumUtf16Units),
            _ => Rhino5AnsiNlsProfile.Reference60502.CompareRaw(first, second, RecordedCrtFallback, maximumUtf16Units, maximumUtf8Bytes)
        };
    }

    private int CompareStored(string? first, string? second)
    {
        if (string.IsNullOrEmpty(second) || second[0] == 0) return string.IsNullOrEmpty(first) ? 0 : 1;
        if (string.IsNullOrEmpty(first)) return -1;
        return CompareRaw(first, second);
    }
    private static void Budget(string? value, int maximumUtf16Units, int maximumUtf8Bytes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf16Units); ArgumentOutOfRangeException.ThrowIfNegative(maximumUtf8Bytes);
        if (value is not null && value.Length > maximumUtf16Units) throw new InvalidDataException("Runtime string input exceeds its explicit UTF16 budget.");
    }
}
