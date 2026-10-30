using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Gems.Obsolete;

[Obsolete]
[CSBlock("GemAroundRingRailSpan_V1", "{1ED8711D-7220-4973-9A3A-B17991E57A09}")]
public sealed class CSGemAroundRingRailSpan1 : CSBlock
{
	public readonly CSInputChannel InRail;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InSpan;

	public readonly CSInputChannel InSpacing;

	public readonly CSInputChannel InZOffset;

	public readonly CSInputChannel InYOffset;

	public readonly CSInputChannel InAngle;

	public readonly CSInputChannel InRotation;

	public readonly CSInputChannel InPosition;

	public readonly CSInputChannel InPlacementMethod;

	public readonly CSOutputChannel OutGem;

	public CSGemAroundRingRailSpan1(string identifier)
		: base(identifier)
	{
		InRail = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InRail.NurbsCurve;
		if (nurbsCurve == null)
		{
			return UpdateMissing("Rail");
		}
		if (!(InGemDefinition.Data is CSGemData cSGemData))
		{
			return UpdateMissing("Gem");
		}
		double angleDegrees = InSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InSpacing.Double;
		double zOffset = InZOffset.Double;
		double num2 = InYOffset.Double;
		double angleDegrees2 = InAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleRadians = CSBlockUtil.CSUtilMath.ToRadians(InRotation.Double);
		double num3 = InPosition.Double;
		int integer = InPlacementMethod.Integer;
		Transform val = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.XAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, Vector3d.ZAxis, Point3d.Origin);
		Transform val3 = Transform.Translation(0.0, num2, 0.0);
		CSData cSData = cSGemData.Duplicate();
		CSDataHelper.Transform(cSData, val2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		Point3d val4 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num4 = x - ((Point3d)(ref val4)).X;
		CSDataHelper.GetTransformNoArray(InRail.Data, out var value);
		Vector3d val5 = default(Vector3d);
		Interval domain;
		Vector3d val7;
		if (!((Transform)(ref value)).IsValid)
		{
			((Vector3d)(ref val5))._002Ector(Vector3d.ZAxis);
			Vector3d val6 = val5;
			domain = ((Curve)nurbsCurve).Domain;
			val7 = Vector3d.CrossProduct(val6, ((Curve)nurbsCurve).TangentAt(((Interval)(ref domain)).Min));
			((Vector3d)(ref val7)).Unitize();
		}
		else
		{
			((Vector3d)(ref val5))._002Ector(Vector3d.YAxis);
			((Vector3d)(ref val5)).Transform(value);
			((Vector3d)(ref val5)).Unitize();
			domain = ((Curve)nurbsCurve).Domain;
			val7 = Vector3d.CrossProduct(((Curve)nurbsCurve).TangentAt(((Interval)(ref domain)).Min), val5);
			((Vector3d)(ref val7)).Unitize();
		}
		CSUtilGem.GetGemZOffset(cSGemData, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Curve val8 = CSUtilGeometry.OriginOffset((Curve)(object)nurbsCurve, -val7, zOffset);
		if (val8 == null)
		{
			return UpdateError("Offset Rail");
		}
		double length = val8.GetLength();
		if (double.IsNaN(length) || length <= 0.0)
		{
			return UpdateError("Offset Rail Length");
		}
		double num5;
		if (angleDegrees <= 0.0)
		{
			num5 = 0.0;
		}
		else if (angleDegrees >= Math.PI * 2.0)
		{
			num5 = length;
		}
		else
		{
			LineCurve val9 = new LineCurve(Point3d.Origin, new Point3d(0.0, 0.0, length));
			((GeometryBase)val9).Rotate(angleDegrees, Vector3d.YAxis, Point3d.Origin);
			CurveIntersections val10 = Intersection.CurveCurve(val8, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val10.Count != 1)
			{
				return UpdateError("Intersection");
			}
			double tolerance = CSApplicationHelper.Application.Tolerance;
			domain = val8.Domain;
			num5 = val8.GetLength(tolerance, new Interval(((Interval)(ref domain)).Min, val10[0].ParameterA));
		}
		int num6 = (int)(length / (num4 + num));
		int num7 = (int)(num5 / (num4 + num));
		CSDataArray cSDataArray = new CSDataArray();
		if (num7 > 0)
		{
			int num8 = num7 % 2;
			int num9 = num7 / 2;
			double num10 = 1.0 / (double)num6;
			double num11 = num10 / 2.0;
			double num12 = (double)(1 - num8) * num11 % num10;
			for (int i = 0; i < num6; i++)
			{
				if (i < num8 + num9 || i > num6 - 1 - num9)
				{
					double num13 = num3 + num12;
					if (num13 > 1.0)
					{
						num13 -= 1.0;
					}
					if (num13 < 0.0)
					{
						num13 += 1.0;
					}
					double num14 = 0.0;
					if (!val8.NormalizedLengthParameter(num13, ref num14))
					{
						continue;
					}
					Vector3d destination = Vector3d.CrossProduct(val8.TangentAt(num14), val7);
					((Vector3d)(ref destination)).Unitize();
					Transform val11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, cSGemData.North, cSGemData.Up, val8.PointAt(num14), val7, destination);
					CSData data = cSGemData.Duplicate();
					CSDataHelper.Transform(data, val11 * val3 * val * val2);
					cSDataArray.Append(data);
				}
				num12 += num10;
			}
		}
		else
		{
			UpdateError("Count contains Zero Elements - Cannot create gems.");
		}
		OutGem.SetData(cSDataArray);
		return base.Update();
	}
}
