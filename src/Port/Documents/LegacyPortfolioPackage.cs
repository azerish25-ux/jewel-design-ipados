namespace JewelDesign.Documents;

public sealed record LegacyPortfolioPackageLimits(int MaxMembers=10000,int MaxMemberBytes=128*1024*1024,long MaxTotalBytes=512L*1024*1024);

/// <summary>Owned immutable bytes and supplied source timestamp; no filesystem access.</summary>
public sealed class LegacyPortfolioMember
{
    private readonly byte[] bytes;
    public string RelativePath { get; }
    public int ByteLength=>bytes.Length;
    public DateTimeOffset? LastWriteTimeUtc { get; }
    internal ReadOnlySpan<byte> Data=>bytes;
    public LegacyPortfolioMember(string relativePath,ReadOnlySpan<byte> data,DateTimeOffset? lastWriteTimeUtc=null)
    {RelativePath=Normalize(relativePath);bytes=data.ToArray();LastWriteTimeUtc=lastWriteTimeUtc;}
    public byte[] CopyBytes()=>(byte[])bytes.Clone();
    internal static string Normalize(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);var normalized=path.Replace('\\','/');
        if(normalized.StartsWith('/')||normalized.Contains(':')||normalized.Contains('\0')||normalized.Split('/').Any(p=>p.Length==0||p=="."||p==".."))
            throw new ArgumentException("Expected a bounded relative member path",nameof(path));
        return normalized;
    }
}

public enum LegacyPortfolioIssueKind { UnpairedDesignFile,NoMetadata,MissingDesignMember,AmbiguousMetadata,MetadataGuidMismatch,InvalidMetadata }
public sealed record LegacyPortfolioIssue(LegacyPortfolioIssueKind Kind,string MemberPath,int? MetadataOrdinal=null);

public sealed class LegacyPortfolioDesignFile
{
    public string MemberPath { get; }
    public LegacyDesignFileKey Key { get; }
    public IReadOnlyList<int> MetadataOrdinals { get; }
    public string? BasePreviewMember { get; }
    public string? FavoritePreviewMember { get; }
    public string? SearchFavoritePreviewMember { get; }
    internal LegacyPortfolioDesignFile(string member,LegacyDesignFileKey key,int[] metadata,string? basePreview,string? favorite,string? searchFavorite)
    {MemberPath=member;Key=key;MetadataOrdinals=Array.AsReadOnly(metadata);BasePreviewMember=basePreview;FavoritePreviewMember=favorite;SearchFavoritePreviewMember=searchFavorite;}
}

/// <summary>Owner-bound scalar archive-data editor; not a regenerated design/application save.</summary>
public sealed class LegacyPortfolioArchiveEdit
{
    internal LegacyPortfolioPackage Owner { get; }
    public string MemberPath { get; }
    public LegacyCsg4EditSession Session { get; }
    internal LegacyPortfolioArchiveEdit(LegacyPortfolioPackage owner,string path,LegacyCsg4EditSession session) {Owner=owner;MemberPath=path;Session=session;}
}

/// <summary>Immutable folder inventory. Matching is diagnostic; no native path relocation or dropped metadata.</summary>
public sealed class LegacyPortfolioPackage
{
    private readonly Dictionary<string,LegacyPortfolioMember> index=new(StringComparer.OrdinalIgnoreCase);
    private readonly LegacyPortfolioPackageLimits limits;
    public string ManifestPath { get; }
    public LegacyPortfolioXmlDocument Manifest { get; }
    public IReadOnlyList<LegacyPortfolioMember> Members { get; }
    public IReadOnlyList<LegacyPortfolioDesignFile> Designs { get; }
    public IReadOnlyList<LegacyPortfolioIssue> Diagnostics { get; }

    private LegacyPortfolioPackage(string manifest,LegacyPortfolioMember[] members,LegacyPortfolioPackageLimits limits)
    {
        this.limits=limits;foreach(var member in members)index.Add(member.RelativePath,member);
        if(!index.TryGetValue(manifest,out var manifestMember))throw new InvalidDataException("Chosen portfolio manifest is missing");
        ManifestPath=manifestMember.RelativePath;Manifest=LegacyPortfolioXmlCodec.Read(manifestMember.Data);Members=Array.AsReadOnly(members);
        var issues=new List<LegacyPortfolioIssue>();var bindings=new List<LegacyPortfolioDesignFile>();
        var metadata=new Dictionary<string,List<int>>(StringComparer.OrdinalIgnoreCase);
        for(int i=0;i<Manifest.DesignCount;i++)
        {
            try
            {
                var stored=Manifest.ReadDesignText(i,"Path");
                if(stored is null){issues.Add(new(LegacyPortfolioIssueKind.InvalidMetadata,string.Empty,i));continue;}
                var leaf=LegacyDesignFileKey.StoredFileName(stored);
                if(!metadata.TryGetValue(leaf,out var ordinals))metadata.Add(leaf,ordinals=new List<int>());ordinals.Add(i);
            }
            catch(Exception error)when(error is InvalidDataException or NotSupportedException) {issues.Add(new(LegacyPortfolioIssueKind.InvalidMetadata,string.Empty,i));}
        }
        foreach(var member in members.Where(m=>!m.RelativePath.Contains('/')&&m.RelativePath.EndsWith(".csg",StringComparison.OrdinalIgnoreCase)))
        {
            LegacyDesignFileKey key;try{key=LegacyDesignFileKey.Parse(member.RelativePath);}
            catch(FormatException){issues.Add(new(LegacyPortfolioIssueKind.UnpairedDesignFile,member.RelativePath));continue;}
            var ordinals=metadata.TryGetValue(member.RelativePath,out var matches)?matches.ToArray():Array.Empty<int>();
            if(ordinals.Length==0)issues.Add(new(LegacyPortfolioIssueKind.NoMetadata,member.RelativePath));
            if(ordinals.Length>1)issues.Add(new(LegacyPortfolioIssueKind.AmbiguousMetadata,member.RelativePath));
            foreach(int ordinal in ordinals)
            {
                try
                {
                    if(Manifest.ReadDesignGuid(ordinal,"BaseDesignInstance")!=key.BaseDesignInstance||Manifest.ReadDesignGuid(ordinal,"DesignInstance")!=key.DesignInstance)
                        issues.Add(new(LegacyPortfolioIssueKind.MetadataGuidMismatch,member.RelativePath,ordinal));
                }
                catch(Exception error)when(error is InvalidDataException or NotSupportedException){issues.Add(new(LegacyPortfolioIssueKind.InvalidMetadata,member.RelativePath,ordinal));}
            }
            bindings.Add(new(member.RelativePath,key,ordinals,Find(key.BasePreviewName),Find(key.FavoritePreviewName),Find(key.SearchFavoritePreviewName)));
        }
        foreach(var pair in metadata)
            if(!index.ContainsKey(pair.Key))foreach(int ordinal in pair.Value)issues.Add(new(LegacyPortfolioIssueKind.MissingDesignMember,pair.Key,ordinal));
        Designs=Array.AsReadOnly(bindings.ToArray());Diagnostics=Array.AsReadOnly(issues.ToArray());
    }

