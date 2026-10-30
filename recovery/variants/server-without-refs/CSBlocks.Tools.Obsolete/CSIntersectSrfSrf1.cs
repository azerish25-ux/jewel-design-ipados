using System;
using CS;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Tools.Obsolete;

[Obsolete]
[CSBlock("IntersectSrfSrf_V1", "{C4BFDB8B-E567-4544-88F9-5242B92CB400}")]
public sealed class CSIntersectSrfSrf1 : CSBlock
{
	public CSOutputChannel OutCurves { get; }

	public CSInputChannel InSurface { get; }

	public CSInputChannel InIntersector { get; }

	public CSIntersectSrfSrf1(string identifier)
		: base(identifier)
	{
		InSurface = new CSInputChannel(GetPropertyName(() => InSurface), this, ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		InIntersector = new CSInputChannel(GetPropertyName(() => InIntersector), this, ChannelDataType.Curve | ChannelDataType.Surface, ChannelArrayType.Array | ChannelArrayType.Individual);
		OutCurves = new CSOutputChannel(GetPropertyName(() => OutCurves), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InSurface.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface 1");
		}
		OnNurbsSurface val = flattenArray.GetArrayItem(0).GetNurbsSurface().Convert();
		if (val == null)
		{
			return UpdateMissing("Missing Surface 1");
		}
		CSDataArray flattenArray2 = InIntersector.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Missing Surface 2");
		}
		OnNurbsSurface val2 = flattenArray2.GetArrayItem(0).GetNurbsSurface().Convert();
		OnNurbsCurve val3 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val2 == null && val3 == null)
		{
			return UpdateMissing("Missing Surface 2");
		}
		OnNurbsSurface val4 = null;
		if (val2 != null)
		{
			val4 = val2;
		}
		else
		{
			OnPlane val5 = new OnPlane();
			if (((OnCurve)val3).IsPlanar(val5))
			{
				On3dPoint val6 = val5.PointAt(0.5, 0.5);
				OnXform val7 = new OnXform();
				val7.Scale((IOn3dPoint)(object)val6, 1000.0);
				OnPlaneSurface val8 = new OnPlaneSurface((IOnPlane)(object)val5);
				val8.Transform((IOnXform)(object)val7);
				val4 = ((OnSurface)val8).NurbsSurface();
			}
		}
		if (val4 == null)
		{
			return UpdateError("Unable to use Intersect Surface");
		}
		OnCurve[] array = default(OnCurve[]);
		On3dPointArray val9 = default(On3dPointArray);
		if (!RhUtil.RhinoIntersectSurfaces((IOnSurface)(object)val, (IOnSurface)(object)val2, CSApplicationHelper.Application.Tolerance, ref array, ref val9))
		{
			return UpdateError("Unable to intersect");
		}
		CSDataArray cSDataArray = new CSDataArray();
		OnCurve[] array2 = array;
		foreach (OnCurve crv in array2)
		{
			cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)crv.Convert()));
		}
		OutCurves.SetData(cSDataArray);
		return base.Update();
	}
}
