namespace JewelDesign.Geometry.Candidate;
/// <summary>Immutable nonperiodic tensor surface: Euclidean poles/weights, full knots and U-major grid.</summary>
public sealed class CandidateNurbsSurfaceDefinition
{
 public uint DegreeU {get;} public uint DegreeV {get;} public uint CountU {get;} public uint CountV {get;}
 internal double[] Coordinates {get;} internal double[] KnotsU {get;} internal double[] KnotsV {get;} internal double[]? Weights {get;}
 public CandidateNurbsSurfaceDefinition(uint degreeU,uint degreeV,uint countU,uint countV,IEnumerable<CandidatePoint3> poles,IEnumerable<double> knotsU,IEnumerable<double> knotsV,IEnumerable<double>? weights=null){
  ArgumentNullException.ThrowIfNull(poles);ArgumentNullException.ThrowIfNull(knotsU);ArgumentNullException.ThrowIfNull(knotsV);
  DegreeU=degreeU;DegreeV=degreeV;CountU=countU;CountV=countV;var points=poles.ToArray();Coordinates=new double[checked(points.Length*3)];for(int i=0;i<points.Length;i++){Coordinates[i*3]=points[i].X;Coordinates[i*3+1]=points[i].Y;Coordinates[i*3+2]=points[i].Z;}KnotsU=knotsU.ToArray();KnotsV=knotsV.ToArray();Weights=weights?.ToArray();
 }
}
