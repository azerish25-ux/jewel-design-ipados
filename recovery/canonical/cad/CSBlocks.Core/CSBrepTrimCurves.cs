using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Core;

[CSBlock("BrepTrimCurves", "{79DEF231-7681-489d-A390-080519DCD4B8}")]
public sealed class CSBrepTrimCurves : CSBlock
{
	public readonly CSInputChannel iBrep;

	public readonly CSOutputChannel oArrayOfCurves;

	public CSBrepTrimCurves(string identifier)
		: base(identifier)
	{
		iBrep = new CSInputChannel(this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Individual);
		oArrayOfCurves = new CSOutputChannel(this, ChannelDataType.Curve, ChannelArrayType.Array);
		SetChannelIdentifiers();
	}

	public override bool Update()
	{
		if (iBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		OnBrep onBrep = iBrep.GetBrep();
		if (onBrep == null)
		{
			OnNurbsSurface nurbsSurface = iBrep.GetNurbsSurface();
			if (nurbsSurface != null)
			{
				onBrep = nurbsSurface.BrepForm();
			}
		}
		if (onBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnBoundingBox tight_bbox = new OnBoundingBox();
		for (int i = 0; i < onBrep.m_T.Count(); i++)
		{
			OnNurbsCurve onNurbsCurve = onBrep.m_T[i].NurbsCurve();
			OnNurbsCurve onNurbsCurve2 = new OnNurbsCurve(3, bIsRational: true, onNurbsCurve.Order(), onNurbsCurve.CVCount());
			for (int j = 0; j < onNurbsCurve.CVCount(); j++)
			{
				On4dPoint vertex_value = new On4dPoint();
				onNurbsCurve.GetCV(j, ref vertex_value);
				onNurbsCurve2.SetCV(j, vertex_value);
			}
			for (int k = 0; k < onNurbsCurve.KnotCount(); k++)
			{
				onNurbsCurve2.SetKnot(k, onNurbsCurve.Knot(k));
			}
			onNurbsCurve2.SetDomain(onNurbsCurve.Domain());
			onNurbsCurve2.GetTightBoundingBox(ref tight_bbox, 1);
			cSDataArray.Append(new CSOnGeometryData(onNurbsCurve2.Convert()));
		}
		Transform transform = Transform.Translation(Point3d.Origin - tight_bbox.Min().Convert());
		cSDataArray.ApplyTransform(transform);
		oArrayOfCurves.SetData(cSDataArray);
		return base.Update();
	}
}
