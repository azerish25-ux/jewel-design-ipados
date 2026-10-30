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
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		BoundingBox val = default(BoundingBox);
		Transform val2 = default(Transform);
		Transform val3 = default(Transform);
		val = ((GeometryBase)profile).GetBoundingBox(true);
		Point3d val4 = ((BoundingBox)(ref val)).Center;
		double x = ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref val)).Center;
		Point3d val5 = default(Point3d);
		((Point3d)(ref val5))._002Ector(x, ((Point3d)(ref val4)).Y, 0.0);
		Vector3d val6 = default(Vector3d);
		((Vector3d)(ref val6))._002Ector(val5);
		((Vector3d)(ref val6)).Reverse();
		val2 = Transform.Translation(val6);
		val3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(90.0), new Vector3d(1.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		val2 = val3 * val2;
		((GeometryBase)profile).Transform(val2);
		NurbsCurve val7 = ((Curve)profile).DuplicateCurve().ToNurbsCurve();
		NurbsCurve obj = ((Curve)profile).DuplicateCurve().ToNurbsCurve();
		val4 = ((BoundingBox)(ref val)).Max;
		double num = widthInside / ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref val)).Max;
		double num2 = heightInside / ((Point3d)(ref val4)).Y;
		val2 = Transform.Scale(Plane.WorldXY, num, 1.0, num2);
		((GeometryBase)val7).Transform(val2);
		val4 = ((BoundingBox)(ref val)).Max;
		num = widthOutside / ((Point3d)(ref val4)).X;
		val4 = ((BoundingBox)(ref val)).Max;
		num2 = heightOutside / ((Point3d)(ref val4)).Y;
		val2 = Transform.Scale(Plane.WorldXY, num, 1.0, num2);
		((GeometryBase)obj).Transform(val2);
		val2 = Transform.Translation(new Vector3d(0.0, length, 0.0));
		((GeometryBase)obj).Transform(val2);
		Brep val8 = CSUtilRhino.CapPlanarHoles(((Surface)NurbsSurface.CreateRuledSurface((Curve)(object)obj, (Curve)(object)val7)).ToBrep(), CSApplicationHelper.Application.Tolerance);
		val3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(angle), new Vector3d(1.0, 0.0, 0.0), new Point3d(0.0, 0.0, 0.0));
		if (val8 == null)
		{
			return false;
		}
		((GeometryBase)val8).Transform(val3);
		val2 = Transform.Translation(xOffset, yOffset, zOffset);
		((GeometryBase)val8).Transform(val2);
		double num3 = 0.0 + rotation;
		double num4 = 360.0 / count;
		for (int i = 0; (double)i < count; i++)
		{
			Brep val9 = val8.DuplicateBrep();
			val3 = Transform.Rotation(CSBlockUtil.CSUtilMath.ToRadians(num3), new Vector3d(0.0, 0.0, 1.0), new Point3d(0.0, 0.0, 0.0));
			((GeometryBase)val9).Transform(xUp * val3);
			outputSurface.Append(new CSOnGeometryData((GeometryBase)(object)val9, xUp * val3, applyTransform: false));
			num3 += num4;
		}
		return true;
	}
}
