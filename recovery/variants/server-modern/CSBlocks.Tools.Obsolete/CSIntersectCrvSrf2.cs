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
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = InCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		CSDataArray flattenArray2 = InIntersector.GetFlattenArray();
		if (flattenArray2.GetArrayCount() <= 0)
		{
			return UpdateMissing("Surface");
		}
		OnNurbsSurface val2 = flattenArray2.GetArrayItem(0).GetNurbsSurface().Convert();
		OnNurbsCurve val3 = flattenArray2.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val2 == null && val3 == null)
		{
			return UpdateMissing("Surface");
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
		ArrayOnX_EVENT val9 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectSurface((IOnSurface)(object)val4, ref val9);
		if (val9.Count() > 0)
		{
			int num = -1;
			double num2 = 1000.0;
			for (int i = 0; i < val9.Count(); i++)
			{
				if (!val9[i].IsOverlapEvent())
				{
					double num3 = val9[i][0];
					if (num3 < num2)
					{
						num = i;
						num2 = num3;
					}
				}
			}
			if (num >= 0)
			{
				OnNurbsCurve obj = val.Duplicate();
				((OnCurve)obj).Trim((IOnInterval)new OnInterval(((OnCurve)obj).Domain().Min(), val9[num][0]));
				double num4 = 0.0;
				((OnCurve)obj).GetLength(ref num4);
				double num5 = 0.0;
				((OnCurve)val).GetLength(ref num5);
				On3dPoint pt = val9[num][0];
				OutPoints.SetData(new CSPointData(pt.Convert()));
				OutParameters.SetData(new CSNumberData(num4 / num5));
				return base.Update();
			}
			return UpdateError("No Valid Output Set");
		}
		return UpdateError("No Intersection");
	}
}
