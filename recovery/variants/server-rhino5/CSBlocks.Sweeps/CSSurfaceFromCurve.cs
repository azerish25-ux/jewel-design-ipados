using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;

namespace CSBlocks.Sweeps;

[CSBlock("SurfaceFromCurve", "{DF3C453E-8FE5-470a-B3B8-D1E151382B77}")]
public sealed class CSSurfaceFromCurve : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSOutputChannel oBrep { get; }

	public CSSurfaceFromCurve(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00e5: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		OnNurbsCurve nurbsCurve = iCurve.GetNurbsCurve();
		if (nurbsCurve == null)
		{
			return UpdateMissing("Missing Curve");
		}
		OnPlane val = new OnPlane();
		if (!((OnCurve)nurbsCurve).IsPlanar(val, 0.01))
		{
			return UpdateError("Missing Is Not Planar");
		}
		OnPlaneSurface val2 = new OnPlaneSurface((IOnPlane)(object)val);
		OnBoundingBox val3 = new OnBoundingBox();
		((OnGeometry)nurbsCurve).GetTightBoundingBox(ref val3);
		OnXform val4 = new OnXform();
		val4.Scale((IOn3dPoint)(object)val.PointAt(0.5, 0.5), val3.Diagonal().Length() * 2.0 + 1.0);
		val2.Transform((IOnXform)(object)val4);
		OnCurve[] array = (OnCurve[])(object)new OnCurve[1] { (OnCurve)nurbsCurve };
		OnBrep[] array2 = default(OnBrep[]);
		RhUtil.RhinoCutUpSurface((IOnSurface)val2, false, array, 0.01, 0.01, ref array2);
		OnBrep[] array3 = default(OnBrep[]);
		RhUtil.RhinoCutUpSurface((IOnSurface)val2, true, array, 0.01, 0.01, ref array3);
		bool flag = false;
		if (array2 == null)
		{
			array2 = (OnBrep[])(object)new OnBrep[0];
		}
		if (array3 == null)
		{
			array3 = (OnBrep[])(object)new OnBrep[0];
		}
		if (array2.Length == 0 && array3.Length == 0)
		{
			return UpdateError("RhinoCutUpSurface failed");
		}
		if (array2.Length != 0 && array3.Length != 0)
		{
			OnBoundingBox val5 = new OnBoundingBox();
			((OnGeometry)array2[0]).GetTightBoundingBox(ref val5);
			OnBoundingBox val6 = new OnBoundingBox();
			((OnGeometry)array3[0]).GetTightBoundingBox(ref val6);
			if (val5.Volume() > val6.Volume())
			{
				flag = true;
			}
		}
		else if (array2.Length != 0)
		{
			flag = true;
		}
		if (flag)
		{
			array2[0].ShrinkSurfaces();
			oBrep.SetData(new CSOnGeometryData(array2[0].Convert()));
		}
		else
		{
			array3[0].ShrinkSurfaces();
			oBrep.SetData(new CSOnGeometryData(array3[0].Convert()));
		}
		return base.Update();
	}
}
