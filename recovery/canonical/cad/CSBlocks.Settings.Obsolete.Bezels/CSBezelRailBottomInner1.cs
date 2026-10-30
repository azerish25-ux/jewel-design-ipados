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
[CSBlock("BezelRailBottomInner_V1", "{C9CC249E-0CCD-498c-93B0-96480FB6B232}")]
public sealed class CSBezelRailBottomInner1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iBaseThickness { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelRailBottomInner1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Array | ChannelArrayType.Individual);
		iBaseThickness = new CSInputChannel(GetPropertyName(() => iBaseThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		iBaseThickness.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
			if (!value2.IsValid)
			{
				value2 = Transform.Identity;
			}
			value2.TryGetInverse(out var inverseTransform);
			OnNurbsCurve onNurbsCurve = arrayItem.GetNurbsCurve().Convert();
			if (onNurbsCurve == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			onNurbsCurve2.Transform(inverseTransform.Convert());
			OnCurve onCurve = CSUtilGeometry.OriginOffsetXY(onNurbsCurve2, 0.0 - value, closed: true);
			if (onCurve == null)
			{
				CS.CSUtil.PrintLine("Bezel Rail Bottom Inner Offset Curve Unable to Create Output");
				cSDataArray.Append(new CSData());
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData(onCurve.Convert(), value2));
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
