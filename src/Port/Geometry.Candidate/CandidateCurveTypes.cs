namespace JewelDesign.Geometry.Candidate;

public readonly record struct CandidatePoint3(double X,double Y,double Z);
public readonly record struct CandidateVector3(double X,double Y,double Z);
public readonly record struct CandidatePlane(CandidatePoint3 Origin,CandidateVector3 Normal,CandidateVector3 XDirection)
{
    public static CandidatePlane WorldZX=>new(new(0,0,0),new(0,1,0),new(0,0,1));
    public static CandidatePlane WorldXY=>new(new(0,0,0),new(0,0,1),new(1,0,0));
}
public readonly record struct CandidateCurveInfo(double FirstParameter,double LastParameter,double Length,uint Degree,uint PoleCount,bool IsRational);

public readonly record struct CandidateCurveProjection(double Parameter,CandidatePoint3 Point,double Distance);
