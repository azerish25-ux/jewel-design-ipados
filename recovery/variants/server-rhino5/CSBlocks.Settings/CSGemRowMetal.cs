using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Settings;

[CSBlock("GemRowMetal", "{FEA7DD33-7719-430a-B7D4-416A6584F066}")]
public sealed class CSGemRowMetal : CSBlock
{
	public readonly CSInputChannel InGem;

	public readonly CSInputChannel InProfile;

	public readonly CSInputChannel InGirdleSpacing;

	public readonly CSInputChannel InEndSpacing;

	public readonly CSInputChannel InPlacement;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InHeight;

	public readonly CSInputChannel InTopThickness;

	public readonly CSInputChannel InChannelDepth;

	public readonly CSInputChannel InSeatHeight;

	public readonly CSOutputChannel OutSurface;

	public CSGemRowMetal(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InEndSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacement = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InTopThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InChannelDepth = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSeatHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		List<CSGemData> gemList = InGem.GemList;
		if (gemList == null)
		{
			return UpdateMissing("Missing gem data");
		}
		if (gemList.Count < 1)
		{
			return UpdateMissing("Missing gem");
		}
		double num = InGirdleSpacing.Double;
		double num2 = InEndSpacing.Double;
		double dz = InPlacement.Double;
		double angleDegrees = InAngle.Double;
		double num3 = InHeight.Double;
		double num4 = InTopThickness.Double;
		double num5 = InChannelDepth.Double;
		double num6 = InSeatHeight.Double;
		if (num5 >= num3 - num6)
		{
			num5 = num3 - num6 - 0.1;
		}
		Transform transform = Transform.Translation(0.0, 0.0, dz);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < gemList.Count; i++)
		{
			CSGemData cSGemData = gemList[i];
			if (cSGemData == null)
			{
				return UpdateError("Invalid gem");
			}
			Transform transform2 = CSBlockUtil.CSUtilMath.Orient3Point(cSGemData.Center, cSGemData.Up, Vector3d.YAxis, Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis);
			Point3d point3d = new Point3d(cSGemData.Center);
			point3d.Y = 0.0;
			Point3d destination = point3d;
			Transform transform3 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, Vector3d.ZAxis, Vector3d.YAxis, destination, cSGemData.Up, Vector3d.YAxis);
			BoundingBox boundingBox = BoundingBox.Unset;
			if (!cSGemData.GetTightBoundingBox(ref boundingBox, doGrow: false, transform2))
			{
				return UpdateError("GetTightBoundingBox failed");
			}
			NurbsCurve nurbsCurve = InProfile.NurbsCurve;
			if (nurbsCurve == null)
			{
				return UpdateError("Missing profile");
			}
			double num7 = gemList.Count;
			double num8 = boundingBox.Max.Y - boundingBox.Min.Y;
			double num9 = num7 * num8 + 2.0 * num2;
			if (num7 > 1.0)
			{
				CSGemData cSGemData2 = gemList[1];
				if (cSGemData2 == null)
				{
					return UpdateError("Invalid gem");
				}
				double num10 = cSGemData.Center.DistanceTo(cSGemData2.Center) - num8;
				num9 += (num7 - 1.0) * num10;
			}
			Point3d point3d2 = new Point3d(boundingBox.Min.X - num, num9 / 2.0, 0.0 - num5);
			Point3d point3d3 = new Point3d(boundingBox.Max.X + num, num9 / 2.0, 0.0 - num5);
			LineCurve lineCurve = new LineCurve(point3d2, point3d3);
			Point3d point3d4 = new Point3d(boundingBox.Min.X - num, num9 / 2.0, num6);
			Point3d point3d5 = new Point3d(boundingBox.Max.X + num, num9 / 2.0, num6);
			LineCurve lineCurve2 = new LineCurve(point3d2, point3d4);
			LineCurve lineCurve3 = new LineCurve(point3d3, point3d5);
			Point3d to = new Point3d(boundingBox.Min.X - num - num4, num9 / 2.0, num6);
			Point3d point3d6 = new Point3d(boundingBox.Max.X + num + num4, num9 / 2.0, num6);
			LineCurve lineCurve4 = new LineCurve(point3d4, to);
			LineCurve lineCurve5 = new LineCurve(point3d5, point3d6);
			Transform xform = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
			nurbsCurve.Transform(xform);
			Vector3d motion = new Vector3d(nurbsCurve.PointAtStart);
			motion.Reverse();
			Transform xform2 = Transform.Translation(motion);
			nurbsCurve.Transform(xform2);
			double num11 = CSBlockUtil.CSUtilMath.AngleOfPointD(nurbsCurve.PointAtEnd.X, 0.0 - nurbsCurve.PointAtEnd.Z);
			Transform xform3 = CSBlockUtil.CSUtilMath.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angleDegrees) + num11, Vector3d.YAxis, Point3d.Origin);
			nurbsCurve.Transform(xform3);
			double num12 = 0.0 - nurbsCurve.PointAtEnd.Z;
			double num13 = num3 / num12;
			Transform xform4 = Transform.Scale(Plane.WorldXY, num13, num13, num13);
			nurbsCurve.Transform(xform4);
			Transform xform5 = Transform.Translation(point3d6 - nurbsCurve.PointAtStart);
			nurbsCurve.Transform(xform5);
			Transform xform6 = Transform.Scale(Plane.WorldXY, -1.0, 1.0, 1.0);
			NurbsCurve nurbsCurve2 = nurbsCurve.ToNurbsCurve();
			nurbsCurve2.Transform(xform6);
			LineCurve lineCurve6 = new LineCurve(nurbsCurve.PointAtEnd, nurbsCurve2.PointAtEnd);
			Curve[] inputCurves = new Curve[8] { lineCurve, lineCurve2, lineCurve4, nurbsCurve2, lineCurve6, nurbsCurve, lineCurve5, lineCurve3 };
			Curve[] array = null;
			array = Curve.JoinCurves(inputCurves);
			if (array.Length != 1)
			{
				return UpdateError("RhinoMergeCurves failed");
			}
			Curve obj = array[0];
			Curve curve = obj.DuplicateCurve();
			Transform xform7 = Transform.Translation(0.0, 0.0 - num9, 0.0);
			curve.Transform(xform7);
			Brep brep = NurbsSurface.CreateRuledSurface(obj, curve).ToBrep();
			if (brep == null)
			{
				return UpdateError("BrepForm failed");
			}
			Brep brep2 = CSUtilRhino.CapPlanarHoles(brep, CSApplicationHelper.Application.Tolerance);
			if (brep2 == null)
			{
				return UpdateError("CapPlanarHoles failed");
			}
			brep2.Transform(transform3 * transform);
			cSDataArray.Append(new CSOnGeometryData(brep2, transform3 * transform, applyTransform: false));
		}
		OutSurface.SetData(cSDataArray);
		return base.Update();
	}
}
