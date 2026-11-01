using JewelDesign.Geometry.Candidate;
namespace JewelDesign.Rendering.Candidate;

public readonly record struct Rhino5PlaneEquation(double A,double B,double C,double D);
public readonly record struct Rhino5ViewPlane(CandidatePoint3 Origin,CandidateVector3 X,CandidateVector3 Y,CandidateVector3 Z,Rhino5PlaneEquation Equation);
public readonly record struct Rhino5ClippingPlaneInfo(Rhino5PlaneEquation Equation,Guid Id,bool Enabled);
public readonly record struct Rhino5ViewPosition(double Left,double Right,double Top,double Bottom,
    byte MaximizedFlag,byte FloatingViewport,byte Reserved1,byte Reserved2,byte Reserved3);
public readonly record struct Rhino5PageSettings(int PageNumber,double WidthMillimeters,double HeightMillimeters,
    double LeftMargin,double RightMargin,double TopMargin,double BottomMargin,string PrinterName);
public readonly record struct Rhino5ConstructionPlane(Rhino5ViewPlane Plane,double GridSpacing,double SnapSpacing,
    int GridLineCount,int ThickLineFrequency,bool DepthBuffered,string Name);
public readonly record struct Rhino5TraceImage(Rhino5ViewPlane Plane,double Width,double Height,string Filename,bool GrayScale,bool Hidden,bool Filtered);
public readonly record struct Rhino5WallpaperImage(string Filename,bool GrayScale,bool Hidden);

/// <summary>Original5 metadata value shape. No native constructor defaults, filename resolution or later6 fields synthesized.</summary>
public sealed record Rhino5ViewMetadata(bool ProjectionLocked,string Name,Guid DisplayModeId,int LegacyV3DisplayMode,
    Rhino5ViewPosition Position,int ViewType,Rhino5PageSettings PageSettings,Rhino5ConstructionPlane ConstructionPlane,
    bool ShowConstructionGrid,bool ShowConstructionAxes,bool ShowWorldAxes,Rhino5TraceImage TraceImage,Rhino5WallpaperImage Wallpaper);

public readonly record struct Rhino5ViewportDetails(Guid ViewportId,Rhino5ScreenPort ScreenPort,
    Rhino5Matrix4 ClipModification,Rhino5Matrix4 InverseClipModification,double PerspectiveMinNearDist,double PerspectiveMinNearOverFar);

/// <summary>Owned converted userdata envelope. Preserving serialized payload is not a native polymorphic clone implementation.</summary>
public sealed class Rhino5ViewUserData
{
    private readonly byte[] payload;
    public Guid ClassId {get;}
    public Guid UserDataId {get;}
    public Guid ApplicationId {get;}
    public uint CopyCount {get;}
    public Rhino5Matrix4 Transform {get;}
    public int PayloadLength=>payload.Length;
    public Rhino5ViewUserData(Guid classId,Guid userDataId,Guid applicationId,uint copyCount,Rhino5Matrix4 transform,ReadOnlySpan<byte> serializedPayload)
    {
        ArgumentNullException.ThrowIfNull(transform);ClassId=classId;UserDataId=userDataId;ApplicationId=applicationId;
        CopyCount=copyCount;Transform=transform;payload=serializedPayload.ToArray();
    }
    public byte[] CopyPayload()=>(byte[])payload.Clone();
}

/// <summary>Opaque archive record retained in order, including duplicate type codes. This container does not validate/decode an archive.</summary>
public sealed class Rhino5PreservedViewChunk
{
    private readonly byte[] encoded;
    public uint TypeCode {get;}
    public long Value {get;}
    public int EncodedLength=>encoded.Length;
    public Rhino5PreservedViewChunk(uint typeCode,long value,ReadOnlySpan<byte> encodedChunk)
    {TypeCode=typeCode;Value=value;encoded=encodedChunk.ToArray();}
    public byte[] CopyEncoded()=>(byte[])encoded.Clone();
}
