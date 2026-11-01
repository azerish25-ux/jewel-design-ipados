using JewelDesign.Documents;
namespace JewelDesign.Views.Candidate;

/// <summary>Immutable observation of one actual body read, including its original bytes and diagnostics.</summary>
public sealed class Rhino5UserStringReadOrigin
{
    public int ArchiveVersion {get;}
    public uint OpenNurbsVersion {get;}
    public LegacyUserStringListReadResult Result {get;}
    internal Rhino5UserStringReadOrigin(int version,uint build,LegacyUserStringListReadResult result)
    {ArchiveVersion=version;OpenNurbsVersion=build;Result=result;}
}
