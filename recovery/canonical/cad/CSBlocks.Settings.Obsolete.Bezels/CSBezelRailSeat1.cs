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
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("BezelRailSeat_V1", "{9DAF23C3-6347-4674-A7F4-328BD7126363}")]
public sealed class CSBezelRailSeat1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iSeatAngle { get; }

	public CSInputChannel iSeatLength { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelRailSeat1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iSeatAngle = new CSInputChannel(GetPropertyName(() => iSeatAngle), this, ChannelDataType.Double, ChannelArrayType.Individual);
		iSeatLength = new CSInputChannel(GetPropertyName(() => iSeatLength), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		iSeatAngle.GetDouble(ref value);
		double value2 = 0.0;
		iSeatLength.GetDouble(ref value2);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value3);
			if (!value3.IsValid)
			{
				value3 = Transform.Identity;
			}
			value3.TryGetInverse(out var inverseTransform);
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			onNurbsCurve2.Transform(inverseTransform.Convert());
			double num = CSBlockUtil.CSUtilMath.ToRadians(value);
			double distance = (0.0 - Math.Sin(num)) * value2;
			double dz = (0.0 - Math.Cos(num)) * value2;
			OnXform onXform = new OnXform();
			onXform.Translation(0.0, 0.0, dz);
			OnCurve onCurve = CSUtilGeometry.OriginOffsetXY(onNurbsCurve2, distance, closed: true);
			if (onCurve == null)
			{
				CS.CSUtil.PrintLine("Bezel Rail Seat Offset Curve Unable to Create Output");
				cSDataArray.Append(new CSData());
			}
			else
			{
				onCurve.Transform(onXform);
				cSDataArray.Append(new CSOnGeometryData(onCurve.Convert(), value3));
			}
		}
		if (cSDataArray.GetArrayCount() == 1)
		{
			oCurve.SetData(cSDataArray.GetArrayItem(0));
		}
		else
		{
			oCurve.SetData(cSDataArray);
		}
		return base.Update();
	}
}
