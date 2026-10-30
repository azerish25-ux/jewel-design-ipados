using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Curves;

[CSBlock("UVCurveOnSurfaceFromTwoPoints", "{D9836B66-B1FC-4FB9-A3B9-553185A65870}")]
public sealed class CSUVCurveOnSurfaceFromTwoPoints : CSBlock
{
	public readonly CSInputChannel iSurface;

	public readonly CSInputChannel iPointStart;

	public readonly CSInputChannel iPointEnd;

	public readonly CSOutputChannel oCurve;

	public readonly CSOutputChannel oCurveFull;

	public CSUVCurveOnSurfaceFromTwoPoints(string identifier)
		: base(identifier)
	{
		iSurface = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPointStart = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		iPointEnd = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Array | ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oCurveFull = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface");
		}
		NurbsSurface nurbsSurface = flattenArray.GetArrayItem(0).GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataArray flattenArray2 = iPointStart.GetFlattenArray();
		if (flattenArray2.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Point Start");
		}
		Point3d value = default(Point3d);
		if (!CSDataHelper.GetPoint(flattenArray2.GetArrayItem(0), out value))
		{
			return UpdateMissing("Missing Point Start");
		}
		CSDataArray flattenArray3 = iPointEnd.GetFlattenArray();
		if (flattenArray3.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Point End");
		}
		Point3d value2 = default(Point3d);
		if (!CSDataHelper.GetPoint(flattenArray3.GetArrayItem(0), out value2))
		{
			return UpdateMissing("Missing Point End");
		}
		nurbsSurface.GetBoundingBox(accurate: true);
		if (flattenArray.GetArrayCount() < 1)
		{
			return UpdateMissing("Missing Surface");
		}
		double u = 0.0;
		double v = 0.0;
		if (!nurbsSurface.ClosestPoint(value, out u, out v))
		{
			return UpdateError("Closest Point 1 Failed");
		}
		Point3d point3d = new Point3d(u, v, 0.0);
		if (!nurbsSurface.ClosestPoint(value2, out u, out v))
		{
			return UpdateError("Closest Point 2 Failed");
		}
		Point3d to = new Point3d(u, v, 0.0);
		Line line = new Line(point3d, to);
		if (line.Length < double.Epsilon)
		{
			return UpdateError("Curve Legnth 0");
		}
		Curve curve2d = new LineCurve(point3d, to);
		Curve curve = nurbsSurface.Pushup(curve2d, 0.01);
		if (curve == null)
		{
			return UpdateError("Unable to Pushup");
		}
		oCurve.SetData(new CSOnGeometryData(curve));
		double num = (nurbsSurface.Domain(0).Length + nurbsSurface.Domain(1).Length) / line.Length;
		Curve curve2 = CSUtilGeometry.Trim2dLineCurveToSurface(new LineCurve(line.PointAt(0.0 - num), line.PointAt(1.0 + num)), nurbsSurface);
		if (curve2 == null)
		{
			return UpdateError("Unable to trim");
		}
		Curve curve3 = nurbsSurface.Pushup(curve2, 0.01);
		if (curve3 == null)
		{
			return UpdateError("Unable to push up trimmed curve");
		}
		oCurveFull.SetData(new CSOnGeometryData(curve3));
		return base.Update();
	}
}