    public static LegacyPortfolioPackage Create(string manifestPath,IEnumerable<LegacyPortfolioMember> members,LegacyPortfolioPackageLimits? limits=null)
    {
        ArgumentNullException.ThrowIfNull(members);limits??=new();
        if(limits.MaxMembers<=0||limits.MaxMemberBytes<=0||limits.MaxTotalBytes<=0)throw new ArgumentOutOfRangeException(nameof(limits));
        var manifest=LegacyPortfolioMember.Normalize(manifestPath);
        if(manifest.Contains('/')||!manifest.EndsWith(".csp",StringComparison.OrdinalIgnoreCase))throw new ArgumentException("Choose a root CSP manifest",nameof(manifestPath));
        var captured=new List<LegacyPortfolioMember>();var names=new HashSet<string>(StringComparer.OrdinalIgnoreCase);long total=0;
        foreach(var member in members)
        {
            if(member is null)throw new ArgumentException("Null package member",nameof(members));
            if(captured.Count>=limits.MaxMembers||member.ByteLength>limits.MaxMemberBytes||(total+=member.ByteLength)>limits.MaxTotalBytes)throw new InvalidDataException("Portfolio inventory exceeds configured bounds");
            if(!names.Add(member.RelativePath))throw new InvalidDataException("Case-colliding/duplicate member names are ambiguous on Windows");
            captured.Add(member);
        }
        return new LegacyPortfolioPackage(manifest,captured.ToArray(),limits);
    }

    private string? Find(string path)=>index.TryGetValue(path,out var member)?member.RelativePath:null;
    private LegacyPortfolioMember Member(string path)=>index.TryGetValue(LegacyPortfolioMember.Normalize(path),out var member)?member:throw new KeyNotFoundException("Portfolio member is missing");
    public byte[] CopyMemberBytes(string path)=>Member(path).CopyBytes();
    public IReadOnlyList<LegacyPortfolioDesignFile> GetHistoryCandidates(Guid baseDesignInstance)=>Array.AsReadOnly(Designs.Where(d=>!d.Key.IsBaseDesign&&d.Key.BaseDesignInstance==baseDesignInstance).ToArray());

    public LegacyPortfolioArchiveEdit OpenDesign(string path,LegacyCsg4Limits? readLimits=null)
    {
        var member=Member(path);
        if(!Designs.Any(d=>StringComparer.OrdinalIgnoreCase.Equals(d.MemberPath,member.RelativePath)))throw new NotSupportedException("Member is not a paired root design archive");
        return new LegacyPortfolioArchiveEdit(this,member.RelativePath,new LegacyCsg4EditSession(LegacyCsg4Codec.Read(member.Data,readLimits)));
    }
    public LegacyPortfolioPackage WithDesignArchiveData(LegacyPortfolioArchiveEdit edit)
    {
        ArgumentNullException.ThrowIfNull(edit);
        if(!ReferenceEquals(edit.Owner,this))throw new ArgumentException("Archive editor belongs to a foreign or stale portfolio snapshot",nameof(edit));
        return Replace(edit.MemberPath,edit.Session.ExportArchiveData());
    }
    public LegacyPortfolioPackage WithProjectMetadata(LegacyPortfolioXmlDocument metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        if(!Manifest.HasSameOrigin(metadata))throw new ArgumentException("Metadata belongs to a foreign or stale manifest snapshot",nameof(metadata));
        return Replace(ManifestPath,LegacyPortfolioXmlCodec.Write(metadata));
    }
    private LegacyPortfolioPackage Replace(string path,byte[] data)
    {
        var original=Member(path);
        var replacement=new LegacyPortfolioMember(original.RelativePath,data,original.LastWriteTimeUtc);
        return Create(ManifestPath,Members.Select(m=>ReferenceEquals(m,original)?replacement:m),limits);
    }
}
