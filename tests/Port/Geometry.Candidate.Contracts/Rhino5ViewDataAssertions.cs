using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace CandidateGeometryContractChecks;
public static class Rhino5ViewDataAssertions
{
    private static int checks;
    private static void Check(bool ok,string message){checks++;if(!ok)throw new Exception(message);}
    public static string Run()
    {
        checks=0;int guidCounter=0;Guid NextId()=>new(++guidCounter,0,0,0,0,0,0,0,0,0,0);double nan=BitConverter.Int64BitsToDouble(unchecked((long)0x7ff8000000001234));
        var frame=new Rhino5CameraState(new(nan,-0d,80),new(2,3,-4),new(5,6,7),new(8,9,10),new(11,12,13),new(14,15,16),false,true,false,true);
        var viewport=new Rhino5ViewportState(frame,new(-3,2,-4,5,.1,100),false,99,0x83,new(1,2,3));
        var plane=new Rhino5ViewPlane(new(1,2,3),new(4,5,6),new(7,8,9),new(10,11,12),new(13,14,15,16));
        var metadata=new Rhino5ViewMetadata(true,"名前 α",NextId(),-77,new(-.2,1.3,.1,.9,1,2,3,4,5),42,
            new(9,210,297,1,2,3,4,@"C:\unavailable\printer"),new(plane,.5,.25,70,5,true,"grid"),true,false,true,
            new(plane,123,456,@"C:\unavailable\trace.png",true,false,true),new(@"C:\unavailable\wall.png",false,true));
        var matrix=new double[]{1,.2,0,3,0,1,.3,-2,.1,0,1,4,0,0,0,1};var clip=new Rhino5Matrix4(matrix);var inverse=Rhino5Matrix4.Identity;
        var details=new Rhino5ViewportDetails(NextId(),new(-1,900,700,2,0,65535,false),clip,inverse,1e-8,1e-7);
        var clipping=new[]{new Rhino5ClippingPlaneInfo(new(1,2,3,4),NextId(),true),new(new(5,6,7,8),NextId(),false)};
        var bytes=new byte[]{1,2,3,4};var dataId=NextId();var envelope=new Rhino5ViewUserData(NextId(),dataId,NextId(),0,clip,bytes);
        var userdata=new[]{envelope};var chunk=new Rhino5PreservedViewChunk(0x12345678,-91,bytes);var chunks=new[]{chunk,chunk};
        var view=new Rhino5ViewData(viewport,new(90,80,70),details,metadata,clipping,userdata,chunks);
        bytes[0]=99;clipping[0]=default;userdata[0]=null!;chunks[0]=null!;matrix[0]=99;
        Check(view.Metadata==metadata,"All metadata value sectors retained");
        Check(view.Metadata.Name=="名前 α"&&view.Metadata.LegacyV3DisplayMode==-77&&view.Metadata.ViewType==42,"Names and unknown enums retained");
        Check(view.Metadata.Position.Left==-.2&&view.Metadata.Position.Right==1.3&&view.Metadata.Position.Reserved3==5,"Desktop coordinates and reserved position bytes retained");
        Check(view.Metadata.TraceImage.Filename==@"C:\unavailable\trace.png"&&view.Metadata.Wallpaper.Filename==@"C:\unavailable\wall.png"&&view.Metadata.PageSettings.PrinterName==@"C:\unavailable\printer","Windows paths remain inert unchanged values");
        Check(BitConverter.DoubleToInt64Bits(view.Viewport.Frame.Location.X)==BitConverter.DoubleToInt64Bits(nan)&&BitConverter.DoubleToInt64Bits(view.Viewport.Frame.Location.Y)<0,"Raw NaN payload and signed zero retained without normalization");
        Check(view.TargetMirror==new CandidatePoint3(90,80,70)&&view.Viewport.Target==new CandidatePoint3(1,2,3),"Capture must not implicitly repair stale target mirror");
        Check(view.Details==details&&view.Details.ClipModification[0,0]==1&&view.Details.PerspectiveMinNearDist==1e-8&&view.Details.PerspectiveMinNearOverFar==1e-7,"Viewport UUID, screen, clip/inverse and copied near constraints retained");
        Check(view.ClippingPlaneCount==2&&view.GetClippingPlane(0).Equation==new Rhino5PlaneEquation(1,2,3,4)&&!view.GetClippingPlane(1).Enabled,"Clipping input array owned independently");
        var copy=view.CopyClippingPlanes();copy[0]=default;Check(view.GetClippingPlane(0).Enabled,"Clipping output cannot mutate owner");
        Check(view.UserDataCount==1&&view.GetUserData(0).UserDataId==dataId&&view.GetUserData(0).CopyCount==0,"Current-view capture retains zero-copy-count userdata; not native history cloning");
        var payload=view.GetUserData(0).CopyPayload();Check(payload.SequenceEqual(new byte[]{1,2,3,4}),"Userdata input bytes owned");payload[0]=88;Check(view.GetUserData(0).CopyPayload()[0]==1,"Userdata output bytes owned");
        Check(view.UnknownChunkCount==2&&view.GetUnknownChunk(0).TypeCode==0x12345678&&view.GetUnknownChunk(1).Value==-91,"Unknown chunk order and duplicates preserved");
        var encoded=view.GetUnknownChunk(0).CopyEncoded();encoded[0]=77;Check(view.GetUnknownChunk(0).CopyEncoded()[0]==1,"Unknown chunk bytes owned");
        var changed=view.WithViewport(viewport with {Target=new(3,2,1)},new(3,2,1));Check(changed.Viewport.Target!=view.Viewport.Target&&changed.TargetMirror!=view.TargetMirror,"Numeric update returns separate value state");
        Check(changed.Metadata==view.Metadata&&changed.Details==view.Details&&changed.GetUserData(0).CopyPayload().SequenceEqual(view.GetUserData(0).CopyPayload())&&changed.UnknownChunkCount==2,"Numeric update retains all unrelated metadata/payloads");
        var renamed=view.WithMetadata(metadata with {Name="renamed"});Check(renamed.Metadata.Name=="renamed"&&view.Metadata.Name=="名前 α"&&renamed.Viewport==view.Viewport,"Metadata update is immutable and retains numeric state");
        var resized=view.WithDetails(details with {ScreenPort=details.ScreenPort with {Right=1000}});Check(resized.Details.ScreenPort.Right==1000&&view.Details.ScreenPort.Right==900&&resized.Metadata==view.Metadata,"Viewport context update retains metadata and old state");
        Check(view.GetUserData(0).ClassId==envelope.ClassId&&view.GetUserData(0).ApplicationId==envelope.ApplicationId&&ReferenceEquals(view.GetUserData(0).Transform,clip),"Userdata identity/transform preserved without native dispatch");
        bool rejected=false;try{view.WithMetadata(metadata with {Name=null!});}catch(ArgumentNullException){rejected=true;}Check(rejected,"Null logical string rejected rather than silently normalized");
        Check(view.GetClippingPlane(0).Id!=Guid.Empty&&view.GetClippingPlane(0).Id!=view.GetClippingPlane(1).Id,"Clipping identities retained individually");
        var emptyView=new Rhino5ViewData(viewport,view.TargetMirror,details,metadata,Array.Empty<Rhino5ClippingPlaneInfo>(),Array.Empty<Rhino5ViewUserData>(),Array.Empty<Rhino5PreservedViewChunk>());
        Check(emptyView.ClippingPlaneCount==0&&emptyView.UserDataCount==0&&emptyView.UnknownChunkCount==0,"Empty collections do not invent metadata entries");
        rejected=false;try{new Rhino5ViewData(viewport,view.TargetMirror,details,metadata,Array.Empty<Rhino5ClippingPlaneInfo>(),new Rhino5ViewUserData[]{null!},Array.Empty<Rhino5PreservedViewChunk>());}catch(ArgumentNullException){rejected=true;}Check(rejected,"Null userdata entry rejected without silent filtering");
        rejected=false;try{new Rhino5ViewData(viewport,view.TargetMirror,details,metadata,Array.Empty<Rhino5ClippingPlaneInfo>(),Array.Empty<Rhino5ViewUserData>(),new Rhino5PreservedViewChunk[]{null!});}catch(ArgumentNullException){rejected=true;}Check(rejected,"Null preserved chunk rejected without silent filtering");
        rejected=false;try{view.WithDetails(details with {InverseClipModification=null!});}catch(ArgumentNullException){rejected=true;}Check(rejected,"Missing inverse clip matrix rejected instead of synthesized");
        var rawDetails=view.WithDetails(details with {PerspectiveMinNearDist=nan,PerspectiveMinNearOverFar=-0d});
        Check(BitConverter.DoubleToInt64Bits(rawDetails.Details.PerspectiveMinNearDist)==BitConverter.DoubleToInt64Bits(nan)&&BitConverter.DoubleToInt64Bits(rawDetails.Details.PerspectiveMinNearOverFar)<0,"Copied constraint bits retained without clamping");
        var maximumCount=new Rhino5ViewUserData(Guid.Empty,Guid.Empty,Guid.Empty,uint.MaxValue,inverse,Array.Empty<byte>());
        Check(maximumCount.CopyCount==uint.MaxValue&&maximumCount.PayloadLength==0,"Envelope retains raw copy count and identity; no attach/clone policy silently applied");
        return "PASS owned original5 view data:"+checks+" metadata/IEEE/identity/alias checks; converted current-view ownership only, native userdata cloning/history/archive/PC parity separate.";
    }
}
