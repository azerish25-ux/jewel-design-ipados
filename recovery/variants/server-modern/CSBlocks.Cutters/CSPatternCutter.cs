using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("PatternCutter", "{03C668E2-EB14-4c84-BAB9-DCA94EE03D29}")]
public sealed class CSPatternCutter : CSBlock
{
	public CSInputChannel InGem { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InHeight { get; }

	public CSInputChannel InThickness { get; }

	public CSInputChannel InZOffset { get; }

	public CSInputChannel InGirdleSpacing { get; }

	public CSOutputChannel OutSurface { get; }

	public CSPatternCutter(string identifier)
		: base(identifier)
	{
		InGem = new CSInputChannel(this, ChannelDataType.Gem, ChannelArrayType.Array);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InThickness = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		InGirdleSpacing = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutSurface = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Array);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InGem.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Pattern Cutter:  Missing Gem");
		}
		NurbsCurve nurbsCurve = InProfile.NurbsCurve;
		double value = 0.0;
		InHeight.GetDouble(ref value);
		double value2 = 0.0;
		InThickness.GetDouble(ref value2);
		double value3 = 0.0;
		InZOffset.GetDouble(ref value3);
		double value4 = 0.0;
		InGirdleSpacing.GetDouble(ref value4);
		BoundingBox boundingBox = ((GeometryBase)nurbsCurve).GetBoundingBox(true);
		if (!((BoundingBox)(ref boundingBox)).IsValid)
		{
			return UpdateError("Bounding Box Failed");
		}
		double num = value2;
		Point3d max = ((BoundingBox)(ref boundingBox)).Max;
		double num2 = num / ((Point3d)(ref max)).X;
		double num3 = value;
		max = ((BoundingBox)(ref boundingBox)).Max;
		double num4 = num3 / ((Point3d)(ref max)).Y;
		Transform val = Transform.Scale(Plane.WorldXY, num2, num4, 1.0);
		((GeometryBase)nurbsCurve).Transform(val);
		CSData outputSurface = null;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
			{
				CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
				NurbsCurve nurbsCurve2 = cSGemData.GetNurbsCurve();
				((GeometryBase)nurbsCurve2).Transform(xDown);
				if (!PatternCutter(xUp, nurbsCurve2, ((Curve)nurbsCurve).DuplicateCurve().ToNurbsCurve(), value3, value4, ref outputSurface))
				{
					return UpdateError("PatternCutter Failed");
				}
				cSDataArray.Append(outputSurface);
			}
		}
		OutSurface.SetData(cSDataArray);
		return base.Update();
	}

	private static bool PatternCutter(Transform xUp, NurbsCurve curve, NurbsCurve profile, double zOffset, double girdleSpacing, ref CSData outputSurface)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		Point3d pointAtStart = ((Curve)profile).PointAtStart;
		Vector3d val = default(Vector3d);
		((Vector3d)(ref val))._002Ector(pointAtStart);
		((Vector3d)(ref val)).Reverse();
		Transform val2 = Transform.Translation(val);
		Transform val3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform val4 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		((GeometryBase)profile).Transform(val4 * val3 * val2);
		Point3d pointAtStart2 = ((Curve)curve).PointAtStart;
		val2 = Transform.Translation(((Point3d)(ref pointAtStart2)).X, ((Point3d)(ref pointAtStart2)).Y + girdleSpacing, ((Point3d)(ref pointAtStart2)).Z + zOffset);
		((GeometryBase)profile).Transform(val2);
		NurbsSurface val5 = NurbsSurface.CreateRailRevolvedSurface((Curve)(object)profile, (Curve)(object)curve, new Line(new Point3d(0.0, 0.0, -1.0), new Point3d(0.0, 0.0, 1.0)), false);
		((GeometryBase)val5).Transform(xUp);
		outputSurface = new CSOnGeometryData((GeometryBase)(object)val5, xUp, applyTransform: false);
		return true;
	}
}
