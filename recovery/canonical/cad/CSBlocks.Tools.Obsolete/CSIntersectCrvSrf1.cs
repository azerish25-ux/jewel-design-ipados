using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("IntersectCrvSrf_V1", "{957DF594-76DE-41FA-A2D8-75A1DB102455}")]
public sealed class CSIntersectCrvSrf1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iSurface { get; }

	public CSOutputChannel oPoints { get; }

	public CSOutputChannel oParameters { get; }

	public CSIntersectCrvSrf1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSurface = new CSInputChannel(GetPropertyName(() => iSurface), this, ChannelDataType.Surface | ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
		oPoints = new CSOutputChannel(GetPropertyName(() => oPoints), this, ChannelDataType.Point, ChannelArrayType.Individual);
		oParameters = new CSOutputChannel(GetPropertyName(() => oParameters), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsSurface onNurbsSurface = iSurface.GetNurbsSurface();
		OnNurbsCurve nurbsCurve = iSurface.GetNurbsCurve();
		if (onNurbsSurface == null)
		{
			CSDataArray flattenArray2 = iSurface.GetFlattenArray();
			if (flattenArray2.GetArrayCount() > 0)
			{
				onNurbsSurface = flattenArray2.GetArrayItem(0).GetNurbsSurface().Convert();
			}
		}
		if (onNurbsSurface == null && nurbsCurve == null)
		{
			return UpdateMissing("Missing Surface");
		}
		OnNurbsSurface onNurbsSurface2 = null;
		if (onNurbsSurface != null)
		{
			onNurbsSurface2 = onNurbsSurface;
		}
		else if (nurbsCurve != null)
		{
			OnPlane onPlane = new OnPlane();
			if (nurbsCurve.IsPlanar(onPlane))
			{
				OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
				On3dPoint fixed_point = onPlane.PointAt(0.5, 0.5);
				OnXform onXform = new OnXform();
				onXform.Scale(fixed_point, 1000.0);
				onPlaneSurface.Transform(onXform);
				onNurbsSurface2 = onPlaneSurface.NurbsSurface();
			}
		}
		if (onNurbsSurface2 == null)
		{
			return UpdateError("Unable to use Intersect Surface");
		}
		ArrayOnX_EVENT x = new ArrayOnX_EVENT();
		onNurbsCurve.IntersectSurface(onNurbsSurface2, ref x);
		if (x.Count() > 0)
		{
			int num = -1;
			double num2 = 1000.0;
			for (int i = 0; i < x.Count(); i++)
			{
				if (!x[i].IsOverlapEvent())
				{
					double num3 = x[i].get_m_a(0);
					if (num3 < num2)
					{
						num = i;
						num2 = num3;
					}
				}
			}
			if (num >= 0)
			{
				OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
				onNurbsCurve2.Trim(new OnInterval(onNurbsCurve2.Domain().Min(), x[num].get_m_a(0)));
				double length = 0.0;
				onNurbsCurve2.GetLength(ref length);
				double length2 = 0.0;
				onNurbsCurve.GetLength(ref length2);
				On3dPoint pt = x[num].get_m_pointA(0);
				oPoints.SetData(new CSPointData(pt.Convert()));
				oParameters.SetData(new CSNumberData(length / length2));
			}
		}
		return base.Update();
	}
}
