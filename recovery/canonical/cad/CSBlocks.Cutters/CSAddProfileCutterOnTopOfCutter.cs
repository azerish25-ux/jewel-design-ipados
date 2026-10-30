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
		double num = InHeight.Double;
		Brep brep = InObject.Brep;
		Transform xform = Transform.Scale(Plane.WorldXY, 1.0, 1.0, 0.0);
		NurbsCurve nurbsCurve = InProfileBottom.NurbsCurve;
		nurbsCurve.Transform(xform);
		NurbsCurve nurbsCurve2 = InProfileTop.NurbsCurve;
		nurbsCurve2.Transform(xform);
		BoundingBox boundingBox = brep.GetBoundingBox(accurate: true);
		NurbsCurve nurbsCurve3 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
		NurbsCurve nurbsCurve4 = nurbsCurve.DuplicateCurve().ToNurbsCurve();
		nurbsCurve3.Translate(new Vector3d(0.0, 0.0, boundingBox.Min.Z));
		nurbsCurve4.Translate(new Vector3d(0.0, 0.0, boundingBox.Max.Z));
		Brep[] array = Brep.CreateFromLoft(new Curve[2] { nurbsCurve3, nurbsCurve4 }, Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: false);
		if (array[0] == null)
		{
			return UpdateError("Loft of intersect surface");
		}
		if (!Intersection.BrepBrep(array[0], brep, CSApplicationHelper.Application.Tolerance, out var intersectionCurves, out var _))
		{
			return UpdateError("RhinoIntersectBreps failed");
		}
		if (intersectionCurves == null)
		{
			return UpdateError("RhinoIntersectBreps failed to produce results");
		}
		Curve[] array2 = Curve.JoinCurves(intersectionCurves);
		if (array2 == null)
		{
			return UpdateError("RhinoMergeCurves failed to produce results");
		}
		BoundingBox boundingBox2 = default(BoundingBox);
		double num2 = double.MaxValue;
		double num3 = double.MinValue;
		for (int i = 0; i < array2.Length; i++)
		{
			boundingBox2 = array2[i].GetBoundingBox(accurate: true);
			if (num2 > boundingBox2.Max.Z)
			{
				num2 = boundingBox2.Max.Z;
			}
			if (num3 < boundingBox2.Max.Z)
			{
				num3 = boundingBox2.Max.Z;
			}
		}
		Curve[] array3 = new Curve[2];
		if (num > 0.0)
		{
			array3[0] = nurbsCurve.DuplicateCurve();
			array3[0].Translate(new Vector3d(0.0, 0.0, num2));
			array3[1] = nurbsCurve2.DuplicateCurve();
			array3[1].Translate(new Vector3d(0.0, 0.0, num3 + num));
		}
		else
		{
			array3[0] = nurbsCurve.DuplicateCurve();
			array3[0].Translate(new Vector3d(0.0, 0.0, num2 + num));
			array3[1] = nurbsCurve2.DuplicateCurve();
			array3[1].Translate(new Vector3d(0.0, 0.0, num3));
		}
		Brep[] array4 = Brep.CreateFromLoft(array3, Point3d.Unset, Point3d.Unset, LoftType.Straight, closed: true);
		OutCutter.SetData(new CSOnGeometryData(array4[0]));
		return base.Update();
	}
}
