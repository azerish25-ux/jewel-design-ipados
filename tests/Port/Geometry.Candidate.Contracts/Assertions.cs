using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks;

public static class Assertions
{
    private static void Check(bool condition,string message)
    { if(!condition)throw new Exception(message); }
    private static T Throws<T>(Action action) where T:Exception
    {
        try {action();}catch(T error){return error;}
        throw new Exception("Expected "+typeof(T).Name);
    }
    private static void Mass(CandidateContext context,CandidateShape shape,double expected,uint solids)
    {
        var actual=context.Inspect(shape);
        Check(actual.IsValid&&actual.SolidCount==solids&&double.IsFinite(actual.Volume)&&Math.Abs(actual.Volume-expected)<1e-7,"Managed ABI actual geometry/topology");
    }
    public static string Run()
    {
        using var context=CandidateContext.Create();
        using var foreignContext=CandidateContext.Create();
        {
            using var original=context.CreateBox(2,2,2);
            using var translated=context.Translate(original,1,0,0);
            using var far=context.Translate(original,100,0,0);
            using var common=context.Boolean(CandidateBooleanOperation.Common,original,translated,.01);
            using var fused=context.Boolean(CandidateBooleanOperation.Fuse,original,translated,.01);
            using var cut=context.Boolean(CandidateBooleanOperation.Cut,original,translated,.01);
            using var empty=context.Boolean(CandidateBooleanOperation.Common,original,far,.01);
            Mass(context,original,8,1);Mass(context,translated,8,1);
            Mass(context,common,4,1);Mass(context,fused,12,1);Mass(context,cut,4,1);Mass(context,empty,0,0);
            using var clone=context.Clone(original);
            using var retained=context.Retain(original);
            original.Dispose();original.Dispose();
            Mass(context,clone,8,1);Mass(context,retained,8,1);
            Throws<ObjectDisposedException>(()=>context.Inspect(original));
            using var foreign=foreignContext.CreateBox(1,1,1);
            Throws<ArgumentException>(()=>context.Clone(foreign));
            Throws<ArgumentNullException>(()=>context.Inspect(null!));
            foreach(var bad in new[]{-1.0,0.0,double.NaN,double.PositiveInfinity}) {
                var error=Throws<CandidateGeometryException>(()=>{using var ignored=context.CreateBox(bad,1,1);});
                Check(error.Status==CandidateNativeStatus.InvalidArgument&&!string.IsNullOrWhiteSpace(error.Message),"Native argument status/message lost");
            }
            var unsupported=Throws<CandidateGeometryException>(()=>{using var ignored=context.Boolean((CandidateBooleanOperation)999,clone,translated,.01);});
            Check(unsupported.Status==CandidateNativeStatus.UnsupportedOperation,"Unsupported operation status lost");
            var tolerance=Throws<CandidateGeometryException>(()=>{using var ignored=context.Boolean(CandidateBooleanOperation.Common,clone,translated,double.NaN);});
            Check(tolerance.Status==CandidateNativeStatus.InvalidArgument,"Invalid tolerance was silently repaired");
        }
        Check(context.DiagnosticLiveShapeCount==0,"Managed shape disposal retained native entries");
        Check(foreignContext.DiagnosticLiveShapeCount==0,"Foreign shape disposal retained entries");
        for(int i=0;i<16;i++) {using var shape=context.CreateBox(1,1,1);Mass(context,shape,1,1);}
        Check(context.DiagnosticLiveShapeCount==0,"Repeated SafeHandle disposal retained entries");
        using var lifetime=CandidateContext.Create();
        var child=lifetime.CreateBox(1,1,1);var alias=lifetime.Retain(child);
        var id=lifetime.DangerousContextId;var message=new byte[256];
        lifetime.Dispose();
        Check(NativeMethods.ContextLiveShapesRaw(id,out var count,message,256)==0&&count==1,"Child parent lease did not retain native context");
        Throws<ObjectDisposedException>(()=>lifetime.Inspect(child));
        child.Dispose();child.Dispose();
        Check(NativeMethods.ContextLiveShapesRaw(id,out count,message,256)==0&&count==1,"Retained alias lost parent/shape lifetime");
        alias.Dispose();alias.Dispose();
        Check(NativeMethods.ContextLiveShapesRaw(id,out count,message,256)==(int)CandidateNativeStatus.InvalidContext,"Last child did not release native parent");
        var curves=CurveAssertions.Run();
        var ringRail=RingRailAssertions.Run();
        var curveLine=CurveLineAssertions.Run();
        var seam=CurveSeamAssertions.Run();
        var bounds=CurveBoundsAssertions.Run();
        var outsideRail=OutsideRailAssertions.Run();
        var storedProfiles=StoredProfileAssertions.Run();
        var valueGeometry=ValueGraphGeometryAssertions.Run();
        var railNodes=RailNodeAssertions.Run();
        var railGraph=RailGraphAssertions.Run();
        var selectRail=SelectRailAssertions.Run();
        var transforms=TransformProducerAssertions.Run();
        var placement=ProfilePlacementAssertions.Run();
        var rotation=RotationProducerAssertions.Run();
        var consumer=TransformConsumerAssertions.Run();
        var collectionSockets=CollectionSocketAssertions.Run();
        var ownedCollections=OwnedCollectionAssertions.Run();
        var arrayConsumers=ArrayConsumerAssertions.Run();
        var sweepPreparation=SweepPreparationAssertions.Run();
        var pairedSweep=PairedSweepAssertions.Run();
        var brepGraph=BrepGraphAssertions.Run();
        var nurbsSurfaces=NurbsSurfaceAssertions.Run();
        var domeProfiles=DomeProfileAssertions.Run();
        return "PASS managed candidate ABI: real box/translate/boolean/copy, retained aliases, typed errors/ownership guards, SafeHandle parent leases and native entry disposal; candidate only, no full kernel/PC/physical or process-memory leak claim. "+curves+" "+ringRail+" "+curveLine+" "+seam+" "+bounds+" "+outsideRail+" "+storedProfiles+" "+valueGeometry+" "+railNodes+" "+railGraph+" "+selectRail+" "+transforms+" "+placement+" "+rotation+" "+consumer+" "+collectionSockets+" "+ownedCollections+" "+arrayConsumers+" "+sweepPreparation+" "+pairedSweep+" "+brepGraph+" "+nurbsSurfaces+" "+domeProfiles;
    }
}
