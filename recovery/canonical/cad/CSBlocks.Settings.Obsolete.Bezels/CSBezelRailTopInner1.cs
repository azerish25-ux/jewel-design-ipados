using System;
using CS.CSDataUtilities;
using CS.CSObjects.CSBlocks;
using CS.Parametric;
using CS.Parametric.Channels;
using CSCompatibility;
using CSDatas;
using RMA.OpenNURBS;
using Rhino.Geometry;

namespace CSBlocks.Settings.Obsolete.Bezels;

[Obsolete]
[CSBlock("BezelRailTopInner_V1", "{08E072FF-3622-48a6-89C5-781938914401}")]
public sealed class CSBezelRailTopInner1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iSeatHeight { get; }

	public CSOutputChannel oCurve { get; }

	public CSBezelRailTopInner1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iSeatHeight = new CSInputChannel(GetPropertyName(() => iSeatHeight), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		iSeatHeight.GetDouble(ref value);
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
			OnXform onXform = new OnXform();
			onXform.Translation(0.0, 0.0, value);
			OnNurbsCurve onNurbsCurve2 = onNurbsCurve.Duplicate();
			onNurbsCurve2.Transform(onXform * inverseTransform.Convert());
			cSDataArray.Append(new CSOnGeometryData(onNurbsCurve2.Convert(), value2));
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
