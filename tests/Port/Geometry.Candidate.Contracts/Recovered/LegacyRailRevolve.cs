using JewelDesign.Geometry.Candidate;
namespace CandidateGeometryContractChecks.Recovered.RailRevolve;
internal sealed class On3dPoint{internal double x,y,z;internal On3dPoint(){}internal On3dPoint(On3dPoint p){x=p.x;y=p.y;z=p.z;}internal double DistanceTo(On3dPoint p)=>Math.Sqrt((x-p.x)*(x-p.x)+(y-p.y)*(y-p.y)+(z-p.z)*(z-p.z));}
internal sealed class On3dVector{internal On3dVector(On3dPoint p){}internal bool Unitize()=>true;}
internal static class OnUtil{internal static On3dPoint On_origin=>new();}
internal readonly record struct On4dPoint(double x,double y,double z,double w);
internal sealed class SourceCurve{private readonly CandidateNurbsDefinition d;private readonly double endZ;internal SourceCurve(CandidateNurbsDefinition d,double endZ){this.d=d;this.endZ=endZ;}internal int m_order=>(int)d.Degree+1;internal int m_cv_count=>d.Poles.Count;internal int CVCount()=>d.Poles.Count;internal int KnotCount()=>d.KnotVector.Count-2;internal double Knot(int i)=>d.KnotVector[i+1];internal double Weight(int i)=>d.PoleWeights?[i]??1;internal void GetCV(int i,ref On3dPoint p){var v=d.Poles[i];p.x=v.X;p.y=v.Y;p.z=v.Z;}internal On3dPoint PointAtEnd()=>new(){z=endZ};}
internal sealed class OnNurbsSurface{private int ou,ov,nu,nv;private On4dPoint[] p=System.Array.Empty<On4dPoint>();private double[] u=System.Array.Empty<double>(),v=System.Array.Empty<double>();internal void Create(int dim,bool is_rat,int ou,int ov,int nu,int nv){this.ou=ou;this.ov=ov;this.nu=nu;this.nv=nv;p=new On4dPoint[nu*nv];u=new double[nu+ou-2];v=new double[nv+ov-2];}internal void SetCV(int i,int j,On4dPoint q){p[i*nv+j]=q;}internal void SetKnot(int direction,int i,double x){if(direction==0)u[i]=x;else v[i]=x;}internal CandidateNurbsSurfaceDefinition Definition(){static double[] Full(double[] x){var a=new double[x.Length+2];a[0]=x[0];a[^1]=x[^1];System.Array.Copy(x,0,a,1,x.Length);return a;}return new((uint)(ou-1),(uint)(ov-1),(uint)nu,(uint)nv,p.Select(x=>new CandidatePoint3(x.x/x.w,x.y/x.w,x.z/x.w)),Full(u),Full(v),p.Select(x=>x.w));}}
internal static class SourceTensor{internal static CandidateNurbsSurfaceDefinition Build(SourceCurve onNurbsCurve,SourceCurve onNurbsCurve2,double s1,int direction,bool zAdjust){
OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
		if (direction == 0)
		{
			onNurbsSurface.Create(3, is_rat: true, onNurbsCurve.m_order, onNurbsCurve2.m_order, onNurbsCurve.m_cv_count, onNurbsCurve2.m_cv_count);
		}
		else
		{
			onNurbsSurface.Create(3, is_rat: true, onNurbsCurve2.m_order, onNurbsCurve.m_order, onNurbsCurve2.m_cv_count, onNurbsCurve.m_cv_count);
		}
		int num = onNurbsCurve2.CVCount();
		int num2 = onNurbsCurve.CVCount();
		double z = onNurbsCurve.PointAtEnd().z;
		for (int i = 0; i < num2; i++)
		{
			On3dPoint vertex_value = new On3dPoint();
			onNurbsCurve.GetCV(i, ref vertex_value);
			On3dPoint on3dPoint = new On3dPoint(vertex_value);
			on3dPoint.z = 0.0;
			new On3dVector(on3dPoint).Unitize();
			double num3 = on3dPoint.DistanceTo(OnUtil.On_origin);
			double num4 = (zAdjust ? (1.0 - vertex_value.z / z) : 0.0);
			if (double.IsNaN(num4))
			{
				num4 = 0.0;
			}
			for (int j = 0; j < num; j++)
			{
				On3dPoint vertex_value2 = new On3dPoint();
				onNurbsCurve2.GetCV(j, ref vertex_value2);
				vertex_value2.x = vertex_value2.x * num3 / s1;
				vertex_value2.y = vertex_value2.y * num3 / s1;
				vertex_value2.z = vertex_value.z + num4 * vertex_value2.z;
				double num5 = onNurbsCurve.Weight(i) * onNurbsCurve2.Weight(j);
				if (direction == 0)
				{
					onNurbsSurface.SetCV(i, j, new On4dPoint(vertex_value2.x * num5, vertex_value2.y * num5, vertex_value2.z * num5, num5));
				}
				else
				{
					onNurbsSurface.SetCV(j, i, new On4dPoint(vertex_value2.x * num5, vertex_value2.y * num5, vertex_value2.z * num5, num5));
				}
			}
		}
		for (int k = 0; k < onNurbsCurve.KnotCount(); k++)
		{
			onNurbsSurface.SetKnot(direction, k, onNurbsCurve.Knot(k));
		}
		for (int l = 0; l < onNurbsCurve2.KnotCount(); l++)
		{
			onNurbsSurface.SetKnot(1 - direction, l, onNurbsCurve2.Knot(l));
		}
		return onNurbsSurface.Definition();
}}
