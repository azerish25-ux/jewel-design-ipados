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

namespace CSBlocks.Gems;

[CSBlock("GemAroundRingRailSpan", "{CACCABCC-B8C5-46e6-9450-6DB2B7B407AB}")]
public sealed class CSGemAroundRingRailSpan : CSBlock
{
	public readonly CSInputChannel InCurve;

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

	public CSGemAroundRingRailSpan(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		double angleDegrees = InSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InSpacing.Double;
		double num2 = InZOffset.Double;
		double num3 = InYOffset.Double;
		double angleDegrees2 = InAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleRadians = CSBlockUtil.CSUtilMath.ToRadians(InRotation.Double);
		double num4 = InPosition.Double;
		int integer = InPlacementMethod.Integer;
		Transform val = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.XAxis, Point3d.Origin);
		Transform val2 = CSBlockUtil.CSUtilMath.Rotation(angleRadians, Vector3d.ZAxis, Point3d.Origin);
		Transform val3 = Transform.Translation(0.0, num3, 0.0);
		CSData cSData = gem.Duplicate();
		CSDataHelper.Transform(cSData, val2);
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!cSData.GetTightBoundingBox(ref boundingBox))
		{
			return UpdateError("Gem Bounding Box");
		}
		Point3d val4 = ((BoundingBox)(ref boundingBox)).Max;
		double x = ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref boundingBox)).Min;
		double num5 = x - ((Point3d)(ref val4)).X;
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(value);
		Vector3d val5;
		Interval domain;
		Vector3d val7;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			val5 = Vector3d.ZAxis;
			((Vector3d)(ref val5)).Transform(value);
			((Vector3d)(ref val5)).Unitize();
			Vector3d val6 = val5;
			domain = ((Curve)nurbsCurve).Domain;
			val7 = Vector3d.CrossProduct(val6, ((Curve)nurbsCurve).TangentAt(((Interval)(ref domain)).Min));
			((Vector3d)(ref val7)).Unitize();
		}
		else
		{
			val5 = Vector3d.YAxis;
			((Vector3d)(ref val5)).Transform(value);
			((Vector3d)(ref val5)).Unitize();
			domain = ((Curve)nurbsCurve).Domain;
			val7 = Vector3d.CrossProduct(((Curve)nurbsCurve).TangentAt(((Interval)(ref domain)).Min), val5);
			((Vector3d)(ref val7)).Unitize();
		}
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Curve val8 = CSUtilGeometry.OffsetOld((Curve)(object)nurbsCurve, -val7, origin, zOffset + num2, closed: true);
		if (val8 == null)
		{
			return UpdateError("Offset Rail");
		}
		double num6 = double.NaN;
		num6 = val8.GetLength();
		if (double.IsNaN(num6))
		{
			return UpdateError("Offset Rail Length");
		}
		double num7;
		if (angleDegrees <= 0.0)
		{
			num7 = 0.0;
		}
		else if (angleDegrees >= Math.PI * 2.0)
		{
			num7 = num6;
		}
		else
		{
			LineCurve val9 = new LineCurve(origin, origin + new Vector3d(0.0, 0.0, num6));
			((GeometryBase)val9).Rotate(angleDegrees, Vector3d.YAxis, Point3d.Origin);
			CurveIntersections val10 = Intersection.CurveCurve(val8, (Curve)(object)val9, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
			if (val10.Count != 1)
			{
				return UpdateError("Intersection");
			}
			double tolerance = CSApplicationHelper.Application.Tolerance;
			domain = val8.Domain;
			num7 = val8.GetLength(tolerance, new Interval(((Interval)(ref domain)).Min, val10[0].ParameterA));
			if (double.IsNaN(num7) || num7 <= 0.0)
			{
				return UpdateError("Intersection Length");
			}
		}
		int num8 = (int)(num6 / (num5 + num));
		int num9 = (int)(num7 / (num5 + num));
		CSDataArray cSDataArray = new CSDataArray();
		if (num9 > 0)
		{
			int num10 = num9 % 2;
			int num11 = num9 / 2;
			double num12 = 1.0 / (double)num8;
			double num13 = num12 / 2.0;
			double num14 = (double)(1 - num10) * num13 % num12;
			for (int i = 0; i < num8; i++)
			{
				if (i < num10 + num11 || i > num8 - 1 - num11)
				{
					double num15 = num4 + num14;
					if (num15 > 1.0)
					{
						num15 -= 1.0;
					}
					if (num15 < 0.0)
					{
						num15 += 1.0;
					}
					double num16 = 0.0;
					if (!val8.NormalizedLengthParameter(num15, ref num16))
					{
						continue;
					}
					Vector3d destination = Vector3d.CrossProduct(val8.TangentAt(num16), val7);
					((Vector3d)(ref destination)).Unitize();
					Transform val11 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val8.PointAt(num16), val7, destination);
					CSData data = gem.Duplicate();
					CSDataHelper.Transform(data, val11 * val3 * val * val2);
					cSDataArray.Append(data);
				}
				num14 += num12;
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
