using JewelDesign.Documents;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Views.Candidate;

/// <summary>One actual body codec and explicit source-preserving write policy, not the full native catalog.</summary>
public sealed class Rhino5UserStringArchiveBinding
{
    private const int MaxReadOrigins=1024;
    private readonly Rhino5UserDataRuntime runtime;
    private readonly LegacyUserStringListLimits limits;

    // Immutable and owned by each list's lifecycle. Sharing this state between native-style
    // copies preserves source bytes without sharing mutable list values or owner links.
    private sealed class ReadHistory
    {
        internal readonly Rhino5UserStringReadOrigin Origin;
        internal readonly ReadHistory? Previous;
        internal readonly long TotalOriginalBytes;
        internal readonly int OriginCount;
        internal ReadHistory(Rhino5UserStringReadOrigin origin,ReadHistory? previous,int bodyLength)
        {
            Origin=origin;Previous=previous;
            TotalOriginalBytes=(previous?.TotalOriginalBytes??0)+bodyLength;
            OriginCount=(previous?.OriginCount??0)+1;
        }
    }

    private sealed class SourceState : IRhino5UserStringArchiveOrigin
    {
        internal readonly ReadHistory History;
        internal readonly bool CoversEntireList;
        private readonly KeyValuePair<string,string>[] baseline;
        internal SourceState(Rhino5UserStringReadOrigin origin,SourceState? previous,
            bool coversEntireList,KeyValuePair<string,string>[] values,int bodyLength)
        {
            History=new(origin,previous?.History,bodyLength);
            CoversEntireList=coversEntireList;
            // Prior reads retain their own bytes/results, not every superseded accumulated
            // baseline. Only a single whole-body source can take the byte-preserving path.
            baseline=coversEntireList?(KeyValuePair<string,string>[])values.Clone():[];
        }
        internal bool Matches(ReadOnlySpan<KeyValuePair<string,string>> values)=>values.SequenceEqual(baseline);
    }

    private Rhino5UserStringArchiveBinding(Rhino5UserDataRuntime runtime,LegacyUserStringListLimits? limits)
    {this.runtime=runtime;this.limits=limits??new();}

    public static Rhino5UserStringArchiveBinding Register(Rhino5UserDataRuntime runtime,LegacyUserStringListLimits? limits=null)
    {
        ArgumentNullException.ThrowIfNull(runtime);
        var binding=new Rhino5UserStringArchiveBinding(runtime,limits);
        runtime.RegisterClass(Rhino5UserStringList.ClassId,r=>new Rhino5UserStringList(r),
            (data,archive)=>binding.ReadInto((Rhino5UserStringList)data,archive).NativeReadSucceeded);
        return binding;
    }

    public LegacyUserStringListReadResult ReadInto(Rhino5UserStringList target,Rhino5UserDataArchive archive)
    {
        ArgumentNullException.ThrowIfNull(archive);
        return ReadInto(target,archive.CopyBytes(),archive.ArchiveVersion,archive.OpenNurbsVersion);
    }

    public LegacyUserStringListReadResult ReadInto(Rhino5UserStringList target,ReadOnlySpan<byte> body,
        int archiveVersion,uint openNurbsVersion=0)
    {
        ArgumentNullException.ThrowIfNull(target);
        ObjectDisposedException.ThrowIf(target.IsDisposed,target);
        var previous=target.ArchiveOrigin as SourceState;
        if((previous?.History.OriginCount??0)>=MaxReadOrigins||(previous?.History.TotalOriginalBytes??0)+body.Length>limits.MaxPayloadBytes)
            throw new InvalidDataException("Retained user-string provenance exceeds the managed origin-count or total-byte budget.");
        var result=LegacyUserStringListCodec.Read(body,archiveVersion,limits);
        if(result.ManagedLimitHit)
            throw new InvalidDataException("Native user-string conversion exceeded an explicit managed bound; retain original opaque data.");
        var original=target.CopyPairs();var appended=result.CopyPairs();
        if((long)original.Length+appended.Length>limits.MaxPairs)
            throw new InvalidDataException("Appended user-string list exceeds managed bound");
        var combined=new KeyValuePair<string,string>[original.Length+appended.Length];
        original.CopyTo(combined,0);appended.CopyTo(combined,original.Length);
        var source=new SourceState(new(archiveVersion,openNurbsVersion,result),previous,
            original.Length==0&&target.ArchiveOrigin is null,combined,body.Length);
        target.ReplacePairs(combined);
        target.ArchiveOrigin=source;
        runtime.ReportArchiveCrcErrors(result.CrcErrorCount);
        return result;
    }

    public bool TryGetReadResult(Rhino5UserStringList target,out LegacyUserStringListReadResult? result)
    {
        ArgumentNullException.ThrowIfNull(target);
        result=(target.ArchiveOrigin as SourceState)?.History.Origin.Result;
        return result is not null;
    }

    /// <summary>Chronological immutable observations; the returned array is independently owned.</summary>
    public Rhino5UserStringReadOrigin[] CopyReadOrigins(Rhino5UserStringList target)
    {
        ArgumentNullException.ThrowIfNull(target);
        var origins=new List<Rhino5UserStringReadOrigin>();
        for(var node=(target.ArchiveOrigin as SourceState)?.History;node is not null;node=node.Previous)
            origins.Add(node.Origin);
        origins.Reverse();return origins.ToArray();
    }

    /// <summary>Preserves a single unchanged source body exactly. Rewrites only complete known
    /// bodies; unknown, partial, trailing or mixed-source edits require an explicit caller decision.</summary>
    public byte[] WriteForArchive(Rhino5UserStringList target,int archiveVersion)
    {
        ArgumentNullException.ThrowIfNull(target);
        ObjectDisposedException.ThrowIf(target.IsDisposed,target);
        if(target.ArchiveOrigin is not SourceState source)return WriteCanonical(target,archiveVersion);
        if(!source.CoversEntireList||source.History.Previous is not null)
            throw new InvalidDataException("Multiple or mixed source reads cannot be silently represented by one original body.");
        if(source.History.TotalOriginalBytes>limits.MaxPayloadBytes)
            throw new InvalidDataException("Original user-string bytes exceed the receiving binding byte budget.");
        var result=source.History.Origin.Result;var original=result.CopyOriginal();
        if(source.History.Origin.ArchiveVersion==archiveVersion&&source.Matches(target.CopyPairs()))return original;
        if(!result.NativeReadSucceeded||result.HasUnknownSchema||result.ConsumedBytes!=original.Length)
            throw new InvalidDataException("Editing or changing archive context would discard unsupported original user-string content.");
        return WriteCanonical(target,archiveVersion);
    }

    /// <summary>Explicit canonical1.0 rewrite, separate from source preservation.</summary>
    public byte[] WriteCanonical(Rhino5UserStringList target,int archiveVersion)
    {
        ArgumentNullException.ThrowIfNull(target);ObjectDisposedException.ThrowIf(target.IsDisposed,target);
        return LegacyUserStringListCodec.Write(target.CopyPairs(),archiveVersion,limits);
    }
}
