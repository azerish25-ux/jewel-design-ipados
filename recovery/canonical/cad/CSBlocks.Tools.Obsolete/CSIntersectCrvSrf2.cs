using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("IntersectCrvSrf_V2", "{D6A2B68B-DABA-4F50-8992-507AEC63D087}")]
public sealed class CSIntersectCrvSrf2 : CSBlock
{
	public CSInputChannel InCurve { get; }

	public CSInputChannel InIntersector { get; }

	public CSOutputChannel OutPoints { get; }

	public CSOutputChannel OutParameters { get; }

	public CSIntersectCrvSrf2(string identifier)
		: base(identifier)
	{
		InCurve = new CSInputChannel(GetPropertyName(() => InCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		InIntersector = new CSInputChannel(GetPropertyName(() => InIntersector), this, ChannelDataType.Any, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutPoints = new CSOutputChannel(GetPropertyName(() => OutPoints), this, ChannelDataType.Point, ChannelArrayType.Individual);
		OutParameters = new CSOutputChannel(GetPropertyName(() => OutParameters), this, ChannelDataType.Double, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = InCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsCurve == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataArray flattenArray2 = InIntersector.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Surface");
		}
		OnNurbsSurface onNurbsSurface = flattenArray2.GetArrayItem(0).GetNurbsSurface().Convert();
		OnNurbsCurve onNurbsCurve2 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (onNurbsSurface == null && onNurbsCurve2 == null)
		{
			return UpdateMissing("Surface");
		}
		OnNurbsSurface onNurbsSurface2 = null;
		if (onNurbsSurface != null)
		{
			onNurbsSurface2 = onNurbsSurface;
		}
		else
		{
			OnPlane onPlane = new OnPlane();
			if (onNurbsCurve2.IsPlanar(onPlane))
			{
				On3dPoint fixed_point = onPlane.PointAt(0.5, 0.5);
				OnXform onXform = new OnXform();
				onXform.Scale(fixed_point, 1000.0);
				OnPlaneSurface onPlaneSurface = new OnPlaneSurface(onPlane);
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
				OnNurbsCurve onNurbsCurve3 = onNurbsCurve.Duplicate();
				onNurbsCurve3.Trim(new OnInterval(onNurbsCurve3.Domain().Min(), x[num].get_m_a(0)));
				double length = 0.0;
				onNurbsCurve3.GetLength(ref length);
				double length2 = 0.0;
				onNurbsCurve.GetLength(ref length2);
				On3dPoint pt = x[num].get_m_pointA(0);
				OutPoints.SetData(new CSPointData(pt.Convert()));
				OutParameters.SetData(new CSNumberData(length / length2));
				return base.Update();
			}
			return UpdateError("No Valid Output Set");
		}
		return UpdateError("No Intersection");
	}
}
