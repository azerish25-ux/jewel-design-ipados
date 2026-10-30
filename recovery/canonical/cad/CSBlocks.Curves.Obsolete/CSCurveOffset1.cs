using System;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSBlockUtil;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;

namespace CSBlocks.Curves.Obsolete;

[Obsolete]
[CSBlock("CurveOffset_V1", "{BE554417-68CD-46af-841E-66BBC72D2A4B}")]
public sealed class CSCurveOffset1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iOffset { get; }

	public CSInputChannel iZ { get; }

	public CSOutputChannel oCurve { get; }

	public CSCurveOffset1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iOffset = new CSInputChannel(GetPropertyName(() => iOffset), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iZ = new CSInputChannel(GetPropertyName(() => iZ), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		if (flattenArray == null)
		{
			return UpdateMissing("Curve Missing");
		}
		double value = 0.0;
		iOffset.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			OnNurbsCurve onNurbsCurve = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				return UpdateError("Curve Missing");
			}
			double value2 = 0.0;
			if (iZ != null)
			{
				iZ.GetDouble(ref value2);
			}
			OnCurve onCurve = CSUtilGeometry.OriginOffsetXY(onNurbsCurve, value, closed: true);
			if (onCurve == null)
			{
				return UpdateError("Offset Curve Unable to Create Output");
			}
			OnNurbsCurve nurbs_curve = new OnNurbsCurve();
			onCurve.GetNurbForm(ref nurbs_curve);
			if (value2 != 0.0)
			{
				OnXform onXform = new OnXform();
				onXform.Translation(0.0, 0.0, value2);
				nurbs_curve.Transform(onXform);
				cSDataArray.Append(new CSOnGeometryData(nurbs_curve.Convert(), onXform.Convert(), applyTransform: false));
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
