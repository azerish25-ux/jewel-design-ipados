using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Cutters;

[CSBlock("ScallopCutter", "{03DCD2BF-9123-464f-9ABD-E52EBC148D9E}")]
public sealed class CSScallopCutter : CSBlock
{
	public CSInputChannel InBezel { get; }

	public CSInputChannel InProfile { get; }

	public CSInputChannel InCount { get; }

	public CSInputChannel InHeightInside { get; }

	public CSInputChannel InHeightOutside { get; }

	public CSInputChannel InWidthInside { get; }

	public CSInputChannel InWidthOutside { get; }

	public CSInputChannel InLength { get; }

	public CSInputChannel InAngle { get; }

	public CSInputChannel InZOffset { get; }

	public CSInputChannel InXOffset { get; }

	public CSInputChannel InYOffset { get; }

	public CSInputChannel InRotation { get; }

	public CSOutputChannel OutBrep { get; }

	public CSScallopCutter(string identifier)
		: base(identifier)
	{
		InBezel = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array);
		InProfile = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InCount = new CSInputChannel(this, ChannelDataType.Integer, ChannelArrayType.Individual, 1);
		InHeightInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InHeightOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InWidthInside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InWidthOutside = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InLength = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InAngle = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InZOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InXOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InYOffset = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		InRotation = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual, 0);
		OutBrep = new CSOutputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InBezel.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			UpdateMissing("Missing Bezel Datas");
		}
		int integer = InCount.Integer;
		double heightInside = InHeightInside.Double;
		double heightOutside = InHeightOutside.Double;
		double widthInside = InWidthInside.Double;
		double widthOutside = InWidthOutside.Double;
		double length = InLength.Double;
		double angle = InAngle.Double;
		double zOffset = InZOffset.Double;
		double xOffset = InXOffset.Double;
		double yOffset = InYOffset.Double;
		double rotation = InRotation.Double;
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSDataHelper.GetTransformNoArray(flattenArray.GetArrayItem(i), out var value);
			CSBlockUtil.CSUtilMath.GetUnscaledXform(value, out var xUp);
			NurbsCurve nurbsCurve = InProfile.NurbsCurve;
			CSDataArray outputSurface = new CSDataArray();
			if (!ScallopCutter(xUp, nurbsCurve, integer, heightInside, heightOutside, widthInside, widthOutside, length, angle, zOffset, xOffset, yOffset, rotation, ref outputSurface))
			{
				return UpdateError("ScallopCutter Function");
			}
			cSDataArray.Append(outputSurface);
		}
		OutBrep.SetData(cSDataArray);
		return base.Update();
	}

	private static bool ScallopCutter(Transform xUp, NurbsCurve profile, double count, double heightInside, double heightOutside, double widthInside, double widthOutside, double length, double angle, double zOffset, double xOffset, double yOffset, double rotation, ref CSDataArray outputSurface)
	{
		BoundingBox boundingBox = default(BoundingBox);
		Transform transform = default(Transform);
		Transform transform2 = default(Transform);
		boundingBox = profile.GetBoundingBox(accurate: true);
		Point3d point = new Point3d(boundingBox.Center.X, boundingBox.Center.Y, 0.0);
		Vector3d motion = new Vector3d(point);
		motion.Reverse();
		transform = Transform.Translation(motion);
		transform2 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), new Vector3d(1.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		transform = transform2 * transform;
		profile.Transform(transform);
		NurbsCurve nurbsCurve = profile.DuplicateCurve().ToNurbsCurve();
		NurbsCurve nurbsCurve2 = profile.DuplicateCurve().ToNurbsCurve();
		transform = Transform.Scale(xScaleFactor: widthInside / boundingBox.Max.X, zScaleFactor: heightInside / boundingBox.Max.Y, plane: Plane.WorldXY, yScaleFactor: 1.0);
		nurbsCurve.Transform(transform);
		transform = Transform.Scale(xScaleFactor: widthOutside / boundingBox.Max.X, zScaleFactor: heightOutside / boundingBox.Max.Y, plane: Plane.WorldXY, yScaleFactor: 1.0);
		nurbsCurve2.Transform(transform);
		transform = Transform.Translation(new Vector3d(0.0, length, 0.0));
		nurbsCurve2.Transform(transform);
		Brep brep = CSUtilRhino.CapPlanarHoles(NurbsSurface.CreateRuledSurface(nurbsCurve2, nurbsCurve).ToBrep(), CSApplicationHelper.Application.Tolerance);
		transform2 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle), new Vector3d(1.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		if (brep == null)
		{
			return false;
		}
		brep.Transform(transform2);
		transform = Transform.Translation(xOffset, yOffset, zOffset);
		brep.Transform(transform);
		double num = 0.0 + rotation;
		double num2 = 360.0 / count;
		for (int i = 0; (double)i < count; i++)
		{
			Brep brep2 = brep.DuplicateBrep();
			transform2 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num), new Vector3d(0.0, 0.0, 1.0), new Point3d(0.0, 0.0, 0.0));
			brep2.Transform(xUp * transform2);
			outputSurface.Append(new CSOnGeometryData(brep2, xUp * transform2, applyTransform: false));
			num += num2;
		}
		return true;
	}
}
