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
[CSBlock("UnderBezelRailTopInner_V1", "{A4353B42-35A7-423f-AE34-C3B1B25ADEC4}")]
public sealed class CSUnderBezelRailTopInner1 : CSBlock
{
	public CSInputChannel iCurve { get; }

	public CSInputChannel iTopThickness { get; }

	public CSOutputChannel oCurve { get; }

	public CSUnderBezelRailTopInner1(string identifier)
		: base(identifier)
	{
		iCurve = new CSInputChannel(GetPropertyName(() => iCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
		iTopThickness = new CSInputChannel(GetPropertyName(() => iTopThickness), this, ChannelDataType.Double, ChannelArrayType.Individual);
		oCurve = new CSOutputChannel(GetPropertyName(() => oCurve), this, ChannelDataType.Curve, ChannelArrayType.Individual);
	}

	public override bool Update()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		CSDataArray flattenArray = iCurve.GetFlattenArray();
		double value = 0.0;
		iTopThickness.GetDouble(ref value);
		CSDataArray cSDataArray = new CSDataArray();
		Transform transform = default(Transform);
		for (int i = 0; i < flattenArray.GetArrayCount(); i++)
		{
			CSData arrayItem = flattenArray.GetArrayItem(i);
			CSDataHelper.GetTransformNoArray(arrayItem, out var value2);
			if (!((Transform)(ref value2)).IsValid)
			{
				value2 = Transform.Identity;
			}
			((Transform)(ref value2)).TryGetInverse(ref transform);
			OnNurbsCurve val = arrayItem.GetNurbsCurve().Convert();
			if (val == null)
			{
				cSDataArray.Append(new CSData());
				continue;
			}
			OnNurbsCurve obj = val.Duplicate();
			obj.Transform((IOnXform)(object)transform.Convert());
			OnCurve val2 = CSUtilGeometry.OriginOffsetXY((IOnCurve)(object)obj, 0.0 - value, closed: true);
			if (val2 == null)
			{
				CS.CSUtil.Print("Under Bezel Top Inner Offset Curve Unable to Create Output\n");
				cSDataArray.Append(new CSData());
			}
			else
			{
				cSDataArray.Append(new CSOnGeometryData((GeometryBase)(object)val2.Convert(), value2));
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
