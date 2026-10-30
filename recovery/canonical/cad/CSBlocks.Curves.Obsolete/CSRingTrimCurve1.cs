using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("RingTrimCurve_V1", "{2A6B01E2-2B48-471c-B31A-709E265DF9F8}")]
public sealed class CSRingTrimCurve1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iStartAngle { get; }

	public CSInputChannel iEndAngle { get; }

	public CSOutputChannel oCurves { get; }

	public CSRingTrimCurve1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iStartAngle = new CSInputChannel(GetPropertyName(() => iStartAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iEndAngle = new CSInputChannel(GetPropertyName(() => iEndAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurves = new CSOutputChannel(GetPropertyName(() => oCurves), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		if (iCurve == null)
		{
			return UpdateMissing("Curve Input Missing");
		}
		CSData data = iCurve.Data;
		if (data == null)
		{
			return UpdateError("Curve Input Invalid");
		}
		CSDataArray flattenArray = CSDataHelper.GetFlattenArray(data);
		double value = 10.0;
		double value2 = 45.0;
		if (iStartAngle != null)
		{
			iStartAngle.GetDouble(ref value);
		}
		if (iEndAngle != null)
		{
			iEndAngle.GetDouble(ref value2);
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				continue;
			}
			OnBoundingBox bbox = new OnBoundingBox();
			onNurbsCurve.GetBoundingBox(ref bbox);
			OnPlaneSurface onPlaneSurface = new OnPlaneSurface(new OnPlane(OnUtil.On_origin, OnUtil.On_xaxis));
			onPlaneSurface.Translate(new On3dPoint(OnUtil.On_origin) - onPlaneSurface.PointAt(0.5, 0.0));
			double num = Math.Max(bbox.m_max.x, 0.0 - bbox.m_min.x) + Math.Max(bbox.m_max.z, 0.0 - bbox.m_min.z) + 1.0;
			double num2 = Math.Max(bbox.m_max.y, 0.0 - bbox.m_min.y) + 1.0;
			OnXform onXform = new OnXform();
			onXform.Scale(1.0, num2 * 2.1, num * 2.1);
			onPlaneSurface.Transform(onXform);
			OnPlaneSurface onPlaneSurface2 = new OnPlaneSurface(onPlaneSurface);
			onPlaneSurface.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value), OnUtil.On_yaxis, OnUtil.On_origin);
			onPlaneSurface2.Rotate(CSBlockUtil.CSUtilMath.ToRadians(value2), OnUtil.On_yaxis, OnUtil.On_origin);
			ArrayOnX_EVENT x = new ArrayOnX_EVENT();
			onNurbsCurve.IntersectSurface(onPlaneSurface, ref x);
			if (x.Count() > 0)
			{
				if (onNurbsCurve.IsClosed())
				{
					onNurbsCurve.ChangeClosedCurveSeam(x[0].get_m_a(0));
					x.Empty();
					onNurbsCurve.IntersectSurface(onPlaneSurface2, ref x);
					if (x.Count() > 0)
					{
						onNurbsCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), x[0].get_m_a(0)));
					}
				}
				else
				{
					ArrayOnX_EVENT x2 = new ArrayOnX_EVENT();
					onNurbsCurve.IntersectSurface(onPlaneSurface2, ref x2);
					if (x2.Count() > 0)
					{
						onNurbsCurve.Trim(new OnInterval(x[0].get_m_a(0), x2[0].get_m_a(0)));
					}
					else
					{
						onNurbsCurve.Trim(new OnInterval(x[0].get_m_a(0), onNurbsCurve.Domain().Max()));
					}
				}
			}
			else
			{
				x.Empty();
				onNurbsCurve.IntersectSurface(onPlaneSurface2, ref x);
				if (x.Count() > 0)
				{
					onNurbsCurve.Trim(new OnInterval(onNurbsCurve.Domain().Min(), x[0].get_m_a(0)));
				}
			}
			if (flattenArray.GetArrayCount() > 1)
			{
				cSDataArray.Append(new CSOnGeometryData(onNurbsCurve.Convert()));
			}
			else
			{
				oCurves.SetData(new CSOnGeometryData(onNurbsCurve.Convert()));
			}
		}
		if (flattenArray.GetArrayCount() > 1)
		{
			oCurves.SetData(cSDataArray);
		}
		return base.Update();
	}
}
