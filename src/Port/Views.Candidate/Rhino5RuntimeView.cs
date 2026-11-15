using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Views.Candidate;

public sealed record Rhino5ViewArchiveContext(int ArchiveVersion,uint OpenNurbsVersion);
public sealed record Rhino5RuntimeViewImportLimits(int MaxBodies=1_000_000,long MaxTotalPayloadBytes=64*1024*1024);

/// <summary>Actual viewport userdata owner plus mapped original5 view values. Archive capture
/// is immutable source provenance; it is never an updated save image or a substitute for live userdata.</summary>
public sealed class Rhino5RuntimeView : Rhino5UserDataOwner
{
    // This private value contains no serialized userdata/chunks. Runtime userdata has exactly
    // one mutable owner here; original archive envelopes belong only to originalCapture.
    private Rhino5ViewData values=null!;
    private Rhino5ViewData? originalCapture;
    public Rhino5ViewportState Viewport=>values.Viewport;
    public CandidatePoint3 TargetMirror=>values.TargetMirror;
    public Rhino5ViewportDetails Details=>values.Details;
    public Rhino5ViewMetadata Metadata=>values.Metadata;
    public int ClippingPlaneCount=>values.ClippingPlaneCount;
    public Rhino5ViewData? OriginalArchiveCapture=>originalCapture;

    public Rhino5RuntimeView(Rhino5UserDataRuntime runtime,Rhino5ViewportState viewport,
        CandidatePoint3 targetMirror,Rhino5ViewportDetails details,Rhino5ViewMetadata metadata,
        ReadOnlySpan<Rhino5ClippingPlaneInfo> clippingPlanes):base(runtime)
    {SetConvertedValues(viewport,targetMirror,details,metadata,clippingPlanes);}

    private Rhino5RuntimeView(Rhino5RuntimeView source):base(source.Runtime)
    {
        source.Alive();
        try
        {
            // Original constructor copies userdata once on an empty base, before value fields.
            CopyUserDataFrom(source);
            values=source.values;
            originalCapture=source.originalCapture;
        }
        catch {PurgeUserData();throw;}
    }

    public Rhino5ClippingPlaneInfo GetClippingPlane(int index)=>values.GetClippingPlane(index);
    public Rhino5ClippingPlaneInfo[] CopyClippingPlanes()=>values.CopyClippingPlanes();

    /// <summary>Explicit converted values only; does not apply native setters, repair mirrors,
    /// notify a document, alter live userdata or pretend to update original archive bytes.</summary>
    public void SetConvertedValues(Rhino5ViewportState viewport,CandidatePoint3 targetMirror,
        Rhino5ViewportDetails details,Rhino5ViewMetadata metadata,ReadOnlySpan<Rhino5ClippingPlaneInfo> clippingPlanes)
    {
        Alive();
        values=new(viewport,targetMirror,details,metadata,clippingPlanes,[],[]);
    }

    public Rhino5RuntimeView Duplicate(){Alive();return new(this);}

    public void AssignFrom(Rhino5RuntimeView source)
    {
        Alive();ArgumentNullException.ThrowIfNull(source);source.Alive();
        if(ReferenceEquals(this,source))return;
        // Preserve native ordering and partial-failure visibility. Copy callbacks run while
        // destination values are still old; there is no fabricated atomic rollback.
        PurgeUserData();
        CopyUserDataFrom(source);
        // The byte is omitted by original assignment, so preserve its current value after
        // callbacks, including a legitimate mutation performed during purge/copy.
        values=source.values.WithDetails(source.Details with {OpaqueCopyByte=Details.OpaqueCopyByte});
        originalCapture=source.originalCapture;
    }

    /// <summary>Explicit native unknown-body import, not a whole3dm reader or arbitrary plugin
    /// reconstruction. Registered lookup can subsequently invoke a real supported body reader.</summary>
    public static Rhino5RuntimeView FromCapturedUnknownBodies(Rhino5UserDataRuntime runtime,
        Rhino5ViewData capture,Rhino5ViewArchiveContext context,Rhino5RuntimeViewImportLimits? limits=null)
    {
        ArgumentNullException.ThrowIfNull(runtime);ArgumentNullException.ThrowIfNull(capture);
        ArgumentNullException.ThrowIfNull(context);
        if(context.ArchiveVersion<0)throw new ArgumentOutOfRangeException(nameof(context));
        limits??=new();
        if(limits.MaxBodies<0||limits.MaxTotalPayloadBytes<0)throw new ArgumentOutOfRangeException(nameof(limits));
        if(capture.UserDataCount>limits.MaxBodies)throw new InvalidDataException("View userdata body-count limit exceeded.");
        long total=0;
        for(int i=0;i<capture.UserDataCount;i++)
        {
            total=checked(total+capture.GetUserData(i).PayloadLength);
            if(total>limits.MaxTotalPayloadBytes)throw new InvalidDataException("View userdata total-byte limit exceeded.");
        }
        var view=new Rhino5RuntimeView(runtime,capture.Viewport,capture.TargetMirror,capture.Details,
            capture.Metadata,capture.CopyClippingPlanes());
        view.originalCapture=capture;
        try
        {
            // Attach prepends. Reverse import retains actual captured source-list order.
            for(int i=capture.UserDataCount-1;i>=0;i--)
            {
                var item=capture.GetUserData(i);
                var data=new Rhino5UnknownUserData(runtime,item.UserDataId,item.ApplicationId,item.CopyCount,
                    item.Transform,item.ClassId,item.CopyPayload(),item.PayloadLength,context.ArchiveVersion,context.OpenNurbsVersion);
                try
                {
                    if(!view.AttachUserData(data))throw new InvalidDataException("Invalid or duplicate native unknown view userdata.");
                }
                catch {if(data.Owner is null)data.Dispose();throw;}
            }
            return view;
        }
        catch {view.Dispose();throw;}
    }

    public override void Dispose()
    {
        if(IsDisposed)return;
        base.Dispose();originalCapture=null;
    }
}
