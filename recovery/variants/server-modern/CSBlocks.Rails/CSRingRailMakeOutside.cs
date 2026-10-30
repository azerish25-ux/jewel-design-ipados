using System.Collections.Generic;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Runtime;

namespace CSBlocks.Rails;

[CSBlock("RingRailMakeOutside", "{620DCD47-00EF-4e0d-9A17-BCBB79B396C5}")]
public sealed class CSRingRailMakeOutside : CSBlock
{
	public readonly CSInputChannel iRingRail;

	public readonly CSInputChannel iOutsideRailTube;

	public readonly CSInputChannel iOutsideRailLine;

	public readonly CSInputChannel iScale;

	public readonly CSOutputChannel oOutsideRingRail;

	public CSRingRailMakeOutside(string identifier)
		: base(identifier)
	{
		iRingRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iOutsideRailTube = new CSInputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		iOutsideRailLine = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iScale = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oOutsideRingRail = new CSOutputChannel(this, ChannelDataType.RingRail, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		CSDataHelper.GetNurbsCurve(iRingRail.Data, out var value);
		if (value == null)
		{
			return UpdateMissing("Ring Rail");
		}
		NurbsSurface nurbsSurface = iOutsideRailTube.Data.GetNurbsSurface();
		if (nurbsSurface == null)
		{
			return UpdateMissing("Outside Ring Rail Tube");
		}
		CSDataHelper.GetNurbsCurve(iOutsideRailLine.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Outside Ring Rail Line");
		}
		double value3 = 1.0;
		iScale.GetDouble(ref value3);
		Transform val = Transform.Scale(Plane.WorldXY, 3.0, 1.0 + value3, 3.0);
		NurbsCurve val2 = new NurbsCurve(value);
		((GeometryBase)val2).Transform(val);
		NurbsSurface val3 = NurbsSurface.CreateRuledSurface((Curve)(object)value, (Curve)(object)val2);
		if (!((CommonObject)val3).IsValid)
		{
			return UpdateError("CreateRuledSurface failed");
		}
		Point3d[] array2 = default(Point3d[]);
		Curve[] array = default(Curve[]);
		if (!Intersection.SurfaceSurface((Surface)(object)val3, (Surface)(object)nurbsSurface, CSApplicationHelper.Application.Tolerance, ref array, ref array2))
		{
			return UpdateError("IntersectionSurfaces failed");
		}
		array = Curve.JoinCurves((IEnumerable<Curve>)array);
		NurbsCurve val4 = array[0].ToNurbsCurve();
		if (((Curve)val4).GetLength() <= 0.0)
		{
			return UpdateError("GetNurbForm failed");
		}
		if (((Curve)val4).IsClosed)
		{
			CurveIntersections val5 = Intersection.CurveCurve((Curve)(object)val4, (Curve)(object)value2, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val5.Count > 0)
			{
				double num = double.NaN;
				int num2 = 0;
				for (int i = 0; i < val5.Count; i++)
				{
					Point3d val6 = ((Curve)val4).PointAt(val5[i].ParameterA);
					double num3 = ((Point3d)(ref val6)).DistanceTo(Point3d.Origin);
					if (double.IsNaN(num) || !(num3 >= num))
					{
						num = num3;
						num2 = i;
					}
				}
				if (!((Curve)val4).ChangeClosedCurveSeam(val5[num2].ParameterA))
				{
					return UpdateError("ChangeClosedCurveSeam failed");
				}
			}
		}
		if (((Curve)value).IsClosed && ((Curve)val4).IsClosed)
		{
			CSUtilSweepRC.MatchCurveDirection2((Curve)(object)value, (Curve)(object)val4, 16);
		}
		else
		{
			CSUtilSweepRC.MatchCurveDirection((Curve)(object)value, (Curve)(object)val4, 16);
		}
		oOutsideRingRail.SetData(new CSOutsideRingRailData((Curve)(object)val4, value3));
		return base.Update();
	}
}
