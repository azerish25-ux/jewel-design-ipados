using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSDatas;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("LineFromBrep", "{9076B94E-E404-47cc-996E-B61B0FC2101F}")]
public sealed class CSLineFromBrep : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSInputChannel iCurve1;

	public readonly CSInputChannel iCurve2;

	public readonly CSInputChannel iParam1;

	public readonly CSInputChannel iParam2;

	public readonly CSOutputChannel oCurve;

	public readonly CSOutputChannel oTick1;

	public readonly CSOutputChannel oTick2;

	public CSLineFromBrep(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		iCurve1 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iCurve2 = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iParam1 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		iParam2 = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick1 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oTick2 = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		CSDataHelper.GetBrep(iBrep.GetFlattenArray().GetArrayItem(0), out var value);
		if (value == null)
		{
			return UpdateMissing("No Brep Surface Input");
		}
		CSDataHelper.GetNurbsCurve(iCurve1.Data, out var value2);
		if (value2 == null)
		{
			return UpdateMissing("Curve1 Missing");
		}
		CSDataHelper.GetNurbsCurve(iCurve2.Data, out var value3);
		if (value3 == null)
		{
			return UpdateMissing("No Curve Input");
		}
		double value4 = 0.0;
		iParam1.GetDouble(ref value4);
		double value5 = 0.0;
		iParam2.GetDouble(ref value5);
		double num = default(double);
		((Curve)value2).NormalizedLengthParameter(value4, ref num);
		double num2 = default(double);
		((Curve)value3).NormalizedLengthParameter(value5, ref num2);
		Point3d val = ((Curve)value2).PointAt(num);
		Point3d val2 = ((Curve)value3).PointAt(num2);
		if (!CSUtilGeometry.GetClosestPointToBrep(value, val, out var faceIndex, out var u, out var v))
		{
			return UpdateError("Closest Point 1");
		}
		if (!CSUtilGeometry.GetClosestPointToBrep(value, val2, out var faceIndex2, out var u2, out var v2))
		{
			return UpdateError("Closest Point 2");
		}
		if (faceIndex != faceIndex2 && faceIndex2 != -1 && faceIndex != -1)
		{
			BrepFace val3 = value.Faces[faceIndex];
			BrepFace val4 = value.Faces[faceIndex2];
			double num3 = default(double);
			double num4 = default(double);
			((Surface)val4).ClosestPoint(val, ref num3, ref num4);
			double num5 = default(double);
			double num6 = default(double);
			((Surface)val3).ClosestPoint(val2, ref num5, ref num6);
			Point3d val5 = ((Surface)val4).PointAt(num3, num4);
			double num7 = ((Point3d)(ref val5)).DistanceTo(((Surface)val3).PointAt(u, v));
			val5 = ((Surface)val3).PointAt(num5, num6);
			if (num7 < ((Point3d)(ref val5)).DistanceTo(((Surface)val4).PointAt(u2, v2)))
			{
				faceIndex = faceIndex2;
				u = num3;
				v = num4;
			}
			else
			{
				faceIndex2 = faceIndex;
				u2 = num5;
				v2 = num6;
			}
		}
		if (faceIndex >= 0)
		{
			BrepFace obj = value.Faces[faceIndex];
			Point3d val6 = ((Surface)obj).PointAt(u, v);
			Vector3d val7 = ((Surface)obj).NormalAt(u, v);
			if (value.Faces[faceIndex].OrientationIsReversed)
			{
				((Vector3d)(ref val7)).Reverse();
			}
			LineCurve geometry = new LineCurve(val6, val6 + val7);
			oTick1.SetData(new CSOnGeometryData((GeometryBase)(object)geometry));
		}
		if (faceIndex2 >= 0)
		{
			BrepFace obj2 = value.Faces[faceIndex2];
			Point3d val8 = ((Surface)obj2).PointAt(u2, v2);
			Vector3d val9 = ((Surface)obj2).NormalAt(u2, v2);
			if (value.Faces[faceIndex2].OrientationIsReversed)
			{
				((Vector3d)(ref val9)).Reverse();
			}
			LineCurve geometry2 = new LineCurve(val8, val8 + val9);
			oTick2.SetData(new CSOnGeometryData((GeometryBase)(object)geometry2));
		}
		if (faceIndex == faceIndex2 && faceIndex >= 0)
		{
			BrepFace obj3 = value.Faces[faceIndex];
			LineCurve val10 = new LineCurve(new Point3d(u, v, 0.0), new Point3d(u2, v2, 0.0));
			Curve geometry3 = ((Surface)obj3).Pushup((Curve)(object)val10, 0.01);
			oCurve.SetData(new CSOnGeometryData((GeometryBase)(object)geometry3));
			return base.Update();
		}
		return UpdateError("Unable to create line");
	}
}
