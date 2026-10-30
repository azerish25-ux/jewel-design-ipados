namespace JewelDesign.Documents;

/// <summary>Paired saved-design filename rules; no filesystem resolution or path rewriting.</summary>
public sealed class LegacyDesignFileKey
{
    private readonly string stem;
    public string FileName { get; }
    public Guid BaseDesignInstance { get; }
    public Guid DesignInstance { get; }
    public bool IsBaseDesign { get; }
    public string BasePreviewName=>BaseDesignInstance+"_spi.png";
    public string FavoritePreviewName=>stem+"_fav.png";
    public string SearchFavoritePreviewName=>stem+"_fav_spi.png";
    private LegacyDesignFileKey(string name,string stem,Guid baseId,Guid designId,bool isBase)
    {FileName=name;this.stem=stem;BaseDesignInstance=baseId;DesignInstance=designId;IsBaseDesign=isBase;}
    public static LegacyDesignFileKey Parse(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);
        if(fileName.IndexOfAny(new[]{'/','\\','\0'})>=0)throw new ArgumentException("Expected one filename, not a path",nameof(fileName));
        if(!fileName.EndsWith(".csg",StringComparison.OrdinalIgnoreCase))throw new FormatException("Expected .csg filename");
        var stem=fileName[..^4];var parts=stem.Split('_');
        if(parts.Length!=2||!Guid.TryParse(parts[0],out var baseId)||!Guid.TryParse(parts[1],out var designId))throw new FormatException("Expected baseGuid_designGuid.csg");
        return new LegacyDesignFileKey(fileName,stem,baseId,designId,StringComparer.Ordinal.Equals(parts[0],parts[1]));
    }
    public static string StoredFileName(string storedPath)
    {
        ArgumentNullException.ThrowIfNull(storedPath);
        int split=Math.Max(storedPath.LastIndexOf('/'),storedPath.LastIndexOf('\\'));return storedPath[(split+1)..];
    }
}
