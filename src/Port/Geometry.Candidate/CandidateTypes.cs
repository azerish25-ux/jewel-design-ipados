namespace JewelDesign.Geometry.Candidate;

public enum CandidateBooleanOperation { Common=1, Fuse=2, Cut=3 }
public enum CandidateNativeStatus {
    Ok=0, InvalidArgument=1, UnsupportedVersion=2, InvalidContext=3,
    InvalidShape=4, UnsupportedOperation=5, KernelFailure=6,
    InternalError=7, OutOfMemory=8, LifetimeLimit=9
}
public readonly record struct CandidateShapeInfo(double Volume,uint SolidCount,bool IsValid);
public sealed class CandidateGeometryException:Exception
{
    public CandidateNativeStatus Status {get;}
    public string NativeMessage {get;}
    internal CandidateGeometryException(CandidateNativeStatus status,string message)
        :base("Candidate geometry failed ("+status+"): "+message)
    {Status=status;NativeMessage=message;}
}
