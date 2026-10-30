using System.Collections.Generic;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSDatas;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace CSBlocks.Cutters;

[CSBlock("AddProfileCutterOnTopOfCutter", "{90804880-967D-4f72-B913-8C0728BDE7A7}")]
public sealed class CSAddProfileCutterOnTopOfCutter : CSBlock
{
	public CSInputChannel InObject { get; }

	public CSInputChannel InProfileTop { get; }

	public CSInputChannel InProfileBottom { get; }

	public CSInputChannel InHeight { get; }

	public CSOutputChannel OutCutter { get; }

	public CSAddProfileCutterOnTopOfCutter(string identifier)
		: base(identifier)
	{
		InObject = new CSInputChannel(this, ChannelDataType.Brep, ChannelArrayType.Individual);
		InProfileTop = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InProfileBottom = new CSInputChannel(this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InHeight = new CSInputChannel(this, ChannelDataType.Double, ChannelArrayType.Individual);
		OutCutter = new CSOutputChannel(this, ChannelDataType.Surface, ChannelArrayType.Individual);
		SetChannelIdentifiers(autoCheckInputs: true);
	}

	public override bool Update()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		double num = InHeight.Double;
		Brep brep = InObject.Brep;
		Transform val = Transform.Scale(Plane.WorldXY, 1.0, 1.0, 0.0);
		NurbsCurve nurbsCurve = InProfileBottom.NurbsCurve;
		((GeometryBase)nurbsCurve).Transform(val);
		NurbsCurve nurbsCurve2 = InProfileTop.NurbsCurve;
		((GeometryBase)nurbsCurve2).Transform(val);
		BoundingBox boundingBox = ((GeometryBase)brep).GetBoundingBox(true);
		NurbsCurve val2 = ((Curve)nurbsCurve).DuplicateCurve().ToNurbsCurve();
		NurbsCurve val3 = ((Curve)nurbsCurve).DuplicateCurve().ToNurbsCurve();
		Point3d val4 = ((BoundingBox)(ref boundingBox)).Min;
		((GeometryBase)val2).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val4)).Z));
		val4 = ((BoundingBox)(ref boundingBox)).Max;
		((GeometryBase)val3).Translate(new Vector3d(0.0, 0.0, ((Point3d)(ref val4)).Z));
		Brep[] array = Brep.CreateFromLoft((IEnumerable<Curve>)(object)new Curve[2]
		{
			(Curve)val2,
			(Curve)val3
		}, Point3d.Unset, Point3d.Unset, (LoftType)3, false);
		if (array[0] == null)
		{
			return UpdateError("Loft of intersect surface");
		}
		Curve[] array2 = default(Curve[]);
		Point3d[] array3 = default(Point3d[]);
		if (!Intersection.BrepBrep(array[0], brep, CSApplicationHelper.Application.Tolerance, ref array2, ref array3))
		{
			return UpdateError("RhinoIntersectBreps failed");
		}
		if (array2 == null)
		{
			return UpdateError("RhinoIntersectBreps failed to produce results");
		}
		Curve[] array4 = Curve.JoinCurves((IEnumerable<Curve>)array2);
		if (array4 == null)
		{
			return UpdateError("RhinoMergeCurves failed to produce results");
		}
		BoundingBox val5 = default(BoundingBox);
		double num2 = double.MaxValue;
		double num3 = double.MinValue;
		for (int i = 0; i < array4.Length; i++)
		{
			val5 = ((GeometryBase)array4[i]).GetBoundingBox(true);
			double num4 = num2;
			val4 = ((BoundingBox)(ref val5)).Max;
			if (num4 > ((Point3d)(ref val4)).Z)
			{
				val4 = ((BoundingBox)(ref val5)).Max;
				num2 = ((Point3d)(ref val4)).Z;
			}
			double num5 = num3;
			val4 = ((BoundingBox)(ref val5)).Max;
			if (num5 < ((Point3d)(ref val4)).Z)
			{
				val4 = ((BoundingBox)(ref val5)).Max;
				num3 = ((Point3d)(ref val4)).Z;
			}
		}
		Curve[] array5 = (Curve[])(object)new Curve[2];
		if (num > 0.0)
		{
			array5[0] = ((Curve)nurbsCurve).DuplicateCurve();
			((GeometryBase)array5[0]).Translate(new Vector3d(0.0, 0.0, num2));
			array5[1] = ((Curve)nurbsCurve2).DuplicateCurve();
			((GeometryBase)array5[1]).Translate(new Vector3d(0.0, 0.0, num3 + num));
		}
		else
		{
			array5[0] = ((Curve)nurbsCurve).DuplicateCurve();
			((GeometryBase)array5[0]).Translate(new Vector3d(0.0, 0.0, num2 + num));
			array5[1] = ((Curve)nurbsCurve2).DuplicateCurve();
			((GeometryBase)array5[1]).Translate(new Vector3d(0.0, 0.0, num3));
		}
		Brep[] array6 = Brep.CreateFromLoft((IEnumerable<Curve>)array5, Point3d.Unset, Point3d.Unset, (LoftType)3, true);
		OutCutter.SetData(new CSOnGeometryData((GeometryBase)(object)array6[0]));
		return base.Update();
	}
}
