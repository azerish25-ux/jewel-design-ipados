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
		BoundingBox boundingBox = nurbsCurve.GetBoundingBox(accurate: true);
		if (!boundingBox.IsValid)
		{
			return UpdateError("Bounding Box Failed");
		}
		double xScaleFactor = value2 / boundingBox.Max.X;
		double yScaleFactor = value / boundingBox.Max.Y;
		Transform xform = Transform.Scale(Plane.WorldXY, xScaleFactor, yScaleFactor, 1.0);
		nurbsCurve.Transform(xform);
		CSData outputSurface = null;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			if (flattenArray.GetArrayItem(i) is CSGemData cSGemData)
			{
				CSBlockUtil.CSUtilMath.GetUnscaledXforms(cSGemData.Xform, out var xUp, out var xDown);
				NurbsCurve nurbsCurve2 = cSGemData.GetNurbsCurve();
				nurbsCurve2.Transform(xDown);
				if (!PatternCutter(xUp, nurbsCurve2, nurbsCurve.DuplicateCurve().ToNurbsCurve(), value3, value4, ref outputSurface))
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
		Point3d pointAtStart = profile.PointAtStart;
		Vector3d motion = new Vector3d(pointAtStart);
		motion.Reverse();
		Transform transform = Transform.Translation(motion);
		Transform transform2 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.XAxis, Point3d.Origin);
		Transform transform3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), Vector3d.ZAxis, Point3d.Origin);
		profile.Transform(transform3 * transform2 * transform);
		Point3d pointAtStart2 = curve.PointAtStart;
		transform = Transform.Translation(pointAtStart2.X, pointAtStart2.Y + girdleSpacing, pointAtStart2.Z + zOffset);
		profile.Transform(transform);
		NurbsSurface nurbsSurface = NurbsSurface.CreateRailRevolvedSurface(profile, curve, new Line(new Point3d(0.0, 0.0, -1.0), new Point3d(0.0, 0.0, 1.0)), scaleHeight: false);
		nurbsSurface.Transform(xUp);
		outputSurface = new CSOnGeometryData(nurbsSurface, xUp, applyTransform: false);
		return true;
	}
}
