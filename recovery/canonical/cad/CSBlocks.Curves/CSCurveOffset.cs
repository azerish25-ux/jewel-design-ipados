using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Curves;

[CSBlock("CurveOffset", "{A838AA68-6518-4843-AF94-5758B5AD09CB}")]
public sealed class CSCurveOffset : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iOffset { get; }

	public CSInputChannel iZ { get; }

	public CSOutputChannel oCurve { get; }

	public CSCurveOffset(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOffset = new CSInputChannel(GetPropertyName(() => iOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(GetPropertyName(() => iZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Array);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Curves");
		}
		double value = 0.0;
		iOffset.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		double value2 = 0.0;
		if (iZ != null)
		{
			iZ.GetDouble(ref value2);
		}
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				return UpdateMissing("Bad Curves");
			}
			IOn3dPoint origin = OnUtil.On_origin;
			IOn3dVector normalDir = OnUtil.On_zaxis;
			OnPlane onPlane = new OnPlane();
			if (onNurbsCurve.IsPlanar(onPlane))
			{
				OnPlane plane = new OnPlane(OnUtil.On_origin, OnUtil.On_zaxis);
				OnXform onXform = new OnXform();
				onXform.ChangeBasis(plane, onPlane);
				OnXform onXform2 = new OnXform();
				onXform2 = onXform.Inverse();
				OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
				onNurbsCurve2.Transform(onXform);
				OnBoundingBox bbox = new OnBoundingBox();
				onNurbsCurve2.GetBoundingBox(ref bbox);
				On3dPoint on3dPoint = bbox.Center();
				on3dPoint.Transform(onXform2);
				origin = on3dPoint;
				normalDir = onPlane.Normal();
				value = 0.0 - value;
			}
			OnCurve onCurve = CSUtilGeometry.OffsetOld(onNurbsCurve, normalDir, origin, value, closed: true);
			if (onCurve == null)
			{
				return UpdateError("Unable to offset");
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			onCurve.GetNurbForm(ref nurbs_curve);
			if (value2 != 0.0)
			{
				OnXform onXform3 = new OnXform();
				onXform3.Translation(0.0, 0.0, value2);
				nurbs_curve.Transform(onXform3);
				cSDataArray.Append(new CSOnGeometryData(nurbs_curve.Convert(), onXform3.Convert(), applyTransform: false));
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData(nurbs_curve.Convert()));
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
