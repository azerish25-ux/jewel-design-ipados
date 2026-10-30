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
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		if (iBrep == null)
		{
			return UpdateMissing("Missing Surface");
		}
		OnBrep val = iBrep.GetBrep();
		if (val == null)
		{
			OnNurbsSurface nurbsSurface = iBrep.GetNurbsSurface();
			if (nurbsSurface != null)
			{
				val = ((OnGeometry)nurbsSurface).BrepForm();
			}
		}
		if (val == null)
		{
			return UpdateMissing("Missing Surface");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnBoundingBox val2 = new OnBoundingBox();
		for (int i = 0; i < val.m_T.Count(); i++)
		{
			OnNurbsCurve val3 = ((OnCurve)val.m_T[i]).NurbsCurve();
			OnNurbsCurve val4 = new OnNurbsCurve(3, true, val3.Order(), val3.CVCount());
			for (int j = 0; j < val3.CVCount(); j++)
			{
				On4dPoint val5 = new On4dPoint();
				val3.GetCV(j, ref val5);
				val4.SetCV(j, (IOn4dPoint)(object)val5);
			}
			for (int k = 0; k < val3.KnotCount(); k++)
			{
				val4.SetKnot(k, val3.Knot(k));
			}
			((OnCurve)val4).SetDomain((IOnInterval)(object)((OnCurve)val3).Domain());
			((OnGeometry)val4).GetTightBoundingBox(ref val2, 1);
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val4.Convert()));
		}
		Transform transform = Transform.Translation(Point3d.Origin - val2.Min().Convert());
		cSDataArray.ApplyTransform(transform);
		oArrayOfCurves.SetData(cSDataArray);
		return base.Update();
	}
}
