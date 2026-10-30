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
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray.GetArrayCount() <= 0)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsCurve val = flattenArray.GetArrayItem(0).GetNurbsCurve().Convert();
		if (val == null)
		{
			return UpdateMissing("Curve");
		}
		OnNurbsSurface val2 = iSurface.GetNurbsSurface();
		OnNurbsCurve nurbsCurve = iSurface.GetNurbsCurve();
		if (val2 == null)
		{
			CSDataArray flattenArray2 = iSurface.GetFlattenArray();
			if (flattenArray2.GetArrayCount() > 0)
			{
				val2 = flattenArray2.GetArrayItem(0).GetNurbsSurface().Convert();
			}
		}
		if (val2 == null && nurbsCurve == null)
		{
			return UpdateMissing("Missing Surface");
		}
		OnNurbsSurface val3 = null;
		if (val2 != null)
		{
			val3 = val2;
		}
		else if (nurbsCurve != null)
		{
			OnPlane val4 = new OnPlane();
			if (((OnCurve)nurbsCurve).IsPlanar(val4))
			{
				OnPlaneSurface val5 = new OnPlaneSurface((IOnPlane)(object)val4);
				On3dPoint val6 = val4.PointAt(0.5, 0.5);
				OnXform val7 = new OnXform();
				val7.Scale((IOn3dPoint)(object)val6, 1000.0);
				val5.Transform((IOnXform)(object)val7);
				val3 = ((OnSurface)val5).NurbsSurface();
			}
		}
		if (val3 == null)
		{
			return UpdateError("Unable to use Intersect Surface");
		}
		ArrayOnX_EVENT val8 = new ArrayOnX_EVENT();
		((OnCurve)val).IntersectSurface((IOnSurface)(object)val3, ref val8);
		if (val8.Count() > 0)
		{
			int num = -1;
			double num2 = 1000.0;
			for (int i = 0; i < val8.Count(); i++)
			{
				if (!val8[i].IsOverlapEvent())
				{
					double num3 = val8[i][0];
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
				((OnCurve)obj).Trim((IOnInterval)new OnInterval(((OnCurve)obj).Domain().Min(), val8[num][0]));
				double num4 = 0.0;
				((OnCurve)obj).GetLength(ref num4);
				double num5 = 0.0;
				((OnCurve)val).GetLength(ref num5);
				On3dPoint pt = val8[num][0];
				oPoints.SetData(new CSPointData(pt.Convert()));
				oParameters.SetData(new CSNumberData(num4 / num5));
			}
		}
		return base.Update();
	}
}
