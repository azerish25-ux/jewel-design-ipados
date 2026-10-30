using System;
using CS;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using RMA.Rhino;
using Rhino.Geometry;

namespace CSBlocks.Cutters.Obsolete;

[Obsolete]
[CSBlock("BezelCutter_V1", "{F90C2092-BB10-4fa3-B23D-8E0B53622FD1}")]
public sealed class CSBezelCutter1 : CSBlock
{
	public CSInputChannel iRail1 { get; }

	public CSInputChannel iRail2 { get; }

	public CSOutputChannel oBrep { get; }

	public CSBezelCutter1(string identifier)
		: base(identifier)
	{
		iRail1 = new CSInputChannel(GetPropertyName(() => iRail1), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iRail2 = new CSInputChannel(GetPropertyName(() => iRail2), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		oBrep = new CSOutputChannel(GetPropertyName(() => oBrep), this, ChannelDataType.Brep, ChannelArrayType.Array | ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_018a: Expected O, but got Unknown
		CSDataArray flattenArray = iRail1.GetFlattenArray();
		CSDataArray flattenArray2 = iRail2.GetFlattenArray();
		if (flattenArray.GetArrayCount() != flattenArray2.GetArrayCount())
		{
			return UpdateError("Input counts do not match");
		}
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSData arrayItem2 = flattenArray2.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value);
			if (!value.IsValid)
			{
				value = Transform.Identity;
			}
			value.TryGetInverse(out var inverseTransform);
			OnNurbsCurve val = arrayItem.GetNurbsCurve().ToNurbsCurve().Convert();
			if (val != null)
			{
				val.Transform((IOnXform)(object)inverseTransform.Convert());
				OnNurbsCurve val2 = arrayItem2.GetNurbsCurve().ToNurbsCurve().Convert();
				if (val2 != null)
				{
					val2.Transform((IOnXform)(object)inverseTransform.Convert());
					OnXform val3 = new OnXform();
					On3dPoint val4 = ((OnCurve)val).PointAtStart();
					val3.Translation(0.0, 0.0, 5.0);
					val4.Transform((IOnXform)(object)val3);
					On3dPoint val5 = ((OnCurve)val2).PointAtStart();
					val3.Translation(0.0, 0.0, -5.0);
					val5.Transform((IOnXform)(object)val3);
					On3dPointArray val6 = new On3dPointArray();
					((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)val5);
					((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)((OnCurve)val2).PointAtStart());
					((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)((OnCurve)val).PointAtStart());
					((ArrayOn3dPoint)val6).Append((IOn3dPoint)(object)val4);
					OnPolylineCurve val7 = new OnPolylineCurve((IOn3dPointArray)val6);
					OnNurbsSurface val8 = new OnNurbsSurface();
					RhUtil.RhinoRailRevolve((IOnCurve)val7, (IOnCurve)(object)val, (IOnLine)new OnLine(OnUtil.On_origin, (IOn3dPoint)new On3dPoint(OnUtil.On_zaxis)), false, val8);
					OnBrep val9 = CSUtilRhino.CapPlanarHoles(((OnGeometry)val8).BrepForm(), CSApplicationHelper.Application.Tolerance);
					((OnGeometry)val9).Transform((IOnXform)(object)value.Convert());
					cSDataArray.Append(new CSOnGeometryData(val9.Convert(), value, applyTransform: false));
				}
			}
		}
		oBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
