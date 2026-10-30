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
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().ToNurbsCurve().Convert();
			if (onNurbsCurve != null)
			{
				onNurbsCurve.Transform(inverseTransform.Convert());
				OnNurbsCurve onNurbsCurve2 = arrayItem2.GetNurbsCurve().ToNurbsCurve().Convert();
				if (onNurbsCurve2 != null)
				{
					onNurbsCurve2.Transform(inverseTransform.Convert());
					OnXform onXform = new OnXform();
					On3dPoint on3dPoint = onNurbsCurve.PointAtStart();
					onXform.Translation(0.0, 0.0, 5.0);
					on3dPoint.Transform(onXform);
					On3dPoint on3dPoint2 = onNurbsCurve2.PointAtStart();
					onXform.Translation(0.0, 0.0, -5.0);
					on3dPoint2.Transform(onXform);
					On3dPointArray on3dPointArray = new On3dPointArray();
					on3dPointArray.Append(on3dPoint2);
					on3dPointArray.Append(onNurbsCurve2.PointAtStart());
					on3dPointArray.Append(onNurbsCurve.PointAtStart());
					on3dPointArray.Append(on3dPoint);
					OnPolylineCurve profile_curve = new OnPolylineCurve(on3dPointArray);
					OnNurbsSurface onNurbsSurface = new OnNurbsSurface();
					RhUtil.RhinoRailRevolve(profile_curve, onNurbsCurve, new OnLine(OnUtil.On_origin, new On3dPoint(OnUtil.On_zaxis)), bScaleHeight: false, onNurbsSurface);
					OnBrep onBrep = CSUtilRhino.CapPlanarHoles(onNurbsSurface.BrepForm(), CSApplicationHelper.Application.Tolerance);
					onBrep.Transform(value.Convert());
					cSDataArray.Append(new CSOnGeometryData(onBrep.Convert(), value, applyTransform: false));
				}
			}
		}
		oBrep.SetData((cSDataArray.GetArrayCount() == 1) ? cSDataArray.GetArrayItem(0) : cSDataArray);
		return base.Update();
	}
}
