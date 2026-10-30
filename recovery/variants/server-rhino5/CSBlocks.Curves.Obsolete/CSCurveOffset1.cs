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
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
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
			OnNurbsCurve val = flattenArray.GetArrayItem(i).GetNurbsCurve().Convert();
			if (val == null)
			{
				return UpdateError("Curve Missing");
			}
			double value2 = 0.0;
			if (iZ != null)
			{
				iZ.GetDouble(ref value2);
			}
			OnCurve val2 = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)val, value, closed: true);
			if (val2 == null)
			{
				return UpdateError("Offset Curve Unable to Create Output");
			}
			OnNurbsCurve val3 = new OnNurbsCurve();
			val2.GetNurbForm(ref val3);
			if (value2 != 0.0)
			{
				OnXform val4 = new OnXform();
				val4.Translation(0.0, 0.0, value2);
				val3.Transform((IOnXform)(object)val4);
				cSDataArray.Append(new CSOnGeometryData(val3.Convert(), val4.Convert(), applyTransform: false));
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData(val3.Convert()));
			}
		}
		oCurve.SetData(cSDataArray);
		return base.Update();
	}
}
