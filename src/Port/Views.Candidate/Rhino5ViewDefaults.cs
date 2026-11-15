using JewelDesign.Geometry.Candidate;
using JewelDesign.Rendering.Candidate;
namespace JewelDesign.Views.Candidate;

/// <summary>Fresh original5 view defaults, derived from the original constructor/default helpers
/// and runtime XY-plane initializer. This is not Initialize/Default on an existing live view.</summary>
public static class Rhino5ViewDefaults
{
    private static readonly double NegativeZero=BitConverter.Int64BitsToDouble(long.MinValue);
    private static readonly double Unset=BitConverter.Int64BitsToDouble(unchecked((long)0xffe5f8bd9e331dd2));
    public static Rhino5RuntimeView Create(Rhino5UserDataRuntime runtime)
    {
        var x=new CandidateVector3(1,0,0);var y=new CandidateVector3(0,1,0);var z=new CandidateVector3(0,0,1);
        var origin=new CandidatePoint3(0,0,0);var target=new CandidatePoint3(Unset,Unset,Unset);
        var plane=new Rhino5ViewPlane(origin,x,y,z,new(0,0,1,NegativeZero));
        var frame=new Rhino5CameraState(new(0,0,100),new(NegativeZero,NegativeZero,-1),y,x,y,z,true,false,false,false);
        var viewport=new Rhino5ViewportState(frame,new(-20,20,-20,20,.0001,1000),true,1,0,target);
        var details=new Rhino5ViewportDetails(Guid.Empty,new(0,1000,0,1000,0,1,false),Rhino5Matrix4.Identity,Rhino5Matrix4.Identity,.0001,.0001){OpaqueCopyByte=0};
        var metadata=new Rhino5ViewMetadata(false,string.Empty,Guid.Empty,1,new(0,1,0,1,0,0,0,0,0),0,
            new(0,0,0,0,0,0,0,string.Empty),new(plane,1,1,70,5,true,string.Empty),true,true,true,
            new(plane,0,0,string.Empty,true,false,false),new(string.Empty,true,false));
        return new(runtime,viewport,target,details,metadata,[]);
    }
}
