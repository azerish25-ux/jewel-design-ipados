using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockDataUtils;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Gems;

[CSBlock("GemPlacer", "{897F521E-A9EE-4688-94C5-C1B6A32B694F}")]
public sealed class CSGemPlacer : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemCount;

	public readonly CSInputChannel InGemSpacing;

	public readonly CSInputChannel InGemZOffset;

	public readonly CSInputChannel InGemYOffset;

	public readonly CSInputChannel InGemYOffsetMirror;

	public readonly CSInputChannel InGemTilt;

	public readonly CSInputChannel InGemAngle;

	public readonly CSInputChannel InGemRotation;

	public readonly CSInputChannel InGemRotationMirror;

	public readonly CSInputChannel InGemPosition;

	public readonly CSInputChannel InGemPlacementMethod;

	public readonly CSInputChannel InGemOnOff;

	public readonly CSOutputChannel OutGem;

	public readonly CSOutputChannel OutDisplayGems;

	public CSGemPlacer(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemYOffsetMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemTilt = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemRotationMirror = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		InGemPosition = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
		InGemPlacementMethod = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 0);
		InGemOnOff = new CSInputChannel(this, ChannelDataType.Boolean, ChannelArrayType.Individual, defaultValue: false);
		OutGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		OutDisplayGems = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		Curve nurbsCurve = (Curve)(object)InCurve.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemCount.Integer;
		int integer2 = InGemPlacementMethod.Integer;
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees = InGemAngle.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double angleDegrees2 = InGemRotation.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double num5 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		Point3d origin = Point3d.Origin;
		((Point3d)(ref origin)).Transform(value);
		Vector3d val;
		Interval domain;
		Vector3d val3;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			val = Vector3d.ZAxis;
			((Vector3d)(ref val)).Transform(value);
			((Vector3d)(ref val)).Unitize();
			Vector3d val2 = val;
			domain = nurbsCurve.Domain;
			val3 = Vector3d.CrossProduct(val2, nurbsCurve.TangentAt(((Interval)(ref domain)).Min));
			((Vector3d)(ref val3)).Unitize();
		}
		else
		{
			val = Vector3d.YAxis;
			((Vector3d)(ref val)).Transform(value);
			((Vector3d)(ref val)).Unitize();
			domain = nurbsCurve.Domain;
			val3 = Vector3d.CrossProduct(nurbsCurve.TangentAt(((Interval)(ref domain)).Min), val);
			((Vector3d)(ref val3)).Unitize();
		}
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer2, ref zOffset);
		Curve val4 = CSUtilGeometry.OffsetOld(nurbsCurve, val3, origin, 0.0 - (zOffset + num2), closed: true);
		if (val4 == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = val4.GetLength();
		Transform val5 = Transform.Rotation(angleDegrees, Vector3d.XAxis, Point3d.Origin);
		Transform val6 = Transform.Translation(0.0, boolean ? 0.0 : num3, 0.0);
		Transform val7 = Transform.Rotation(boolean2 ? 0.0 : angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform val8 = val5 * val7;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, val8))
		{
			return UpdateError("Center Gem Bounding Box");
		}
		Transform val9 = (boolean ? Transform.Translation(0.0, 0.0 - num3, 0.0) : Transform.Translation(0.0, num3, 0.0));
		Transform val10 = (boolean2 ? Transform.Rotation(0.0 - angleDegrees2, Vector3d.ZAxis, Point3d.Origin) : Transform.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin));
		Transform val11 = val5 * val10;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, val11))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		Transform val12 = Transform.Translation(0.0, num3, 0.0);
		Transform val13 = Transform.Rotation(angleDegrees2, Vector3d.ZAxis, Point3d.Origin);
		Transform val14 = val5 * val13;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, val14))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		int num6 = integer % 2;
		int num7 = integer / 2;
		Point3d val15;
		if (num6 > 0)
		{
			double num8 = num5;
			val15 = ((BoundingBox)(ref boundingBox)).Max;
			double num9 = Math.Abs(((Point3d)(ref val15)).X);
			val15 = ((BoundingBox)(ref boundingBox)).Min;
			num5 = num8 - (num9 - Math.Abs(((Point3d)(ref val15)).X)) / length / 2.0;
		}
		else
		{
			double num10 = num5;
			val15 = ((BoundingBox)(ref boundingBox)).Max;
			double num11 = Math.Abs(((Point3d)(ref val15)).X);
			val15 = ((BoundingBox)(ref boundingBox)).Min;
			num5 = num10 - (num11 - Math.Abs(((Point3d)(ref val15)).X)) / length;
		}
		double num13;
		if (num6 > 0)
		{
			if (boolean2)
			{
				val15 = ((BoundingBox)(ref boundingBox)).Max;
				double num12 = Math.Abs(((Point3d)(ref val15)).X);
				val15 = ((BoundingBox)(ref boundingBox2)).Min;
				num13 = num12 + Math.Abs(((Point3d)(ref val15)).X) + num;
			}
			else
			{
				val15 = ((BoundingBox)(ref boundingBox)).Max;
				double num14 = Math.Abs(((Point3d)(ref val15)).X);
				val15 = ((BoundingBox)(ref boundingBox2)).Min;
				num13 = num14 + Math.Abs(((Point3d)(ref val15)).X) + num;
			}
		}
		else if (boolean2)
		{
			val15 = ((BoundingBox)(ref boundingBox2)).Min;
			num13 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
		}
		else
		{
			val15 = ((BoundingBox)(ref boundingBox2)).Max;
			num13 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
		}
		double num20 = default(double);
		for (int num15 = num7 - 1; num15 >= 0; num15--)
		{
			double num16 = num5;
			double num17 = num13;
			double num18 = num15;
			val15 = ((BoundingBox)(ref boundingBox2)).Max;
			double x = ((Point3d)(ref val15)).X;
			val15 = ((BoundingBox)(ref boundingBox2)).Min;
			double num19 = num16 + (num17 + num18 * (x - ((Point3d)(ref val15)).X + num)) / length;
			if (num19 > 1.0)
			{
				num19 -= 1.0;
			}
			if (num19 < 0.0)
			{
				num19 += 1.0;
			}
			if (val4.NormalizedLengthParameter(num19, ref num20))
			{
				Vector3d val16 = Vector3d.CrossProduct(val4.TangentAt(num20), val3);
				((Vector3d)(ref val16)).Unitize();
				Vector3d destination = val16 * num4 + val * (1.0 - num4);
				if (((Vector3d)(ref destination)).Length < 0.001)
				{
					destination = Vector3d.ZAxis;
				}
				else
				{
					((Vector3d)(ref destination)).Unitize();
				}
				Transform val17 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val4.PointAt(num20), val3, destination);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, val17 * val9 * val11);
				cSDataArray.Append(data);
			}
		}
		if (num6 > 0)
		{
			double num21 = num5;
			if (num21 > 1.0)
			{
				num21 -= 1.0;
			}
			if (num21 < 0.0)
			{
				num21 += 1.0;
			}
			double num22 = default(double);
			if (!val4.NormalizedLengthParameter(num21, ref num22))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			Vector3d val18 = Vector3d.CrossProduct(val4.TangentAt(num22), val3);
			((Vector3d)(ref val18)).Unitize();
			Vector3d destination2 = val18 * num4 + val * (1.0 - num4);
			if (((Vector3d)(ref destination2)).Length < 0.001)
			{
				destination2 = Vector3d.ZAxis;
			}
			else
			{
				((Vector3d)(ref destination2)).Unitize();
			}
			Transform val19 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val4.PointAt(num22), val3, destination2);
			CSData data2 = gem.Duplicate();
			CSDataHelper.Transform(data2, val19 * val6 * val8);
			cSDataArray.Append(data2);
		}
		double num24;
		if (num6 > 0)
		{
			if (boolean2)
			{
				val15 = ((BoundingBox)(ref boundingBox)).Min;
				double num23 = Math.Abs(((Point3d)(ref val15)).X);
				val15 = ((BoundingBox)(ref boundingBox3)).Max;
				num24 = num23 + Math.Abs(((Point3d)(ref val15)).X) + num;
			}
			else
			{
				val15 = ((BoundingBox)(ref boundingBox)).Min;
				double num25 = Math.Abs(((Point3d)(ref val15)).X);
				val15 = ((BoundingBox)(ref boundingBox3)).Max;
				num24 = num25 + Math.Abs(((Point3d)(ref val15)).X) + num;
			}
		}
		else if (boolean2)
		{
			val15 = ((BoundingBox)(ref boundingBox3)).Max;
			num24 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
		}
		else
		{
			val15 = ((BoundingBox)(ref boundingBox3)).Min;
			num24 = Math.Abs(((Point3d)(ref val15)).X) + num / 2.0;
		}
		double num30 = default(double);
		for (int i = 0; i < num7; i++)
		{
			double num26 = num5;
			double num27 = num24;
			double num28 = i;
			val15 = ((BoundingBox)(ref boundingBox3)).Max;
			double x2 = ((Point3d)(ref val15)).X;
			val15 = ((BoundingBox)(ref boundingBox3)).Min;
			double num29 = num26 + (0.0 - (num27 + num28 * (x2 - ((Point3d)(ref val15)).X + num)) / length);
			if (num29 > 1.0)
			{
				num29 -= 1.0;
			}
			if (num29 < 0.0)
			{
				num29 += 1.0;
			}
			if (val4.NormalizedLengthParameter(num29, ref num30))
			{
				Vector3d val20 = Vector3d.CrossProduct(val4.TangentAt(num30), val3);
				((Vector3d)(ref val20)).Unitize();
				Vector3d destination3 = val20 * num4 + val * (1.0 - num4);
				if (((Vector3d)(ref destination3)).Length < 0.001)
				{
					destination3 = Vector3d.ZAxis;
				}
				else
				{
					((Vector3d)(ref destination3)).Unitize();
				}
				Transform val21 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val4.PointAt(num30), val3, destination3);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, val21 * val12 * val14);
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		if (InGemOnOff.Boolean)
		{
			OutDisplayGems.SetData(cSDataArray);
		}
		return base.Update();
	}
}
