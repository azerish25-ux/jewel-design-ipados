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

[CSBlock("GemPlacerSpan", "{CE5690A6-DEBD-4c86-92A9-5F4B6638337E}")]
public sealed class CSGemPlacerSpan : CSBlock
{
	public readonly CSInputChannel InCurve;

	public readonly CSInputChannel InGemDefinition;

	public readonly CSInputChannel InGemSpan;

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

	public readonly CSOutputChannel OutDisplayGem;

	public CSGemPlacerSpan(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InGemDefinition = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Individual);
		InGemSpan = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0.0);
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
		OutDisplayGem = new CSOutputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_0938: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af7: Unknown result type (might be due to invalid IL or missing references)
		NurbsCurve nurbsCurve = InCurve.NurbsCurve;
		CSGemData gem = InGemDefinition.Gem;
		int integer = InGemPlacementMethod.Integer;
		double angleDegrees = InGemSpan.Double;
		angleDegrees = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees);
		double num = InGemSpacing.Double;
		double num2 = InGemZOffset.Double;
		double num3 = InGemYOffset.Double;
		double num4 = InGemTilt.Double;
		num4 /= 100.0;
		double angleDegrees2 = InGemAngle.Double;
		angleDegrees2 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees2);
		double angleDegrees3 = InGemRotation.Double;
		angleDegrees3 = CSBlockUtil.CSUtilMath.ToRadians(angleDegrees3);
		double num5 = InGemPosition.Double;
		bool boolean = InGemYOffsetMirror.Boolean;
		bool boolean2 = InGemRotationMirror.Boolean;
		CSDataArray cSDataArray = new CSDataArray();
		CSDataHelper.GetTransformNoArray(InCurve.Data, out var value);
		if (!((Transform)(ref value)).IsValid)
		{
			value = Transform.Identity;
		}
		Point3d val = default(Point3d);
		((Point3d)(ref val))._002Ector(Point3d.Origin);
		((Point3d)(ref val)).Transform(value);
		Vector3d val2 = default(Vector3d);
		Interval domain;
		Vector3d val4;
		if (InCurve.Data is CSOutsideRingRailData || InCurve.Data is CSRingRailData)
		{
			((Vector3d)(ref val2))._002Ector(Vector3d.ZAxis);
			((Vector3d)(ref val2)).Transform(value);
			((Vector3d)(ref val2)).Unitize();
			Vector3d val3 = val2;
			domain = ((Curve)nurbsCurve).Domain;
			val4 = Vector3d.CrossProduct(val3, ((Curve)nurbsCurve).TangentAt(((Interval)(ref domain)).Min));
			((Vector3d)(ref val4)).Unitize();
		}
		else
		{
			((Vector3d)(ref val2))._002Ector(Vector3d.YAxis);
			((Vector3d)(ref val2)).Transform(value);
			((Vector3d)(ref val2)).Unitize();
			domain = ((Curve)nurbsCurve).Domain;
			val4 = Vector3d.CrossProduct(((Curve)nurbsCurve).TangentAt(((Interval)(ref domain)).Min), val2);
			((Vector3d)(ref val4)).Unitize();
		}
		double zOffset = 0.0;
		CSUtilGem.GetGemZOffset(gem, (CSUtilGem.EGemPlacementMethod)integer, ref zOffset);
		Curve val5 = CSUtilGeometry.OffsetOld((Curve)(object)nurbsCurve, -val4, val, zOffset + num2, closed: true);
		if (val5 == null)
		{
			return UpdateError("Offset Curve");
		}
		double length = val5.GetLength();
		if (length <= 0.0)
		{
			return UpdateError("Offset Curve Length");
		}
		Transform val6 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees2, Vector3d.XAxis, Point3d.Origin);
		Transform val7 = ((!boolean) ? Transform.Translation(0.0, num3, 0.0) : Transform.Translation(0.0, 0.0, 0.0));
		Transform val8 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees3, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0, Vector3d.ZAxis, Point3d.Origin));
		Transform val9 = val6 * val8;
		BoundingBox boundingBox = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox, doGrow: false, val9))
		{
			return UpdateError("Center Gem Bounding Box");
		}
		Transform val10 = ((!boolean) ? Transform.Translation(0.0, num3, 0.0) : Transform.Translation(0.0, 0.0 - num3, 0.0));
		Transform val11 = ((!boolean2) ? CSBlockUtil.CSUtilMath.Rotation(angleDegrees3, Vector3d.ZAxis, Point3d.Origin) : CSBlockUtil.CSUtilMath.Rotation(0.0 - angleDegrees3, Vector3d.ZAxis, Point3d.Origin));
		Transform val12 = val6 * val11;
		BoundingBox boundingBox2 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox2, doGrow: false, val12))
		{
			return UpdateError("Rhs Gem Bounding Box");
		}
		Transform val13 = Transform.Translation(0.0, num3, 0.0);
		Transform val14 = CSBlockUtil.CSUtilMath.Rotation(angleDegrees3, Vector3d.ZAxis, Point3d.Origin);
		Transform val15 = val6 * val14;
		BoundingBox boundingBox3 = BoundingBox.Unset;
		if (!gem.GetTightBoundingBox(ref boundingBox3, doGrow: false, val15))
		{
			return UpdateError("Lhs Gem Bounding Box");
		}
		LineCurve val16 = new LineCurve(val, val + new Vector3d(0.0, 0.0, length));
		((GeometryBase)val16).Rotate(angleDegrees / 2.0, Vector3d.YAxis, Point3d.Origin);
		CurveIntersections val17 = Intersection.CurveCurve(val5, (Curve)(object)val16, CSApplicationHelper.Application.Tolerance, CSApplicationHelper.Application.Tolerance);
		if (val17.Count < 1)
		{
			return UpdateError("Intersection");
		}
		double tolerance = CSApplicationHelper.Application.Tolerance;
		domain = val5.Domain;
		double length2 = val5.GetLength(tolerance, new Interval(((Interval)(ref domain)).Min, val17[0].ParameterA));
		if (double.IsNaN(length2) || length2 < 0.0)
		{
			return UpdateError("Intersection Length");
		}
		Point3d val18 = ((BoundingBox)(ref boundingBox2)).Max;
		double num6 = Math.Abs(((Point3d)(ref val18)).X);
		val18 = ((BoundingBox)(ref boundingBox2)).Min;
		int num7 = (int)(length2 / (num6 + Math.Abs(((Point3d)(ref val18)).X) + num));
		val18 = ((BoundingBox)(ref boundingBox2)).Max;
		double num8 = Math.Abs(((Point3d)(ref val18)).X);
		val18 = ((BoundingBox)(ref boundingBox2)).Min;
		double num9 = length2 % (num8 + Math.Abs(((Point3d)(ref val18)).X) + num);
		val18 = ((BoundingBox)(ref boundingBox3)).Max;
		double num10 = Math.Abs(((Point3d)(ref val18)).X);
		val18 = ((BoundingBox)(ref boundingBox3)).Min;
		int num11 = (int)(length2 / (num10 + Math.Abs(((Point3d)(ref val18)).X) + num));
		val18 = ((BoundingBox)(ref boundingBox3)).Max;
		double num12 = Math.Abs(((Point3d)(ref val18)).X);
		val18 = ((BoundingBox)(ref boundingBox3)).Min;
		double num13 = length2 % (num12 + Math.Abs(((Point3d)(ref val18)).X) + num);
		double num14 = num9 + num13;
		val18 = ((BoundingBox)(ref boundingBox)).Max;
		double num15 = Math.Abs(((Point3d)(ref val18)).X);
		val18 = ((BoundingBox)(ref boundingBox)).Min;
		int num16 = (int)(num14 / (num15 + Math.Abs(((Point3d)(ref val18)).X)));
		int num17 = (num7 + num11) / 2;
		if (num16 > 0)
		{
			double num18 = num5;
			val18 = ((BoundingBox)(ref boundingBox)).Max;
			double num19 = Math.Abs(((Point3d)(ref val18)).X);
			val18 = ((BoundingBox)(ref boundingBox)).Min;
			num5 = num18 - (num19 - Math.Abs(((Point3d)(ref val18)).X)) / length / 2.0;
		}
		else
		{
			double num20 = num5;
			val18 = ((BoundingBox)(ref boundingBox)).Max;
			double num21 = Math.Abs(((Point3d)(ref val18)).X);
			val18 = ((BoundingBox)(ref boundingBox)).Min;
			num5 = num20 - (num21 - Math.Abs(((Point3d)(ref val18)).X)) / length;
		}
		double num23;
		if (num16 > 0)
		{
			if (boolean2)
			{
				val18 = ((BoundingBox)(ref boundingBox)).Max;
				double num22 = Math.Abs(((Point3d)(ref val18)).X);
				val18 = ((BoundingBox)(ref boundingBox2)).Min;
				num23 = num22 + Math.Abs(((Point3d)(ref val18)).X) + num;
			}
			else
			{
				val18 = ((BoundingBox)(ref boundingBox)).Max;
				double num24 = Math.Abs(((Point3d)(ref val18)).X);
				val18 = ((BoundingBox)(ref boundingBox2)).Min;
				num23 = num24 + Math.Abs(((Point3d)(ref val18)).X) + num;
			}
		}
		else if (boolean2)
		{
			val18 = ((BoundingBox)(ref boundingBox2)).Min;
			num23 = Math.Abs(((Point3d)(ref val18)).X) + num / 2.0;
		}
		else
		{
			val18 = ((BoundingBox)(ref boundingBox2)).Max;
			num23 = Math.Abs(((Point3d)(ref val18)).X) + num / 2.0;
		}
		double num30 = default(double);
		for (int num25 = num17 - 1; num25 >= 0; num25--)
		{
			double num26 = num5;
			double num27 = num23;
			double num28 = num25;
			val18 = ((BoundingBox)(ref boundingBox2)).Max;
			double x = ((Point3d)(ref val18)).X;
			val18 = ((BoundingBox)(ref boundingBox2)).Min;
			double num29 = num26 + (num27 + num28 * (x - ((Point3d)(ref val18)).X + num)) / length;
			if (num29 > 1.0)
			{
				num29 -= 1.0;
			}
			if (num29 < 0.0)
			{
				num29 += 1.0;
			}
			if (val5.NormalizedLengthParameter(num29, ref num30))
			{
				Vector3d val19 = Vector3d.CrossProduct(val5.TangentAt(num30), val4);
				((Vector3d)(ref val19)).Unitize();
				Vector3d destination = val19 * num4 + val2 * (1.0 - num4);
				if (((Vector3d)(ref destination)).Length < 0.001)
				{
					((Vector3d)(ref destination))._002Ector(Vector3d.ZAxis);
				}
				else
				{
					((Vector3d)(ref destination)).Unitize();
				}
				Transform val20 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val5.PointAt(num30), val4, destination);
				CSData data = gem.Duplicate();
				CSDataHelper.Transform(data, val20 * val10 * val12);
				cSDataArray.Append(data);
			}
		}
		if (num16 > 0)
		{
			double num31 = num5;
			if (num31 > 1.0)
			{
				num31 -= 1.0;
			}
			if (num31 < 0.0)
			{
				num31 += 1.0;
			}
			double num32 = default(double);
			if (!val5.NormalizedLengthParameter(num31, ref num32))
			{
				return UpdateError("Normalized Arc Length Point");
			}
			Vector3d val21 = Vector3d.CrossProduct(val5.TangentAt(num32), val4);
			((Vector3d)(ref val21)).Unitize();
			Vector3d destination2 = val21 * num4 + val2 * (1.0 - num4);
			if (((Vector3d)(ref destination2)).Length < 0.001)
			{
				((Vector3d)(ref destination2))._002Ector(Vector3d.ZAxis);
			}
			else
			{
				((Vector3d)(ref destination2)).Unitize();
			}
			Transform val22 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val5.PointAt(num32), val4, destination2);
			CSData data2 = gem.Duplicate();
			CSDataHelper.Transform(data2, val22 * val7 * val9);
			cSDataArray.Append(data2);
		}
		double num34;
		if (num16 > 0)
		{
			if (boolean2)
			{
				val18 = ((BoundingBox)(ref boundingBox)).Min;
				double num33 = Math.Abs(((Point3d)(ref val18)).X);
				val18 = ((BoundingBox)(ref boundingBox3)).Max;
				num34 = num33 + Math.Abs(((Point3d)(ref val18)).X) + num;
			}
			else
			{
				val18 = ((BoundingBox)(ref boundingBox)).Min;
				double num35 = Math.Abs(((Point3d)(ref val18)).X);
				val18 = ((BoundingBox)(ref boundingBox3)).Max;
				num34 = num35 + Math.Abs(((Point3d)(ref val18)).X) + num;
			}
		}
		else if (boolean2)
		{
			val18 = ((BoundingBox)(ref boundingBox3)).Max;
			num34 = Math.Abs(((Point3d)(ref val18)).X) + num / 2.0;
		}
		else
		{
			val18 = ((BoundingBox)(ref boundingBox3)).Min;
			num34 = Math.Abs(((Point3d)(ref val18)).X) + num / 2.0;
		}
		double num40 = default(double);
		for (int i = 0; i < num17; i++)
		{
			double num36 = num5;
			double num37 = num34;
			double num38 = i;
			val18 = ((BoundingBox)(ref boundingBox3)).Max;
			double x2 = ((Point3d)(ref val18)).X;
			val18 = ((BoundingBox)(ref boundingBox3)).Min;
			double num39 = num36 + (0.0 - (num37 + num38 * (x2 - ((Point3d)(ref val18)).X + num)) / length);
			if (num39 > 1.0)
			{
				num39 -= 1.0;
			}
			if (num39 < 0.0)
			{
				num39 += 1.0;
			}
			if (val5.NormalizedLengthParameter(num39, ref num40))
			{
				Vector3d val23 = Vector3d.CrossProduct(val5.TangentAt(num40), val4);
				((Vector3d)(ref val23)).Unitize();
				Vector3d destination3 = val23 * num4 + val2 * (1.0 - num4);
				if (((Vector3d)(ref destination3)).Length < 0.001)
				{
					((Vector3d)(ref destination3))._002Ector(Vector3d.ZAxis);
				}
				else
				{
					((Vector3d)(ref destination3)).Unitize();
				}
				Transform val24 = CSBlockUtil.CSUtilMath.Orient3Point(Point3d.Origin, gem.North, gem.Up, val5.PointAt(num40), val4, destination3);
				CSData data3 = gem.Duplicate();
				CSDataHelper.Transform(data3, val24 * val13 * val15);
				cSDataArray.Append(data3);
			}
		}
		OutGem.SetData(cSDataArray);
		if (InGemOnOff.Boolean)
		{
			OutDisplayGem.SetData(cSDataArray);
		}
		return base.Update();
	}
}
