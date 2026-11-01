using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

/// <summary>
/// Owned converted current-view data, including raw values and serialized unsupported payloads.
/// Capturing/updating this value does not execute native CopyUserData, filter zero copy counts,
/// repair target mirrors or imply whole-view history/archive compatibility.
/// </summary>
public sealed class Rhino5ViewData
{
    private readonly Rhino5ClippingPlaneInfo[] clippingPlanes;
    private readonly Rhino5ViewUserData[] userData;
    private readonly Rhino5PreservedViewChunk[] unknownChunks;
    public Rhino5ViewportState Viewport {get;}
    public CandidatePoint3 TargetMirror {get;}
    public Rhino5ViewportDetails Details {get;}
    public Rhino5ViewMetadata Metadata {get;}
    public int ClippingPlaneCount=>clippingPlanes.Length;
    public int UserDataCount=>userData.Length;
    public int UnknownChunkCount=>unknownChunks.Length;

    public Rhino5ViewData(Rhino5ViewportState viewport,CandidatePoint3 targetMirror,Rhino5ViewportDetails details,
        Rhino5ViewMetadata metadata,ReadOnlySpan<Rhino5ClippingPlaneInfo> clippingPlanes,
        ReadOnlySpan<Rhino5ViewUserData> userData,ReadOnlySpan<Rhino5PreservedViewChunk> unknownChunks)
    {
        Validate(details,metadata);Viewport=viewport;TargetMirror=targetMirror;Details=details;Metadata=metadata;
        this.clippingPlanes=clippingPlanes.ToArray();this.userData=userData.ToArray();this.unknownChunks=unknownChunks.ToArray();
        foreach(var item in this.userData)ArgumentNullException.ThrowIfNull(item);
        foreach(var item in this.unknownChunks)ArgumentNullException.ThrowIfNull(item);
    }
    private Rhino5ViewData(Rhino5ViewData source,Rhino5ViewportState viewport,CandidatePoint3 mirror,
        Rhino5ViewportDetails details,Rhino5ViewMetadata metadata)
    {
        Validate(details,metadata);Viewport=viewport;TargetMirror=mirror;Details=details;Metadata=metadata;
        // Sharing these private arrays is safe: no API exposes mutable arrays or mutable elements.
        clippingPlanes=source.clippingPlanes;userData=source.userData;unknownChunks=source.unknownChunks;
    }
    private static void Validate(Rhino5ViewportDetails details,Rhino5ViewMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(details.ClipModification);ArgumentNullException.ThrowIfNull(details.InverseClipModification);
        ArgumentNullException.ThrowIfNull(metadata);ArgumentNullException.ThrowIfNull(metadata.Name);
        ArgumentNullException.ThrowIfNull(metadata.PageSettings.PrinterName);ArgumentNullException.ThrowIfNull(metadata.ConstructionPlane.Name);
        ArgumentNullException.ThrowIfNull(metadata.TraceImage.Filename);ArgumentNullException.ThrowIfNull(metadata.Wallpaper.Filename);
    }
    public Rhino5ClippingPlaneInfo GetClippingPlane(int index)=>clippingPlanes[index];
    public Rhino5ClippingPlaneInfo[] CopyClippingPlanes()=>(Rhino5ClippingPlaneInfo[])clippingPlanes.Clone();
    public Rhino5ViewUserData GetUserData(int index)=>userData[index];
    public Rhino5PreservedViewChunk GetUnknownChunk(int index)=>unknownChunks[index];
    public Rhino5ViewData WithViewport(Rhino5ViewportState viewport,CandidatePoint3 targetMirror)=>new(this,viewport,targetMirror,Details,Metadata);
    public Rhino5ViewData WithMetadata(Rhino5ViewMetadata metadata)=>new(this,Viewport,TargetMirror,Details,metadata);
    public Rhino5ViewData WithDetails(Rhino5ViewportDetails details)=>new(this,Viewport,TargetMirror,details,Metadata);
}
